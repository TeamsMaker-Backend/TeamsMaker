using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Services.Profiles.Interfaces;

public interface IUserService
{
    Task<List<GetUserAsRowResponse>> FilterAsync(UsersSearchQueryString query, CancellationToken ct);
}