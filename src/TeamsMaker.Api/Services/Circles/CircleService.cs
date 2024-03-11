using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.NewFolder;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Circles.Interfaces;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Services.Storage.Interfacecs;
using Core.ValueObjects;

namespace TeamsMaker.Api.Services.Circles;

public class CircleService
    (AppDBContext db, IUserInfo userInfo, IStorageService storageService, IWebHostEnvironment host)
    : ICircleService
{
    private readonly AppDBContext _db = db;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly IWebHostEnvironment _host = host;
    private readonly IStorageService _storageService = storageService;
    
    public async Task AddCircleAsync(AddCircleRequest request, CancellationToken ct)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        var circle = new Circle
        {
            Name = request.Name,
            Description = request.Description,
            Summary = request.Summary,
            Skills = request.Skills?.Select(s => new Skill { Name = s }).ToList() ?? [],
            Links = request.Links?.Select(l => new Link { UserId = _userInfo.UserId, Url = l.Url, Type = l.Type }).ToList() ?? [],
            
            Avatar = await _storageService.
            UpdateFileAsync(null, request.Avatar, CreateName(FileTypes.Avatar, request.Avatar?.FileName),
            Path.Combine(Path.Combine(_host.WebRootPath, BaseTypes.Circle), _userInfo.UserId), ct),

            Header = await _storageService.
            UpdateFileAsync(null, request.Header, CreateName(FileTypes.Header, request.Header?.FileName),
            Path.Combine(Path.Combine(_host.WebRootPath, BaseTypes.Circle), _userInfo.UserId), ct)
        };
        await _db.Circles.AddAsync(circle, ct);

        var circlemember = new CircleMember
        {
            UserId = _userInfo.UserId,
            CircleId = circle.Id,
            IsOwner = true
        };
        await _db.CircleMembers.AddAsync(circlemember, ct);

        var circleifopermissions = new CircleInfoPermissions
        {
            UpdateFiles = true,
            UpdateInfo = true
        };
        var permission = new Permission
        {
            CircleMemberId = circlemember.Id,
            CircleInfoPermissions = circleifopermissions
        };
        await _db.Permissions.AddAsync(permission, ct);

        await _db.SaveChangesAsync(ct);
        await transaction.CommitAsync();
    }

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}
