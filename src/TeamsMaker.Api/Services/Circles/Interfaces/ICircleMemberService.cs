using TeamsMaker.Api.Contracts.Requests.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleMemberService
{
    Task AddAsync(Guid circleId, string userId, string reciever, CancellationToken ct);
    Task UpdateAsync(Guid memberId, UpdateCircleMemberRequest request, CancellationToken ct);
    Task RemoveAsync(Guid circleMemberId, CancellationToken ct);
}
