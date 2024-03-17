using TeamsMaker.Api.Contracts.Requests.Session;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class SessionService(AppDBContext db) : ISessionService
{
    public async Task AddAsync(AddSessionRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
        .FindAsync([request.CircleId], ct) ??
        throw new ArgumentException("Invalid Circle ID");

        circle.Sessions.Add(new Session
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? SessionStatus.Upcoming,
            Date = request.Date,
            Time = request.Time,

            CircleId = request.CircleId,
        });

        await db.SaveChangesAsync(ct);
    }
}
