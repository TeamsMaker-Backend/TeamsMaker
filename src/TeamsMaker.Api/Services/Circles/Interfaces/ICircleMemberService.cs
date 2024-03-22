namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleMemberService
{
    Task AddAsync(Guid circleId, String userId, CancellationToken ct);
    Task RemoveAsync(Guid circleMemberId, CancellationToken ct);
    Task UpdateBadgeAsync(Guid circleMemberId, string? badge, CancellationToken ct);
}
