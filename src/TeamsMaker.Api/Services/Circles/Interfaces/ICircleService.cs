﻿using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Contracts.Responses.Circle;

namespace TeamsMaker.Api.Services.Circles.Interfaces;

public interface ICircleService
{
    Task<GetCircleResponse> GetAsync(Guid id, CancellationToken ct);
    Task UpdateInfoAsync(Guid id, UpdateCircleInfoRequest request, CancellationToken ct);
    Task UpdateLinksAsync(Guid id, UpdateCircleLinksRequest request, CancellationToken ct);
    Task UpdatePrivacyAsync(Guid id, bool isPublic, CancellationToken ct);
}
