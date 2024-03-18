using TeamsMaker.Api.Contracts.Requests.Session;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class SessionService(AppDBContext db) : ISessionService
{
    public async Task<Guid> AddAsync(AddSessionRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
        .FindAsync([request.CircleId], ct) ??
        throw new ArgumentException("Invalid Circle ID");

        var session = new Session
        {
            Title = request.Title,
            Notes = request.Notes,
            Status = request.Status ?? SessionStatus.Upcoming,
            Date = request.Date,
            Time = request.Time,

            CircleId = request.CircleId,
        };

        circle.Sessions.Add(session);

        await db.SaveChangesAsync(ct);

        return session.Id;
    }

    public async Task UpdateInfoAsync(Guid id, UpdateSessionInfoRequest request, CancellationToken ct)
    {
        var session = await db.Sessions.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Session Id");

        session.Title = request.Title;
        session.Notes = request.Notes;
        session.Date = request.Date;
        session.Time = request.Time;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid id, SessionStatus status, CancellationToken ct)
    {
        var session = await db.Sessions.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Session Id");

        session.Status = status;

        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var session = await db.Sessions.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Session Id");

        db.Sessions.Remove(session);

        await db.SaveChangesAsync(ct);
    }
}
