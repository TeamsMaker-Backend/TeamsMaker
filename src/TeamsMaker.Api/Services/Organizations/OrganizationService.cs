using Core.Generics;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Organizations;

public class OrganizationService(AppDBContext db, IStorageService storageService, IServiceProvider serviceProvider, IWebHostEnvironment host) : IOrganizationService
{
    private readonly AppDBContext _db = db;
    private readonly IStorageService _storageService = storageService;
    private readonly IWebHostEnvironment _host = host;
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Organization);

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
                Logo = _fileService.GetFileUrl(org.Id.ToString(), FileTypes.Logo)
            });

        if (queryString.OrganizationId.HasValue) query = query.Where(org => org.Id == queryString.OrganizationId);

        return await PagedList<GetOrganizationResponse>.ToPagedListAsync(query, queryString.PageNumber, queryString.PageSize);
    }

    public async Task AddAsync(OrganizationRequest request, CancellationToken ct)
    {
        var organization = new Organization
        {
            Name = new(request.EngName, request.LocName),
            Address = request.Address,
            Phone = request.Phone,
            Description = request.Description
        };
        await _db.Organizations.AddAsync(organization, ct);
        await _db.SaveChangesAsync(ct);

        organization.Logo = await _storageService.UpdateFileAsync(null, request.Logo, CreateName(FileTypes.Logo, request.Logo?.FileName),
            Path.Combine(_host.WebRootPath, BaseTypes.Organization, organization.Id.ToString()), ct);

        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(int id, OrganizationRequest request, CancellationToken ct)
    {
        var organization = await _db.Organizations.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Id!");

        organization.Name.Eng = request.EngName;
        organization.Name.Loc = request.LocName;
        organization.Address = request.Address;
        organization.Description = request.Description;
        organization.Phone = request.Phone;
        organization.Logo = await _storageService.UpdateFileAsync(organization.Logo?.Name, request.Logo, CreateName(FileTypes.Logo, request.Logo?.FileName),
            Path.Combine(_host.WebRootPath, BaseTypes.Organization, organization.Id.ToString()), ct);

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var organization = await _db.Organizations.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Id!");

        organization.IsActive = false;

        await _db.SaveChangesAsync(ct);
    }

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}