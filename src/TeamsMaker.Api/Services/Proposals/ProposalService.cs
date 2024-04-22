﻿using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Proposal;
using TeamsMaker.Api.Contracts.Responses.Proposal;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Proposals.Interfaces;

namespace TeamsMaker.Api.Services.Proposals;

public class ProposalService(AppDBContext db, IUserInfo userInfo,
    ICircleValidationService circleValidationService) : IProposalService
{
    public async Task<GetProposalResponse> GetAsync(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Proposal)
            .SingleOrDefaultAsync(c => c.Id == circleId, ct) ??
            throw new InvalidDataException("Circle Id is not valid");

        var proposal = new GetProposalResponse
        {
            Id = circle.Proposal.Id,
            CircleId = circle.Id,
            Overview = circle.Proposal.Overview,
            Objectives = circle.Proposal.Objectives,
            TechStack = circle.Proposal.TechStack,
            Status = circle.Proposal.Status,
        };


        throw new NotImplementedException();
    }

    public async Task<Guid> AddAsync(AddProposalRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Proposal)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == request.CircleId, ct) ??
            throw new InvalidDataException("Circle Id is not valid");

        if (circle.Proposal is not null) throw new InvalidOperationException("There is already a proposal");

        var member = circle.CircleMembers
            .Where(cm => cm.UserId == userInfo.UserId)
            .SingleOrDefault() ??
            throw new InvalidDataException("Member Id is not valid");

        circleValidationService.CheckPermission(member, circle, PermissionsEnum.ProposalManagement);

        var proposal = new Proposal
        {
            Overview = request.Overview,
            Objectives = request.Objectives,
            TechStack = request.TechStack,
            CircleId = request.CircleId
        };

        await db.Proposals.AddAsync(proposal, ct);
        await db.SaveChangesAsync(ct);

        return proposal.Id;
    }


    public async Task UpdateAsync(Guid id, UpdateProposalRequest request, CancellationToken ct)
    {
        var proposal = await db.Proposals
            .Include(p => p.Circle)
                .ThenInclude(c => c.CircleMembers.FirstOrDefault(cm => cm.UserId == userInfo.UserId))
            .SingleOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidDataException("Proposal not found");

        circleValidationService.CheckPermission(proposal.Circle.CircleMembers.First(), proposal.Circle, PermissionsEnum.ProposalManagement);

        if (proposal.Status != ProposalStatusEnum.NoApproval)
            throw new InvalidOperationException("Reset your proposal approval status to update it");

        if (!string.IsNullOrEmpty(request.Overview)) proposal.Overview = request.Overview;

        if (!string.IsNullOrEmpty(request.Objectives)) proposal.Objectives = request.Objectives;

        if (!string.IsNullOrEmpty(request.TechStack)) proposal.TechStack = request.TechStack;


        await db.SaveChangesAsync(ct);
    }

    // notifications
    public async Task ResetAsync(Guid id, CancellationToken ct)
    {
        var proposal = await db.Proposals
            .Include(p => p.Circle)
                .ThenInclude(c => c.CircleMembers.FirstOrDefault(cm => cm.UserId == userInfo.UserId))
            .Include(p => p.ApprovalRequests)
                .ThenInclude(ar => ar.Supervisor)
            .SingleOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidDataException("Proposal not found");

        circleValidationService.CheckPermission(proposal.Circle.CircleMembers.First(), proposal.Circle, PermissionsEnum.ProposalManagement);

        //TODO: 1st, 2nd approval
        if (proposal.Status != ProposalStatusEnum.FirstApproval && proposal.Status != ProposalStatusEnum.SecondApproval)
            throw new InvalidOperationException("Reset your proposal approval status to update it");

        foreach (var approval in proposal.ApprovalRequests.Where(ar => ar.IsAccepted))
        {
            approval.IsAccepted = false;

            if (approval.Supervisor is not null)
            {
                approval.Supervisor = null;
                approval.SupervisorId = null;
            }

            approval.IsReseted = true;
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var proposal = await db.Proposals
            .Include(p => p.ApprovalRequests)
            .SingleOrDefaultAsync(p => p.Id == id, ct) ?? throw new InvalidDataException("proposal not found");

        if (proposal.ApprovalRequests.Any(ar => ar.IsAccepted))
            throw new InvalidOperationException("This proposal already approved");

        db.ApprovalRequests.RemoveRange(proposal.ApprovalRequests);
        db.Proposals.Remove(proposal);

        await db.SaveChangesAsync(ct);
    }

}
