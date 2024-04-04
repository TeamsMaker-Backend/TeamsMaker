using Core.ValueObjects;

using TeamsMaker.Api.Contracts.Responses.Profile;

namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class AddCircleRequest
{
    public required string Name { get; init; }
    public SummaryData? Summary { get; init; }
    public ICollection<string>? InvitedStudents { get; set; }
}
