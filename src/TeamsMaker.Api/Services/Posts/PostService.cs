using Azure.Core;

using DataAccess.Base.Interfaces;

using Microsoft.IdentityModel.Tokens;

using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Services.Posts;

public class PostService(ICircleValidationService validationService, IUserInfo userInfo, AppDBContext db) : IPostService
{
    public async Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct)
    {
        Post post;
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        // CircleId
        if (request.CircleId != null)
        {
            var circle = await db.Circles
                .Include(c => c.DefaultPermission)
                .SingleOrDefaultAsync(c => c.Id == request.CircleId, ct) ??
                throw new ArgumentException("Invalid Id");
            //circleMember
            var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, (Guid)request.CircleId, ct);
            //FeedManagement
            validationService.CheckPermission(circleMember, circle, PermissionsEnum.FeedManagement);

            var author = await db.Authors.SingleOrDefaultAsync(a => a.CircleId == request.CircleId, ct);

            if (author == null)
            {
                var newAuthor = new Author { CircleId = request.CircleId };
                await db.Authors.AddAsync(newAuthor, ct);

                author = newAuthor;
            }
            post = new Post
            {
                AuthorId = author.Id,
                Content = request.Content,
                ParentPostId = request.ParentPostId,
            };
        }
        else
        {
            //userId
            var author = await db.Authors.SingleOrDefaultAsync(a => a.UserId == userInfo.UserId, ct);
            if (author == null)
            {
                var newAuthor = new Author { UserId = userInfo.UserId };
                await db.Authors.AddAsync(newAuthor, ct);
                author = newAuthor;
            }
            post = new Post
            {
                AuthorId = author.Id,
                Content = request.Content,
                ParentPostId = request.ParentPostId,
            };
        }

        await db.Posts.AddAsync(post, ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return post.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct, bool isAuthorized = false)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var post = await db.Posts
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .Include(p => p.Reacts)
            .SingleOrDefaultAsync(p => p.Id == id, ct) ??
            throw new ArgumentException("Invalid Id");

        var circleId = post.Author.CircleId;
        
        if (!isAuthorized && circleId != null)
            await CheckFeedPermission((Guid)circleId, ct);

        isAuthorized = true;

        if (post.LikesNumber > 0 && post.Reacts.Any())
            db.Reacts.RemoveRange(post.Reacts);

        if (!post.Comments.IsNullOrEmpty())
        {
            var commentsId = post.Comments.Select(c => c.Id);

            for (int i = 0; i < commentsId.Count(); i++)
                await DeleteAsync(commentsId.ElementAt(i), ct, isAuthorized);

        }
        db.Posts.Remove(post);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
    
    public async Task UpdateAsync(Guid id, UpdatePostRequest request, CancellationToken ct)
    {
        var post = await db.Posts
            .Include(p => p.Author)
            .SingleOrDefaultAsync(p => p.Id == id, ct) ??
        throw new ArgumentException("Invalid Id");

        var circleId = post.Author.CircleId;
        if(circleId != null)
            await CheckFeedPermission((Guid)circleId, ct);

        post.Content = request.Content;

        await db.SaveChangesAsync(ct);

    }

    public async Task<Guid> AddReactAsync(Guid postId, CancellationToken ct)
    {
        var post = await db.Posts.FindAsync([postId], ct) ??
        throw new ArgumentException("Invalid Id");

        var react = await db.Reacts
            .SingleOrDefaultAsync(r => r.UserId == userInfo.UserId && r.PostId == postId, ct);
        if (react != null)
        {
            throw new ArgumentException("You already reacted");
        }

        var newReact = new React
        {
            PostId = postId,
            UserId = userInfo.UserId,
        };
        await db.Reacts.AddAsync(newReact, ct);

        post.LikesNumber += 1;

        await db.SaveChangesAsync(ct);

        return newReact.Id;
    }

    public async Task DeleteReactAsync(Guid postId, CancellationToken ct)
    {
        var post = await db.Posts.FindAsync([postId], ct) ??
        throw new ArgumentException("Invalid Id");

        var react = await db.Reacts
            .SingleOrDefaultAsync(r => r.UserId == userInfo.UserId && r.PostId == postId, ct) ??
            throw new ArgumentException("You not reacted yet");

        post.LikesNumber -= 1;

        db.Reacts.Remove(react);

        await db.SaveChangesAsync(ct);
    }

    private async Task CheckFeedPermission(Guid circleId, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.DefaultPermission)
            .SingleAsync(c => c.Id == circleId, ct);

        var circleMember = await validationService.TryGetCircleMemberAsync(userInfo.UserId, (Guid)circleId, ct);
        //FeedManagement
        validationService.CheckPermission(circleMember, circle, PermissionsEnum.FeedManagement);
    }
}


/*
Post Service

Post: add post -> Posts table && add author -> Authors
Update: patch
Delete: parent with childs Post&Comments  Comment&Replies
Get: circle, user, get feed   pagination order by desc (CreationDate & CreatedBy)

Get circle posts (circle id)
Get user posts (user id)
Get feed (split 2 tabs get all with pagination order by desc (CreationDate & CreatedBy))
Get Post (post id)
-----
Include Author data

*/