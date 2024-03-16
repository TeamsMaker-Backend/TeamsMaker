using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Models;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;

namespace TeamsMaker.Api.Services.JoinRequests
{
    public class JoinRequestService(AppDBContext db) : IJoinRequestService
    {
        public async Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct)
        {
            var userId = await db.Users.FindAsync(request.UserId, ct);
            var circleId = await db.Circles.FindAsync(request.CircleId, ct);

            if (userId == null)
                throw new ArgumentException("Invalid User ID");

            if (circleId == null)
                throw new ArgumentException("Invalid Circle ID");

            if(request.EntityType.ToLower() == InvitationTypes.Circle.ToLower() || 
                request.EntityType.ToLower() == InvitationTypes.User.ToLower())
            {
                var joinRequest = new JoinRequest
                {
                    Sender = request.EntityType,
                    IsAccepted = false,
                    UserId = request.UserId,
                    CircleId = request.CircleId,

                };
                await db.JoinRequests.AddAsync(joinRequest, ct);
                await db.SaveChangesAsync(ct);
            }
            else
                throw new ArgumentException("Invalid Entity Type");

        }

        public async Task DeleteJoinRequestAsync(Guid id, CancellationToken ct)
        {
            var joinRequest = 
                await db.JoinRequests.FindAsync(id, ct) ??
                throw new ArgumentException("Not found");

            db.JoinRequests.Remove(joinRequest);

            await db.SaveChangesAsync(ct);
        }
    }
}
