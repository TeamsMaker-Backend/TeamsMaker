using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.Services.Circles;

public class CircleMembersService(AppDBContext db) // Not Finished Yet (and not Registered)
{
    public async Task AddAsync(JoinRequest joinRequest, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == joinRequest.CircleId, ct) ??
            throw new ArgumentException("Invalid Circle Id");

        if (await db.CircleMembers.AnyAsync(cm => cm.UserId == joinRequest.StudentId, ct))
            throw new ArgumentException("Student Cannot Be In Two Circles");

        circle.CircleMembers.Add(new CircleMember
        {
            UserId = joinRequest.StudentId,
            IsOwner = false,
        });

        await db.SaveChangesAsync(ct);
    }

    public Task RemoveAsync(Guid memberIdToBeRemoved, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
