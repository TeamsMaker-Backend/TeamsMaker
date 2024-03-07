using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class UpdateProfileRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Bio { get; init; }
    public string? About { get; init; }
    public GenderEnum? Gender { get; init; }
    public string? City { get; init; }
    public string? Phone { get; init; }
    public ICollection<LinkInfo>? Links { get; init; }

    public IFormFile? Avatar { get; init; }
    public IFormFile? Header { get; init; }
    public IFormFile? CV { get; init; }
}