using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;


namespace TeamsMaker.Api.Services.JoinRequests;

//TODO: get joins/invitations from student prespective
//TODO: get joins/invitations from circle prespective
public class JoinRequestService
    (AppDBContext db, IUserInfo userInfo, ICircleMemberService memberService, ICircleValidationService validationService, IServiceProvider serviceProvider) : IJoinRequestService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

    public async Task<Guid> AddAsync(AddJoinRequest request, CancellationToken ct)
    {
        var studentId = await db.Students.FindAsync([request.StudentId], ct) ??
            throw new ArgumentException("Invalid User Id");

        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleOrDefaultAsync(c => c.Id == request.CircleId, ct) ??
            throw new ArgumentException("Invalid CircleId");

        if (!request.SenderType.Equals(InvitationTypes.Circle, StringComparison.CurrentCultureIgnoreCase) &&
            !request.SenderType.Equals(InvitationTypes.Student, StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException("Invalid Sender Type");


        // In case of sender is a circle
        if (request.SenderType == InvitationTypes.Circle)
        {
            // Check wether the circle member have a permission to send an invitition or not
            var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, request.CircleId, ct);
            validationService.CheckPermission(circleMember, circle, PermissionsEnum.MemberManagement);
        }
        // check the student wether in a circle or not
        else if (await db.CircleMembers.AnyAsync(cm => cm.UserId == request.StudentId, ct))
            throw new ArgumentException("Cannot send a join request while you are a circle member");

        var joinRequest = new JoinRequest
        {
            Sender = request.SenderType,
            IsAccepted = false,
            StudentId = request.StudentId,
            CircleId = request.CircleId,
        };

        await db.JoinRequests.AddAsync(joinRequest, ct);
        await db.SaveChangesAsync(ct);

        return joinRequest.Id;
    }

    public async Task<GetJoinRequestResponse> GetAsync(string? circleId, CancellationToken ct)
    {
        string entityId = !string.IsNullOrEmpty(circleId) && await db.Circles.AnyAsync(c => c.Id == Guid.Parse(circleId), ct)
            ? circleId
            : userInfo.UserId;

        var fileService = string.IsNullOrEmpty(circleId) ?
            serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle) : serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var response = new GetJoinRequestResponse
        {
            JoinRequests = await db.JoinRequests
                .Where(jr => jr.Sender == InvitationTypes.Student)
                .OrderByDescending(jr => jr.CreationDate)
                .Select(jr => new GetBaseJoinRequestResponse
                {
                    Id = jr.Id,
                    Sender = jr.Sender,
                    Name = jr.Circle.Name,
                    Avatar = string.IsNullOrEmpty(circleId) ?
                        fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar) : fileService.GetFileUrl(jr.StudentId, FileTypes.Avatar)
                })
                .ToListAsync(cancellationToken: ct),

            Invitations = await db.JoinRequests
                .Where(jr => jr.Sender == InvitationTypes.Circle)
                .OrderByDescending(jr => jr.CreationDate)
                .Select(jr => new GetBaseJoinRequestResponse
                {
                    Id = jr.Id,
                    Sender = jr.Sender,
                    Name = jr.Circle.Name,
                    Avatar = string.IsNullOrEmpty(circleId) ?
                        fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar) : fileService.GetFileUrl(jr.StudentId, FileTypes.Avatar)
                })
                .ToListAsync(cancellationToken: ct),
        };

        return response;
    }

    public async Task AcceptAsync(Guid id, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var joinRequest = await db.JoinRequests.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Join Request Id");

        joinRequest.IsAccepted = true;

        // In case of reciever is a circle
        if (joinRequest.Sender != InvitationTypes.Circle)
        {
            // Check wether the circle member have a permission to accept a request or not
            var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, joinRequest.CircleId, ct);

            var circle = await db.Circles
                .Include(c => c.DefaultPermission)
                .SingleOrDefaultAsync(c => c.Id == joinRequest.CircleId, ct) ??
                throw new ArgumentException("Invalid CircleId");

            validationService.CheckPermission(circleMember, circle, PermissionsEnum.MemberManagement);
        }

        await db.SaveChangesAsync(ct);

        await memberService.AddAsync(joinRequest.CircleId, joinRequest.StudentId, ct);

        await transaction.CommitAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var joinRequest =
            await db.JoinRequests
            .SingleOrDefaultAsync(jr => jr.Id == id && jr.IsAccepted == false, ct) ??
            throw new ArgumentException("Not found");

        db.JoinRequests.Remove(joinRequest);

        // In case of sender is a circle
        if (joinRequest.Sender == InvitationTypes.Circle)
        {
            // Check wether the circle member have a permission to delete an invitition or not
            var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, joinRequest.CircleId, ct);

            var circle = await db.Circles
                .Include(c => c.DefaultPermission)
                .SingleOrDefaultAsync(c => c.Id == joinRequest.CircleId, ct) ??
                throw new ArgumentException("Invalid CircleId");

            validationService.CheckPermission(circleMember, circle, PermissionsEnum.MemberManagement);
        }

        await db.SaveChangesAsync(ct);
    }
}
