using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Models;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Services.JoinRequests;

public class JoinRequestService(AppDBContext db) : IJoinRequestService
{
    public async Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct)
    {
        var studentId = await db.Students.FindAsync([request.StudentId], ct);
        var circleId = await db.Circles.FindAsync([request.CircleId], ct);

        if (studentId == null)
            throw new ArgumentException("Invalid User Id");

        if (circleId == null)
            throw new ArgumentException("Invalid Circle Id");

        if (!request.EntityType.Equals(InvitationTypes.Circle, StringComparison.CurrentCultureIgnoreCase) ||
            !request.EntityType.Equals(InvitationTypes.Student, StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("Invalid Entity Type");

        var joinRequest = new JoinRequest
        {
            Sender = request.EntityType,
            IsAccepted = false,
            StudentId = request.StudentId,
            CircleId = request.CircleId,
        };

        await db.JoinRequests.AddAsync(joinRequest, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteJoinRequestAsync(Guid id, CancellationToken ct)
    {
        var joinRequest =
            await db.JoinRequests.FindAsync([id], ct) ??
            throw new ArgumentException("Not found");

        db.JoinRequests.Remove(joinRequest);

        await db.SaveChangesAsync(ct);
    }
}

