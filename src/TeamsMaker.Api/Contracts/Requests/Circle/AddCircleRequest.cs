using Core.ValueObjects;

namespace TeamsMaker.Api.Contracts.Requests.Circle;

public class AddCircleRequest
{
    public required string Name { get; init; }
    public SummaryData? Summary { get; init; }
    public ICollection<string>? InvitedStudents { get; set; }
}
