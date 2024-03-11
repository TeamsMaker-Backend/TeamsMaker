using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class CircleService(AppDBContext db) : ICircleService
{
    public async Task UpdateLinksAsync(Guid id, UpdateCircleLinksRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Links)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var links = db.Links.Where(l => l.CircleId == id);
        db.Links.RemoveRange(links);

        circle.Links =
            request.Links?
            .Select(l => new Link { CircleId = id, Url = l.Url, Type = l.Type })
            .ToList() ?? [];
    }
}
