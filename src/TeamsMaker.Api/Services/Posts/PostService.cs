using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

using Core.Generics;

using DataAccess.Base.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses.Post;
using TeamsMaker.Api.Contracts.Responses.TodoTask;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Models;
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

     public async Task<PagedList<GetPostResponse>> ListAuthorPostsAsync(string id, PostsQueryString queryString, CancellationToken ct)
    {
        var author = await GetAuthorAsync(id, ct);

        var posts = db.Posts
            .Where(p => author != null && p.AuthorId == author.Id && p.ParentPostId == null)
            .OrderByDescending(p => p.CreationDate)
            .Select(p => new GetPostResponse
            {
                Id = p.Id,
                Content = p.Content,
                LikesNumber = p.LikesNumber,
                AuthorId = p.AuthorId,
                CommentsNumber = p.Comments.Count,
                CreationDate = p.CreationDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy,
                LastModificationDate = p.LastModificationDate,
                Comments = p.Comments.Select(cm => new GetPostResponse
                {
                    Id = cm.Id,
                    Content = cm.Content,
                    LikesNumber = cm.LikesNumber,
                    AuthorId = cm.AuthorId,
                    CommentsNumber = cm.Comments.Count,
                    CreationDate = cm.CreationDate,
                    CreatedBy = cm.CreatedBy,
                    ModifiedBy = cm.ModifiedBy,
                    LastModificationDate = cm.LastModificationDate,
                }).ToList()
            });

        return await PagedList<GetPostResponse>
                       .ToPagedListAsync(posts.AsQueryable(), queryString.PageNumber, queryString.PageSize, ct);
    }

    public async Task<PagedList<GetPostResponse>> ListFeedsPostsAsync(string id, PostsQueryString queryString, CancellationToken ct)
    {
        var author = await GetAuthorAsync(id, ct);

        var authorsListId = new List<Guid>();

        if (author?.UserId != null)
        {
            authorsListId = await db.Authors
                .Where(a => a.CircleId != null)
                .Select(a => a.Id).ToListAsync();
        }
        else
        {
            authorsListId = await db.Authors
                .Where(a => a.UserId != null)
                .Select(a => a.Id).ToListAsync();
        }

        var posts = db.Posts
            .Where(p => authorsListId.Contains(p.AuthorId) == true && p.ParentPostId == null)
            .OrderByDescending(p => p.CreationDate)
            .Select(p => new GetPostResponse
            {
                Id = p.Id,
                Content = p.Content,
                LikesNumber = p.LikesNumber,
                AuthorId = p.AuthorId,
                CommentsNumber = p.Comments.Count,
                CreationDate = p.CreationDate,
                CreatedBy = p.CreatedBy,
                ModifiedBy = p.ModifiedBy,
                LastModificationDate = p.LastModificationDate,
                Comments = p.Comments.Select(cm => new GetPostResponse
                {
                    Id = cm.Id,
                    Content = cm.Content,
                    LikesNumber = cm.LikesNumber,
                    AuthorId = cm.AuthorId,
                    CommentsNumber = cm.Comments.Count,
                    CreationDate = cm.CreationDate,
                    CreatedBy = cm.CreatedBy,
                    ModifiedBy = cm.ModifiedBy,
                    LastModificationDate = cm.LastModificationDate,
                }).ToList()
            });

        return await PagedList<GetPostResponse>
                      .ToPagedListAsync(posts.AsQueryable(), queryString.PageNumber, queryString.PageSize, ct);
    }

    public async Task<GetPostResponse> GetPostAsync(Guid postId, CancellationToken ct)
    {
        var post =
            await db.Posts
            .Include(ps => ps.Comments)
            .SingleOrDefaultAsync(ps => postId == ps.Id, ct) ??
             throw new ArgumentException("Invalid Id");


        var response = new GetPostResponse
        {
            Id = postId,
            Content = post.Content,
            LikesNumber = post.LikesNumber,
            AuthorId = post.AuthorId,
            CommentsNumber = post.Comments.Count,
            CreationDate = post.CreationDate,
            CreatedBy = post.CreatedBy,
            ModifiedBy = post.ModifiedBy,
            LastModificationDate = post.LastModificationDate,
            Comments = post.Comments.Select(cm => new GetPostResponse
            {
                Id = cm.Id,
                Content = cm.Content,
                LikesNumber = cm.LikesNumber,
                AuthorId = cm.AuthorId,
                CommentsNumber = cm.Comments.Count,
                CreationDate = cm.CreationDate,
                CreatedBy = cm.CreatedBy,
                ModifiedBy = cm.ModifiedBy,
                LastModificationDate = cm.LastModificationDate,
            }).ToList()
        };

        return response;
    }


    private async Task<Author?> GetAuthorAsync(string id, CancellationToken ct)
    {
        var author =
            await db.Authors
            .Include(a => a.Posts)
            .SingleOrDefaultAsync(a => (a.UserId == id || a.CircleId == Guid.Parse(id)), ct);
        return author;
    }
}


/*
PostInfo Service

PostInfo: add post -> Posts table && add author -> Authors
Update: patch
Delete: parent with childs PostInfo&Comments  Comment&Replies
Get: circle, user, get feed   pagination order by desc (CreationDate & CreatedBy)

Get circle posts (circle postId)
Get user posts (user postId)
Get feed (split 2 tabs get all with pagination order by desc (CreationDate & CreatedBy))
Get PostInfo (post postId)
-----
Include Author data

*/