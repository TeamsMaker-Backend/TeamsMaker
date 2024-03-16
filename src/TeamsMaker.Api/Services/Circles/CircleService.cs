using System.Collections.Generic;

using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Services.Files.Interfaces;

namespace TeamsMaker.Api.Services.Circles;

public class CircleService(AppDBContext db, IServiceProvider serviceProvider, IUserInfo userInfo) : ICircleService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

    public async Task AddAsync(AddCircleRequest request, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct);
        var circle = new Circle
        {
            Name = request.Name,
            Description = request.Description,
            Summary = request.Summary,
        };
        await db.Circles.AddAsync(circle, ct);

        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = circle.Id, Name = s }).ToList() ?? [];
        circle.Links = request.Links?.Select(l => new Link { CircleId = circle.Id, Url = l.Url, Type = l.Type }).ToList() ?? [];

        var circleMember = new CircleMember
        {
            UserId = userInfo.UserId,
            CircleId = circle.Id,
            IsOwner = true,
            Permission = new Permission
            {
                CircleInfoPermissions = new CircleInfoPermissions
                {
                    UpdateFiles = true,
                    UpdateInfo = true
                }
            }
        };

        await db.CircleMembers.AddAsync(circleMember, ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }

    public async Task<GetCircleResponse> GetAsync(Guid id, CancellationToken ct)
    {
        var response = new GetCircleResponse();

        var circle = await db.Circles
            .Include(c => c.Skills)
            .Include(c => c.Summary)
            .Include(c => c.Links)
            .Include(c => c.CircleMembers)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        response.Name = circle.Name;
        response.Description = circle.Description;
        response.IsPublic = circle.Summary?.IsPublic ?? false;
        response.Avatar = _fileService.GetFileUrl(id.ToString(), FileTypes.Avatar);
        response.Header = _fileService.GetFileUrl(id.ToString(), FileTypes.Header);
        response.Rate = circle.Rate;
        response.Status = circle.Status;
        response.OrganizationId = circle.OrganizationId;
        response.Links = circle.Links.Select(l => new LinkInfo { Type = l.Type, Url = l.Url }).ToList();
        response.Skills = circle.Skills.Select(l => l.Name).ToList();

        if (circle.CircleMembers.Any(cm => cm.UserId == userInfo.UserId))
        {
            response.Summary = circle.Summary?.Summary;
        }

        return response;
    }

    public async Task<GetCircleMembersResponse> GetMembersAsync(Guid id, CancellationToken ct)
    {
        var response = new GetCircleMembersResponse();

        var circle = await db.Circles
            .Include(c => c.CircleMembers)
            .ThenInclude(cm => cm.Permission)
            .ThenInclude(p => p.CircleInfoPermissions)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        response.Members = circle.CircleMembers
            .Select(cm => new CircleMemberInfo
            {
                UserId = cm.UserId,
                IsOwner = cm.IsOwner,
                Badge = cm.Badge,
                Permissions = cm.Permission.CircleInfoPermissions
            }).ToList();

        return response;
    }

    public async Task UpdateInfoAsync(Guid id, UpdateCircleInfoRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Summary)
            .Include(c => c.Skills)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        circle.Name = request.Name;
        circle.Description = request.Description;

        var skills = db.Skills.Where(s => s.CircleId == id);
        db.Skills.RemoveRange(skills);

        circle.Skills = request.Skills?.Select(s => new Skill { CircleId = id, Name = s }).ToList() ?? [];

        if (request.Summary != null)
        {
            bool isPublic = false;
            if (circle.Summary != null)
                isPublic = circle.Summary.IsPublic;

            circle.Summary = new SummaryData { Summary = request.Summary, IsPublic = isPublic };
        }
        else
            circle.Summary = null;

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateLinksAsync(Guid id, UpdateCircleLinksRequest request, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Links)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var links = db.Links.Where(l => l.CircleId == id);
        db.Links.RemoveRange(links);

        circle.Links = request.Links?.Select(l => new Link { CircleId = id, Url = l.Url, Type = l.Type }).ToList() ?? [];

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdatePrivacyAsync(Guid id, bool isPublic, CancellationToken ct)
    {
        var circle = await db.Circles
            .Include(c => c.Summary)
            .SingleOrDefaultAsync(c => c.Id == id, ct) ??
            throw new ArgumentException("Invalid Circle ID");

        if (circle.Summary != null)
            circle.Summary.IsPublic = isPublic;

        await db.SaveChangesAsync(ct);
    }
}

// api/circle/accept
// api/joinRequest/accept/{id} = true
// 