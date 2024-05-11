using Azure.Core;

using DataAccess.Base;
using DataAccess.Base.Interfaces;

using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Reacts.Interfaces;

namespace TeamsMaker.Api.Services.Reacts;

public class ReactService(IUserInfo userInfo, AppDBContext db) : IReactService
{
    public async Task<Guid> AddAsync(Guid id, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var post = await db.Posts.FindAsync([id], ct) ??
        throw new ArgumentException("Invalid Id");

        var react = await db.Reacts.SingleOrDefaultAsync(r => r.UserId == userInfo.UserId && r.PostId == id, ct);
        if (react != null)
        {
            throw new ArgumentException("You already reacted");
        }

        var newReact = new React
        { 
            PostId = id,
            UserId = userInfo.UserId,
        };
        await db.AddAsync(newReact, ct);

        post.LikesNumber += 1;
        db.Update(post);
       
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return newReact.Id; 
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var post = await db.Posts.FindAsync([id], ct) ??
        throw new ArgumentException("Invalid Id");

        var react = await db.Reacts.SingleOrDefaultAsync(r => r.UserId == userInfo.UserId && r.PostId == id, ct) ??
        throw new ArgumentException("You not reacted yet");

        db.Remove(react);

        post.LikesNumber -= 1;
        db.Update(post);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
}
