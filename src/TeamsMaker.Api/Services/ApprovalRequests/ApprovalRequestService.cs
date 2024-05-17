using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.ApprovalRequest;
using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Proposal;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Services.ApprovalRequests;

public class ApprovalRequestService
    (AppDBContext db, IUserInfo userInfo, IServiceProvider serviceProvider, ICircleValidationService validationService) : IApprovalRequestService
{
    public async Task<Guid> AddAsync(AddApprovalRequest request, CancellationToken ct)
    {
        // Permission Validation
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == request.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circle.Id, ct);
        validationService.CheckPermission(circleMember, circle, PermissionsEnum.ProposalManagement);

        // Assure that the proposal is exists
        var proposal = await db.Proposals
            .Include(p => p.ApprovalRequests)
            .SingleOrDefaultAsync(p => p.CircleId == circle.Id, ct) ??
            throw new ArgumentException("Make Your Proposal First");

        // Assure that the staff Id is as same as the required position
        await MatchStaffWithPosition(request, ct);

        if ((proposal.Status == ProposalStatusEnum.FirstApproval && request.Position == PositionEnum.Head) ||
            (proposal.Status == ProposalStatusEnum.SecondApproval && request.Position == PositionEnum.Supervisor) ||
            (proposal.Status == ProposalStatusEnum.ThirdApproval && request.Position != PositionEnum.CoSupervisor))
            throw new ArgumentException("Cannot Send To This Position With Your Current Proposal State");

        ProposalStatusEnum targetedProposalStatus = proposal.Status;
        if (request.Position == PositionEnum.Supervisor || request.Position == PositionEnum.CoSupervisor)
            targetedProposalStatus = ProposalStatusEnum.FirstApproval;

        ValidateAddApprovalRequest(request, proposal, targetedProposalStatus);

        var approvalRequest = new ApprovalRequest
        {
            StaffId = request.StaffId,
            Position = request.Position,
            ProposalStatusSnapshot = targetedProposalStatus
        };

        proposal.ApprovalRequests.Add(approvalRequest);

        await db.SaveChangesAsync(ct);

        return approvalRequest.Id;
    }

    public async Task UpdateAsync(Guid id, bool isAccepted, CancellationToken ct)
    {
        var approvalRequest = await db.ApprovalRequests
            .Include(ar => ar.Proposal)
            .SingleOrDefaultAsync(ar => ar.Id == id && ar.StaffId == userInfo.UserId, ct) ??
            throw new ArgumentException("This teaching staff didnot recieve any approval request with this approval request ID");


        if ((approvalRequest.Position == PositionEnum.Supervisor ||approvalRequest.Position == PositionEnum.CoSupervisor)
            && approvalRequest.Proposal.Status == ProposalStatusEnum.NoApproval)
            throw new ArgumentException("Need the first approval");

        using var transaction = await db.Database.BeginTransactionAsync(ct);

        approvalRequest.IsAccepted = isAccepted;
        await db.SaveChangesAsync(ct);

        if (isAccepted && approvalRequest.Position == PositionEnum.Head)
        {
            approvalRequest.Proposal.Status =
                approvalRequest.Proposal.Status == ProposalStatusEnum.NoApproval
                ? ProposalStatusEnum.FirstApproval
                : ProposalStatusEnum.ThirdApproval;
        }
        else if (isAccepted &&
                await db.ApprovalRequests.AnyAsync(ar =>
                    ar.ProposalId == approvalRequest.ProposalId &&
                    ar.Position == PositionEnum.Supervisor &&
                    ar.IsAccepted == true, ct))
        {
            approvalRequest.Proposal.Status = ProposalStatusEnum.SecondApproval;
        }

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }

    public async Task CancelAsync(Guid id, CancellationToken ct)
    {
        var approvalRequest = await db.ApprovalRequests
            .Include(ar => ar.Proposal)
            .SingleOrDefaultAsync(ar => ar.Id == id, ct) ??
            throw new ArgumentException("Invalid Aprroval Request ID");

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleAsync(c => c.Id == approvalRequest.Proposal.CircleId, ct);

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, circle.Id, ct);
        validationService.CheckPermission(circleMember, circle, PermissionsEnum.ProposalManagement);

        if (approvalRequest.IsAccepted == true)
            throw new ArgumentException("Cannot cancel approved request");

        if (approvalRequest.IsAccepted == false)
            throw new ArgumentException("Cannot cancel denied request");

        db.ApprovalRequests.Remove(approvalRequest);
        await db.SaveChangesAsync(ct);
    }

    public async Task<List<GetCircleApprovalRequestResponse>> ListCircleAsync(Guid proposalId, ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        var propsal = await db.Proposals
            .Include(p => p.ApprovalRequests)
            .SingleOrDefaultAsync(p => p.Id == proposalId, ct) ??
            throw new ArgumentException("Invalid Proposal Id");

        var query = propsal.ApprovalRequests.AsQueryable();

        if (queryString.ProposalStatusEnum != null)
            query = query.Where(ar => ar.ProposalStatusSnapshot == queryString.ProposalStatusEnum);

        if (queryString.PositionEnum != null)
            query = query.Where(ar => ar.Position == queryString.PositionEnum);

        if (queryString.IsAccepted != null)
            query = query.Where(ar => ar.IsAccepted == queryString.IsAccepted);

        var response = await query
                .OrderBy(ar => ar.ProposalStatusSnapshot)
                .OrderBy(ar => ar.IsAccepted)
                .Select(ar => new GetCircleApprovalRequestResponse
                {
                    Id = ar.Id,
                    ProposalId = ar.ProposalId,
                    ProposalStatus = ar.ProposalStatusSnapshot,
                    IsAccepted = ar.IsAccepted,
                    TargetedStaffInfo = new StaffInfo
                    {
                        Id = ar.StaffId,
                        Position = ar.Position,
                    },
                }).ToListAsync(ct);

        foreach (var approval in response)
        {
            var staff = await db.Staff.SingleAsync(st => st.Id == approval.TargetedStaffInfo.Id, ct);
            approval.TargetedStaffInfo.Name = staff.FirstName + " " + staff.LastName;
        }

        return response;
    }

    public async Task<List<GetStaffApprovalRequestResponse>> ListStaffAsync(ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        var staff = await db.Staff.SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("You arenot From Teaching Staff");

        var query = db.ApprovalRequests.Where(ar => ar.StaffId == staff.Id);

        if (queryString.ProposalStatusEnum != null)
            query = query.Where(ar => ar.ProposalStatusSnapshot == queryString.ProposalStatusEnum);

        if (queryString.PositionEnum != null)
            query = query.Where(ar => ar.Position == queryString.PositionEnum);

        if (queryString.IsAccepted != null)
            query = query.Where(ar => ar.IsAccepted == queryString.IsAccepted);

        var response = await query
                .OrderBy(ar => ar.ProposalStatusSnapshot)
                .OrderBy(ar => ar.IsAccepted)
                .Select(ar => new GetStaffApprovalRequestResponse
                {
                    Id = ar.Id,
                    ProposalId = ar.ProposalId,
                    ProposalStatus = ar.ProposalStatusSnapshot,
                    IsAccepted = ar.IsAccepted,
                    TargetedStaffInfo = new StaffInfo
                    {
                        Id = ar.StaffId,
                        Position = ar.Position,
                    },

                }).ToListAsync(ct);

        foreach (var approval in response)
        {
            var targetedStaff = await db.Staff.SingleAsync(st => st.Id == approval.TargetedStaffInfo.Id, ct);
            approval.TargetedStaffInfo.Name = targetedStaff.FirstName + " " + targetedStaff.LastName;

            ApprovalRequest? previousApproval = null;

            if (approval.ProposalStatus == ProposalStatusEnum.FirstApproval)
                previousApproval = await db.ApprovalRequests
                    .Include(ar => ar.Staff)
                    .SingleAsync(ar => ar.ProposalId == approval.ProposalId &&
                                       ar.ProposalStatusSnapshot == ProposalStatusEnum.NoApproval, ct);
            else if (approval.ProposalStatus == ProposalStatusEnum.SecondApproval)
                previousApproval = await db.ApprovalRequests
                    .Include(ar => ar.Staff)
                    .SingleAsync(ar => ar.ProposalId == approval.ProposalId &&
                                       ar.Position == PositionEnum.Supervisor &&
                                       ar.ProposalStatusSnapshot == ProposalStatusEnum.FirstApproval, ct);

            if (previousApproval is not null)
            {
                approval.PreviousStaffInfo = new StaffInfo
                {
                    Id = previousApproval.StaffId,
                    Position = previousApproval.Position,
                    Name = previousApproval.Staff.FirstName + " " + previousApproval.Staff.LastName,
                };
            }
        }

        return response;
    }

    public async Task<GetApprovalRequestResponse> GetAsync(Guid id, CancellationToken ct)
    {
        var approvalRequest = await db.ApprovalRequests
            .Include(ar => ar.Staff)
            .Include(ar => ar.Proposal)
                .ThenInclude(p => p.Circle)
                    .ThenInclude(c => c.CircleMembers)
                        .ThenInclude(cm => cm.User)
            .SingleOrDefaultAsync(ar => ar.Id == id, ct) ??
            throw new ArgumentException("Invalid Approval Request ID");

        var circleFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);
        var studentFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var circle = approvalRequest.Proposal.Circle;
        var circleMemberOwner = circle.CircleMembers.Single(cm => cm.IsOwner);

        var proposal = approvalRequest.Proposal;

        var response = new GetApprovalRequestResponse
        {
            IsAccepted = approvalRequest.IsAccepted,
            ProposalId = approvalRequest.ProposalId,
            StaffId = approvalRequest.StaffId,
            Position = approvalRequest.Position,
            ProposalStatusSnapshot = approvalRequest.ProposalStatusSnapshot,
            CircleResponse = new GetCircleAsRowResponse
            {
                Id = circle.Id,
                Avatar = circleFileService.GetFileUrl(circle.Id.ToString(), FileTypes.Avatar),
                Name = circle.Name,
                OwnerName = circleMemberOwner.User.FirstName + " " + circleMemberOwner.User.LastName,
            },
            ProposalResponse = new GetProposalResponse
            {
                Id = proposal.Id,
                CircleId = proposal.CircleId,
                IsReseted = proposal.IsReseted,
                Objectives = proposal.Objectives,
                Overview = proposal.Overview,
                Status = proposal.Status,
                TechStack = proposal.TechStack
            },
            Members = circle.CircleMembers.Select(cm => new GetMemberAsRowResponse
            {
                UserId = cm.UserId,
                Name = cm.User.FirstName + " " + cm.User.LastName,
                Avatar = studentFileService.GetFileUrl(cm.UserId, FileTypes.Avatar),
                Badge = cm.Badge,
                IsOwner = cm.IsOwner,
            }).ToList(),
        };

        return response;
    }

    private async Task MatchStaffWithPosition(AddApprovalRequest request, CancellationToken ct)
    {
        List<string> rolesName =
            request.Position == PositionEnum.Head
            ? [AppRoles.HeadOfDept]
            : request.Position == PositionEnum.Supervisor
            ? [AppRoles.Professor]
            : [AppRoles.Professor, AppRoles.Assistant];

        List<string> rolesId = [];
        foreach (var roleName in rolesName)
        {
            var role = await db.Roles.SingleAsync(r => r.Name == roleName, ct);
            rolesId.Add(role.Id);
        }

        bool isMatched = false;
        foreach (var roleId in rolesId)
            isMatched |= await db.UserRoles.AnyAsync(ur => ur.UserId == request.StaffId && ur.RoleId == roleId, ct);

        if (!isMatched)
            throw new ArgumentException("Staff ID and Position donot match");
    }

    private static void ValidateAddApprovalRequest(AddApprovalRequest request, Proposal proposal, ProposalStatusEnum targetedProposalStatus)
    {
        if (proposal.ApprovalRequests
                        .Any(ar => ar.Position == request.Position
                          && ar.ProposalStatusSnapshot == targetedProposalStatus
                          && ar.IsAccepted == null))
            throw new ArgumentException("There is an already pending Approval request at this position");

        if (request.Position == PositionEnum.Head)
        {
            var firstAR = proposal.ApprovalRequests
                .SingleOrDefault(ar => ar.IsAccepted == true && ar.Position == PositionEnum.Head);
            if (firstAR != null && request.StaffId != firstAR.StaffId)
                throw new ArgumentException("Head of department ID doesnot match Head of department ID of the 1st approval request");

            var repeatedAR = proposal.ApprovalRequests
                .SingleOrDefault(ar => ar.IsAccepted != null && ar.ProposalStatusSnapshot == targetedProposalStatus);
            if (repeatedAR != null)
                throw new ArgumentException("Cannot send This Approval Request Twice");
        }
        else
        {
            var repeatedAR = proposal.ApprovalRequests
                .SingleOrDefault(ar => ar.IsAccepted != null && ar.StaffId == request.StaffId);
            if (repeatedAR != null)
                throw new ArgumentException("Cannot send This Approval Request Twice");
        }
    }
}
