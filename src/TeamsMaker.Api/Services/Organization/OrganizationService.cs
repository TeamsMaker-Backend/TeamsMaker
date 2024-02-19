using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Core.Guards;
using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.Services.Organizations;

public class OrganizationService : IOrganizationService
{
    private readonly AppDBContext _db;

    public OrganizationService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<PagedList<GetOrganizationResponse>> GetAsync(OrganizationsQueryString queryString, CancellationToken ct)
    {
        var query = _db.Organizations
            .Select(org => new GetOrganizationResponse
            {
                Id = org.Id,
                EngName = org.Name.Eng,
                LocName = org.Name.Loc,
                Address = org.Address,
                Phone = org.Phone,
                Description = org.Description,
                Logo = org.Logo
            });


        if (queryString.OrganizationId.HasValue) query = query.Where(o => o.Id == queryString.OrganizationId);

        return await PagedList<GetOrganizationResponse>.ToPagedListAsync(query, queryString.PageNumber, queryString.PageSize);
    }

    public async Task AddAsync(AddOrganizationRequest organizationRequest, CancellationToken ct)
    {
        Organization organization = new()
        {
            Name = new(organizationRequest.EngName, organizationRequest.LocName),
            Address = organizationRequest.Address,
            Phone = organizationRequest.Phone,
            Description = organizationRequest.Description,
            //Logo = organizationRequest.Logo,
        };

        await _db.Organizations.AddAsync(organization);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int organizationId, UpdateOrganizationRequest organizationRequest, CancellationToken ct)
    {
        var organization = await _db.Organizations.FindAsync(organizationId, ct);

        Guard.Against.Null(organization, nameof(organization));

        if (!string.IsNullOrEmpty(organizationRequest.EngName)) organization!.Name.Eng = organizationRequest.EngName;
        if (!string.IsNullOrEmpty(organizationRequest.LocName)) organization!.Name.Loc = organizationRequest.LocName;
        if (!string.IsNullOrEmpty(organizationRequest.Address)) organization!.Address = organizationRequest.Address;
        if (!string.IsNullOrEmpty(organizationRequest.Description)) organization!.Description = organizationRequest.Description;
        if (!string.IsNullOrEmpty(organizationRequest.Phone)) organization!.Phone = organizationRequest.Phone;
        //if (organizationRequest.Logo != null && organizationRequest.Logo.Length > 0) organization!.Logo = organizationRequest.Logo;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int organizationId, CancellationToken ct)
    {
        var organization = await _db.Organizations.FindAsync(organizationId, ct);

        Guard.Against.Null(organization, nameof(organization));

        organization!.IsActive = false;

        await _db.SaveChangesAsync();
    }
}
