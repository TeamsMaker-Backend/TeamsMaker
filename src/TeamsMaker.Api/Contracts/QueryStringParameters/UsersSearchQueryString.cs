using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class UsersSearchQueryString
{
    public string? Q { get; init; }
    public UserEnum? UserType { get; set; }
    public Guid? CircleId { get; set; }
}