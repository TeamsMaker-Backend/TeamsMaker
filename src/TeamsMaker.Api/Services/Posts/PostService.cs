﻿using DataAccess.Base.Interfaces;

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
                LikesNumber = 0,
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
                LikesNumber = 0,
                ParentPostId = request.ParentPostId,
            };
        }

        await db.Posts.AddAsync(post, ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return post.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);

        var post = await db.Posts
            .Include(p => p.Comments)
            .Include(p => p.Reacts)
            .SingleOrDefaultAsync(p => p.Id == id, ct) ??
            throw new ArgumentException("Invalid Id");

        if (post.LikesNumber != 0)
            db.Reacts.RemoveRange(post.Reacts);

        if(post.Comments != null)
        { 
            var commentsId = post.Comments.Select(c => c.Id);

            for (int i = 0; i < commentsId.Count(); i++)
                await DeleteAsync(commentsId.ElementAt(i), ct);
        
        }
        db.Posts.Remove(post);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
    
    public async Task UpdateAsync(Guid id, UpdatePostRequest request, CancellationToken ct)
    {
        var post = await db.Posts.FindAsync([id], ct) ??
        throw new ArgumentException("Invalid Id");

        post.Content = request.Content;
        db.Update(post);

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
        db.Update(post);

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

        db.Reacts.Remove(react);

        post.LikesNumber -= 1;
        db.Update(post);

        await db.SaveChangesAsync(ct);
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