namespace TeamsMaker.Api.Contracts.Requests.Profile;

public class UpdateProfileRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Bio { get; init; }
    public string? About { get; init; }
    public int? Gender { get; init; }
    public string? City { get; init; }
    public string? Phone { get; init; }
    public ICollection<string>? Links { get; init; }

    public IFormFile? Avatar { get; set; }
    public IFormFile? Header { get; set; }
    public IFormFile? CV { get; init; }
}