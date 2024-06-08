using Core.Generics;

using DataAccess.Base.Interfaces;

using Microsoft.IdentityModel.Tokens;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Post;
using TeamsMaker.Api.Contracts.Responses.Post;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Services.Posts;

public class PostService(ICircleValidationService validationService, IUserInfo userInfo, 
    AppDBContext db, IServiceProvider serviceProvider) : IPostService
{
    public async Task<Guid> AddAsync(AddPostRequest request, CancellationToken ct)
    {
        if (request.CircleId != null)
            await CheckFeedPermission((Guid)request.CircleId, ct);

        var author = await GetAuthorAsync(request.CircleId?.ToString() ?? userInfo.UserId, ct);

        if (author == null)
        {
            author = new Author { CircleId = request.CircleId };

            if (request.CircleId == null)
                author.UserId = userInfo.UserId;

            await db.Authors.AddAsync(author, ct);
        }

        var post = new Post
        {
            Content = request.Content,
            ParentPostId = request.ParentPostId,
        };

        author.Posts.Add(post);

        await db.SaveChangesAsync(ct);

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

        if (post.LikesNumber > 0 && post.Reacts.Count != 0)
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
        if (circleId != null)
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
            throw new ArgumentException("You already reacted");

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

    public async Task<PagedList<GetPostResponse>> ListAuthorPostsAsync(string id, PostsQueryString queryString, CancellationToken ct)
    {
        var author = await GetAuthorAsync(id, ct) ??
            throw new ArgumentException("Invalid Entity ID");

        var postsId = author?.Posts
            .Where(p => author != null && p.AuthorId == author.Id && p.ParentPostId == null)
            .OrderByDescending(p => p.CreationDate)
            .Select(p => p.Id);

        var posts = new List<GetPostResponse>();

        foreach (var postId in postsId ?? [])
            posts.Add(await GetPostAsync(postId, ct));

        return PagedList<GetPostResponse>
            .ToPagedList(posts.AsQueryable(), queryString.PageNumber, queryString.PageSize);
    }

    public async Task<PagedList<GetPostResponse>> ListFeedPostsAsync(bool isCircle, PostsQueryString queryString, CancellationToken ct)
    {
        var authorsListId = await db.Authors
                .Where(a => isCircle ? a.UserId != null : a.CircleId != null)
                .Select(a => a.Id)
                .ToListAsync(ct);

        var postsId = await db.Posts
            .Where(p => authorsListId.Contains(p.AuthorId) == true && p.ParentPostId == null)
            .OrderByDescending(p => p.CreationDate)
            .Select(p => p.Id)
            .ToListAsync(ct);

        var posts = new List<GetPostResponse>();

        foreach (var postId in postsId ?? [])
            posts.Add(await GetPostAsync(postId, ct));

        return PagedList<GetPostResponse>
            .ToPagedList(posts.AsQueryable(), queryString.PageNumber, queryString.PageSize);
    }

    public async Task<GetPostResponse> GetPostAsync(Guid postId, CancellationToken ct)
    {
        var post =
            await db.Posts
            .Include(ps => ps.Author)
                .ThenInclude(a => a.User)
            .Include(ps => ps.Reacts)
            .Include(ps => ps.Comments)
                .ThenInclude(cm => cm.Author)
                    .ThenInclude(a => a.User)
            .Include(ps => ps.Comments)
                .ThenInclude(cm => cm.Reacts)
            .SingleOrDefaultAsync(ps => postId == ps.Id, ct) ??
             throw new ArgumentException("Invalid Id");


        var response = new GetPostResponse
        {
            Id = postId,
            Content = post.Content,
            LikesNumber = post.LikesNumber,
            IsLiked = post.Reacts.Any(r => r.UserId == userInfo.UserId && r.PostId == postId),
            AuthorId = post.AuthorId,
            AuthorAvatar = GetAuthorAvatar(post.Author),  
            CommentsNumber = post.Comments.Count,
            CreationDate = post.CreationDate.ToString(),
            Comments = post.Comments.Select(cm => new GetPostResponse
            {
                Id = cm.Id,
                Content = cm.Content,
                LikesNumber = cm.LikesNumber,
                IsLiked = cm.Reacts.Any(r => r.UserId == userInfo.UserId && r.PostId == cm.Id),
                AuthorId = cm.AuthorId,
                AuthorAvatar = GetAuthorAvatar(cm.Author),
                CommentsNumber = cm.Comments.Count,
                CreationDate = cm.CreationDate.ToString()
            })
            .ToList()
        };
    
        return response;
    }

    private string? GetAuthorAvatar(Author author)
    {
        IFileService fileService;

        fileService = author.CircleId != null
            ? serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle)
            : author.User is Student
                ? serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student)
                : serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Staff);


        string id = author.CircleId.ToString() ?? author.UserId!;

        return fileService.GetFileUrl(id, FileTypes.Avatar);
    }

    private async Task<Author?> GetAuthorAsync(string id, CancellationToken ct)
    {
        var author =
            await db.Authors
            .Include(a => a.Posts)
            .SingleOrDefaultAsync(a => (a.UserId == id || a.CircleId == Guid.Parse(id)), ct);
        return author;
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
