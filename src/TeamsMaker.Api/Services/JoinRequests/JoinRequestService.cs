using TeamsMaker.Api.Contracts.Requests.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.JoinRequests.Interfaces;


namespace TeamsMaker.Api.Services.JoinRequests;

public class JoinRequestService(AppDBContext db, IServiceProvider serviceProvider) : IJoinRequestService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

    public async Task AddAsync(AddJoinRequest request, CancellationToken ct)
    {
        var studentId = await db.Students.FindAsync([request.StudentId], ct) ??
            throw new ArgumentException("Invalid User Id");

        var circleId = await db.Circles.FindAsync([request.CircleId], ct) ??
            throw new ArgumentException("Invalid Circle Id");

        if (!request.EntityType.Equals(InvitationTypes.Circle, StringComparison.CurrentCultureIgnoreCase) ||
            !request.EntityType.Equals(InvitationTypes.Student, StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException("Invalid Entity Type");

        var joinRequest = new JoinRequest
        {
            Sender = request.EntityType,
            IsAccepted = false,
            StudentId = request.StudentId,
            CircleId = request.CircleId,
        };

        await db.JoinRequests.AddAsync(joinRequest, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<List<GetCircleJoinRequestResponse>> GetAsync(string id, CancellationToken ct)
    {
        var studentId = await db.JoinRequests
            .SingleOrDefaultAsync(jr => jr.StudentId == id, ct) ??
            throw new ArgumentException("Invalid Student Id");

        var response = await db.JoinRequests
            .Where(jr => jr.Sender == InvitationTypes.Student && jr.StudentId == id)
            .Select(jr => new GetCircleJoinRequestResponse
            {
                Id = jr.Id,
                Name = jr.Circle.Name,
                Avatar = _fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar),
                CircleId = jr.CircleId,
            })
            .ToListAsync(ct);

        return response;
    }

    public async Task AcceptAsync(Guid id, CancellationToken ct)
    {
        var joinRequest = await db.JoinRequests.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Join Request Id");

        joinRequest.IsAccepted = true;

        await db.SaveChangesAsync(ct);


        //TODO
        // Add Cirle Member
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var joinRequest =
            await db.JoinRequests.FindAsync([id], ct) ??
            throw new ArgumentException("Not found");

        db.JoinRequests.Remove(joinRequest);

        await db.SaveChangesAsync(ct);
    }
}
