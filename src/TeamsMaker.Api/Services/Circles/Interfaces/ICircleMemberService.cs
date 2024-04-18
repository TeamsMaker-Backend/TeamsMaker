namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleMemberService
{
    Task AddAsync(Guid circleId, String userId, string reciever, CancellationToken ct);
    Task RemoveAsync(Guid circleMemberId, CancellationToken ct);
}
