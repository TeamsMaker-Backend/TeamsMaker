using TeamsMaker.Api.Core.Enums;

namespace TeamsMaker.Api.Contracts.Requests.ApprovalRequest;

public class AddApprovalRequest
{
    public Guid CircleId { get; init; }
    public string StaffId { get; init; } = null!;
    public PositionEnum Position { get; init; }
}
