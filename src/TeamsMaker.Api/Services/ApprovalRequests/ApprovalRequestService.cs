using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.ApprovalRequest;
using TeamsMaker.Api.Contracts.Responses.ApprovalRequest;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.ApprovalRequests.Interfaces;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.ApprovalRequests;

public class ApprovalRequestService
    (AppDBContext db, IUserInfo userInfo, ICircleValidationService validationService) : IApprovalRequestService
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

        // Assure that the suitable position for a suitable proposal state
        if ((request.CurrentProposalStatus == ProposalStatusEnum.NoApproval && request.Position != PositionEnum.Head) ||
            (request.CurrentProposalStatus == ProposalStatusEnum.FirstApproval && request.Position == PositionEnum.Head) ||
            (request.CurrentProposalStatus == ProposalStatusEnum.SecondApproval && request.Position == PositionEnum.Supervisor))
            throw new ArgumentException("Position and Proposal status donot match");


        // Assure that the staff Id is as same as the required position
        StaffClassificationsEnum staffClassification;

        if (request.Position == PositionEnum.Head)
            staffClassification = StaffClassificationsEnum.HeadOfDept;
        else if (request.Position == PositionEnum.Supervisor)
            staffClassification = StaffClassificationsEnum.Professor;
        else
            staffClassification = StaffClassificationsEnum.Professor | StaffClassificationsEnum.Assistant;

        if (await db.Staff.AnyAsync(s => s.Id == request.StaffId && (s.Classification & staffClassification) != 0, ct) == false)
            throw new ArgumentException("Staff ID and Position donot match");

        // Other Validations
        if (proposal.Status < ProposalStatusEnum.SecondApproval && proposal.ApprovalRequests
                .Any(ar => ar.Position == request.Position && ar.IsAccepted == null))
            throw new ArgumentException("Proposal should has only one pending request at each position");

        if (proposal.Status == ProposalStatusEnum.SecondApproval)
        {
            if (request.Position != PositionEnum.Head)
                throw new ArgumentException("Expected Head of Department");

            // Assure that if there is already pending 3rd approval
            if (proposal.ApprovalRequests
                .Any(ar => ar.Position == PositionEnum.Head && ar.IsAccepted == null))
                throw new ArgumentException("There is an already pending 3rd Approval request");

            // Assure that the hea₫ of the first approval is the same as the head of the third approval
            var firstApprovalRequest = proposal.ApprovalRequests
                .Single(ar => ar.IsAccepted == true && ar.Position == PositionEnum.Head);

            if (firstApprovalRequest.StaffId != request.StaffId)
                throw new ArgumentException("Head of department ID doesnot match Head of department ID of the 1st approval request");
        }
        else if (proposal.Status == ProposalStatusEnum.ThirdApproval)
            throw new ArgumentException("Proposal is already approved");

        var approvalRequest = new ApprovalRequest
        {
            StaffId = request.StaffId,
            Position = request.Position,
            ProposalStatusSnapShot = request.CurrentProposalStatus
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


        if (approvalRequest.Position == PositionEnum.Supervisor ||
            approvalRequest.Position == PositionEnum.CoSupervisor)
        {
            if (approvalRequest.Proposal.Status == ProposalStatusEnum.NoApproval)
                throw new ArgumentException("Need the first approval");
        }

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

    public async Task<ListCircleApprovalRequestResponse> ListCircleAsync(Guid proposalId, ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        var propsal = await db.Proposals
            .Include(p => p.ApprovalRequests)
            .SingleOrDefaultAsync(p => p.Id == proposalId, ct) ??
            throw new ArgumentException("Invalid Proposal Id");

        var response = new ListCircleApprovalRequestResponse
        {
            ApprovalRequests = propsal.ApprovalRequests
                .Where(ar =>
                    (queryString.ProposalStatusEnum == null || ar.ProposalStatusSnapShot == queryString.ProposalStatusEnum))
                .Where(ar =>
                    (queryString.PositionEnum == null || ar.Position == queryString.PositionEnum))
                .Where(ar =>
                    (queryString.IsAccepted == null || ar.IsAccepted == queryString.IsAccepted))
                .OrderBy(ar => ar.ProposalStatusSnapShot)
                .OrderBy(ar => ar.IsAccepted)
                .Select(ar => new CircleApprovalRequestInfo
                {
                    Id = ar.Id,
                    ProposalId = ar.ProposalId,
                    ProposalStatus = ar.ProposalStatusSnapShot,
                    IsAccepted = ar.IsAccepted,
                    TargetedStaffInfo = new StaffInfo
                    {
                        Id = ar.StaffId,
                        Position = ar.Position,
                    },
                }).ToList()
        };

        foreach (var approval in response.ApprovalRequests)
        {
            var staff = await db.Staff.SingleAsync(st => st.Id == approval.TargetedStaffInfo.Id, ct);
            approval.TargetedStaffInfo.Name = staff.FirstName + " " + staff.LastName;
        }

        return response;
    }

    public async Task<ListStaffApprovalRequestResponse> ListStaffAsync(ApprovalRequestQueryString queryString, CancellationToken ct)
    {
        var staff = await db.Staff.SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("You arenot From Teaching Staff");

        var response = new ListStaffApprovalRequestResponse
        {
            ApprovalRequests = await db.ApprovalRequests
                .Where(ar => ar.StaffId == staff.Id)
                .Where(ar =>
                    (queryString.ProposalStatusEnum == null || ar.ProposalStatusSnapShot == queryString.ProposalStatusEnum))
                .Where(ar =>
                    (queryString.PositionEnum == null || ar.Position == queryString.PositionEnum))
                .Where(ar =>
                    (queryString.IsAccepted == null || ar.IsAccepted == queryString.IsAccepted))
                .OrderBy(ar => ar.ProposalStatusSnapShot)
                .OrderBy(ar => ar.IsAccepted)
                .Select(ar => new StaffApprovalRequestInfo
                {
                    Id = ar.Id,
                    ProposalId = ar.ProposalId,
                    ProposalStatus = ar.ProposalStatusSnapShot,
                    IsAccepted = ar.IsAccepted,
                    TargetedStaffInfo = new StaffInfo
                    {
                        Id = ar.StaffId,
                        Position = ar.Position,
                    },

                }).ToListAsync(ct)
        };

        foreach (var approval in response.ApprovalRequests)
        {
            var targetedStaff = await db.Staff.SingleAsync(st => st.Id == approval.TargetedStaffInfo.Id, ct);
            approval.TargetedStaffInfo.Name = targetedStaff.FirstName + " " + targetedStaff.LastName;

            ApprovalRequest? previousApproval =
                approval.ProposalStatus != ProposalStatusEnum.NoApproval
                ? await db.ApprovalRequests
                    .Include(ar => ar.Staff)
                    .SingleAsync(ar => ar.ProposalId == approval.ProposalId &&
                        ar.ProposalStatusSnapShot == approval.ProposalStatus - 1, ct)
                : null;

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
}
