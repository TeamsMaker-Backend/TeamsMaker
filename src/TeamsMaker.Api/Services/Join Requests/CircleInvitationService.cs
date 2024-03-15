using TeamsMaker.Api.Contracts.Requests.Join_Request;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Join_Requests.Interfaces;

namespace TeamsMaker.Api.Services.Join_Requests
{
    public class CircleInvitationService(AppDBContext db) : IJoinRequestService
    {
        private const string sender = InvitationTypes.Circle;
        public async Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct)
        {
            var userId = db.Users.FindAsync(request.UserId).Result;
            var circleId = db.Circles.FindAsync(request.CircleId).Result;

            if (userId == null)
                throw new ArgumentException("Invalid User ID");

            if (circleId == null)
                throw new ArgumentException("Invalid Circle ID");

            var joinRequest = new JoinRequest
            {
                Sender = sender,
                IsAccepted = false,
                UserId = request.UserId,
                CircleId = request.CircleId,

            };
            await db.JoinRequests.AddAsync(joinRequest, ct);
            await db.SaveChangesAsync(ct);
        }
    }
}
