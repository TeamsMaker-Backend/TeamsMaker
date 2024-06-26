﻿using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;

namespace TeamsMaker.Api.Services.Organizations;

public interface IOrganizationService
{
    Task<PagedList<GetOrganizationResponse>> GetAsync(OrganizationsQueryString queryString, CancellationToken ct);
    Task AddAsync(OrganizationRequest organizationRequest, CancellationToken ct);
    Task UpdateAsync(int organizationId, OrganizationRequest organizationRequest, CancellationToken ct);
    Task DeleteAsync(int organizationId, CancellationToken ct);
}
