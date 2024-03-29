﻿using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Responses.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task<Guid> AddAsync(AddCircleRequest request, CancellationToken ct);
    Task<GetCircleResponse> GetAsync(Guid circleId, CancellationToken ct);
    Task<GetCircleMembersResponse> GetMembersAsync(Guid circleId, CancellationToken ct);
    Task UpdateInfoAsync(Guid circleId, UpdateCircleInfoRequest request, CancellationToken ct);
    Task UpdatePrivacyAsync(Guid circleId, bool isPublic, CancellationToken ct);
    Task TransferOwnershipAsync(Guid circleId, string newOwnerId, CancellationToken ct);
    Task ArchiveAsync(Guid circleId, CancellationToken ct);
    Task DeleteAsync(Guid circleId, CancellationToken ct);
}
