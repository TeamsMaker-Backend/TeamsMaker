using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.QueryStringParameters;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.Profiles;

public class UserService(AppDBContext db, IUserInfo userInfo, IServiceProvider serviceProvider) : IUserService
{
    public async Task<List<GetUserAsRowResponse>> FilterAsync(UsersSearchQueryString query, CancellationToken ct)
    {
        IFileService fileService;

        var usersQuery = db.Users
            .Where(u => u.Id != userInfo.UserId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query.Q))
            usersQuery = usersQuery.Where(std => std.FirstName.Contains(query.Q)
                        || std.LastName.Contains(query.Q)
                        || (std.Email != null && std.Email.Contains(query.Q)));


        if(query.CircleId.HasValue) 
            usersQuery = usersQuery
                .Include(u => u.MemberOn)
                .Where(u => u.MemberOn.Any(m => m.CircleId == query.CircleId.Value));


        var users = usersQuery
            .Select(u => new GetUserAsRowResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Bio = u.Bio,
                UserType = u is Student ? UserEnum.Student : UserEnum.Staff
            });

        if (query.UserType.HasValue) users = users.Where(u => u.UserType!.Value == query.UserType.Value);

        var usersList = await users.ToListAsync(ct);


        foreach (var user in usersList)
        {
            fileService = user.UserType == UserEnum.Student
                ? serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student)
                : serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Staff);

            user.Avatar = fileService.GetFileUrl(user.Id, FileTypes.Avatar);
        }

        return usersList;
    }
}