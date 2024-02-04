using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers;

public class ListOrganizationsEndpoint : BaseApiController
{
    private readonly AppDBContext _db;

    public ListOrganizationsEndpoint(AppDBContext db)
    {
        _db = db;
    }


    [HttpGet("organization")]
    public async Task<IActionResult> AddOrganization()
    {
        var count = await _db.Organizations.CountAsync();

        return Ok(_response.SuccessResponse( await _db.Organizations.ToListAsync()));
    }
}