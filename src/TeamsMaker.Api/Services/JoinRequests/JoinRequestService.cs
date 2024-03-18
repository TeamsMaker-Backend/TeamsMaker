using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Models;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamsMaker.Api.Services.Files;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;


namespace TeamsMaker.Api.Services.JoinRequests;

public class JoinRequestService(AppDBContext db,IServiceProvider serviceProvider) : IJoinRequestService
{
    private readonly IFileService fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

    public async Task AddJoinRequestAsync(AddJoinRequest request, CancellationToken ct)
    {
        var studentId = await db.Students.FindAsync([request.StudentId], ct);
        var circleId = await db.Circles.FindAsync([request.CircleId], ct);

        if (studentId == null)
            throw new ArgumentException("Invalid User Id");

        if (circleId == null)
            throw new ArgumentException("Invalid Circle Id");

        if (request.EntityType.ToLower() == InvitationTypes.Circle.ToLower() ||
            request.EntityType.ToLower() == InvitationTypes.Student.ToLower())
        {
            var joinRequest = new JoinRequest
            {
                Sender = request.EntityType,
                IsAccepted = false,
                StudentId = request.StudentId,
                CircleId = request.CircleId,
            };

            await db.JoinRequests.AddAsync(joinRequest, ct);
            await db.SaveChangesAsync(ct);

        }else
            throw new ArgumentException("Invalid Entity Type");
    }

    public async Task<List<GetCircleJoinRequestResponse>> GetCircleJoinRequesAsync(string id, CancellationToken ct)
    {
        var studentId = await db.JoinRequests
        .FirstOrDefaultAsync(jr => jr.StudentId == id) ??
        throw new ArgumentException("Invalid Student Id");

        var response = await db.JoinRequests
        .Where(jr => jr.Sender == InvitationTypes.Student && jr.StudentId == id)
        .Select(jr => new GetCircleJoinRequestResponse
        {
            Id = jr.Id,
            Name = jr.Circle.Name,
            Avatar = fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar),
            CircleId = jr.CircleId,
        })
        .ToListAsync(ct); 

        return response;
    }

    public async Task AcceptJoinRequestAsync(Guid id, CancellationToken ct)
    {
        var joinRequest = await db.JoinRequests.FindAsync([id], ct);

        if (joinRequest == null)
            throw new ArgumentException("Invalid Join Request Id");

        joinRequest.IsAccepted = true;

        await db.SaveChangesAsync(ct);


        //TODO
        // Add Cirle Member
    }

    public async Task DeleteJoinRequestAsync(Guid id, CancellationToken ct)
    {
        var joinRequest =
            await db.JoinRequests.FindAsync([id], ct) ??
            throw new ArgumentException("Not found");

        db.JoinRequests.Remove(joinRequest);

        await db.SaveChangesAsync(ct);
    }

}

