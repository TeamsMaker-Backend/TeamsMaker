using Microsoft.IdentityModel.Tokens;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Core.Enums;
namespace TeamsMaker.Api.Services.Profiles;

public class ProfileUtilities //TODO: [Refactor] remove dublicates - Urgent
    (AppDBContext db, IServiceProvider serviceProvider)
{
    public void GetUserData(User user, GetProfileResponse response)
    {
        response.Id = user.Id;
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;
        response.SSN = user.SSN;
        response.Email = user.Email!;
        response.Bio = user.Bio;
        response.About = user.About;
        response.Gender = user.Gender;
        response.City = user.City;
        response.EmailConfirmed = user.EmailConfirmed;
        response.Phone = user.PhoneNumber;
        response.OrganizationId = user.OrganizationId;
        response.Links = user.Links.Select(l => new LinkInfo { Url = l.Url, Type = l.Type }).ToList();
    }

    public void GetOtherUserData(User user, GetOtherProfileResponse response)
    {
        response.Id = user.Id;
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;
        response.Email = user.Email!;
        response.Bio = user.Bio;
        response.About = user.About;
        response.Gender = user.Gender;
        response.City = user.City;
        response.Phone = user.PhoneNumber;
        response.Links = user.Links.Select(l => new LinkInfo { Url = l.Url, Type = l.Type }).ToList();
    }

    public void GetStudentData(Student student, GetProfileResponse response)
    {
        var studentFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);
        var circleFileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        var studentInfo = new StudentInfo
        {
            CollegeId = student.CollegeId,
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,
            CV = studentFileService.GetFileUrl(student.Id, FileTypes.CV),

            Experiences =
                student.Experiences.Select(ex => new ExperienceInfo
                {
                    Id = ex.Id,
                    Organization = ex.Organization,
                    Role = ex.Role,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    Description = ex.Description
                }).ToList(),

            Projects =
                student.Projects.Select(prj => new ProjectInfo
                {
                    Id = prj.Id,
                    Name = prj.Name,
                    Url = prj.Url,
                    Description = prj.Description,
                    StartDate = prj.StartDate,
                    EndDate = prj.EndDate,
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList(),


            StudentJoinRequests = new()
            {
                JoinRequests = student.JoinRequests
                    .Where(jr => jr.Sender == InvitationTypes.Student) // join request from users
                    .Where(jr => jr.IsAccepted == false)
                    .OrderByDescending(jr => jr.CreationDate)
                    .Take(3)
                    .Select(jr => new GetBaseJoinRequestResponse
                    {
                        JoinRequestId = jr.Id,
                        Sender = jr.Sender,
                        OtherSideId = jr.CircleId.ToString(),
                        OtherSideName = jr.Circle.Name,
                        Avatar = circleFileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar)
                    })
                    .ToList(),

                Invitations = student.JoinRequests
                    .Where(jr => jr.Sender == InvitationTypes.Circle) // invitation from circle
                    .Where(jr => jr.IsAccepted == false)
                    .OrderByDescending(jr => jr.CreationDate)
                    .Take(3)
                    .Select(jr => new GetBaseJoinRequestResponse
                    {
                        JoinRequestId = jr.Id,
                        Sender = jr.Sender,
                        OtherSideId = jr.CircleId.ToString(),
                        OtherSideName = jr.Circle.Name,
                        Avatar = circleFileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar)
                    })
                    .ToList(),
            },

            CircleInfo = student.MemberOn.IsNullOrEmpty() == false ? new()
            {
                Id = student.MemberOn.Single().CircleId,
                Name = student.MemberOn.Single().Circle.Name,
                OwnerName = db.CircleMembers
                    .Include(cm => cm.User)
                    .Where(cm => cm.CircleId == student.MemberOn.Single().CircleId)
                    .Where(cm => cm.IsOwner)
                    .Select(cm => $"{cm.User.FirstName} {cm.User.LastName}")
                    .Single(),
                Avatar = circleFileService.GetFileUrl(student.MemberOn.Single().CircleId.ToString(), FileTypes.Avatar)
            } : null
        };

        response.StudentInfo = studentInfo;
        response.Avatar = studentFileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = studentFileService.GetFileUrl(student.Id, FileTypes.Header);
    }

    public void GetOtherStudentData(Student student, GetOtherProfileResponse response)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var otherStudentInfo = new OtherStudentInfo
        {
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,
            CV = fileService.GetFileUrl(student.Id, FileTypes.CV),

            Experiences =
                student.Experiences.Select(ex => new ExperienceInfo
                {
                    Id = ex.Id,
                    Organization = ex.Organization,
                    Role = ex.Role,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    Description = ex.Description
                }).ToList(),

            Projects =
                student.Projects.Select(prj => new ProjectInfo
                {
                    Id = prj.Id,
                    Name = prj.Name,
                    Url = prj.Url,
                    StartDate = prj.StartDate,
                    EndDate = prj.EndDate,
                    Description = prj.Description,
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList()
        };

        response.StudentInfo = otherStudentInfo;
        response.Avatar = fileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(student.Id, FileTypes.Header);
    }

    public void GetStaffData(Staff staff, GetProfileResponse response)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Staff);

        var staffInfo = new StaffInfo
        {
            Classification = staff.Classification
        };

        response.StaffInfo = staffInfo;
        response.Avatar = fileService.GetFileUrl(staff.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(staff.Id, FileTypes.Header);
    }

    public void GetOtherStaffData(Staff staff, GetOtherProfileResponse response)
    {
        var otherStaffInfo = new OtherStaffInfo
        {
            Classification = staff.Classification
        };
        response.StaffInfo = otherStaffInfo;
    }

    private async Task<List<Circle>> GetStaffCircles(string staffId, bool isArchieved, CancellationToken ct)
    {
        var acceptedProposalIds = await db.ApprovalRequests
            .Where(ar => ar.ProposalStatusSnapshot == ProposalStatusEnum.ThirdApproval
                      && ar.IsAccepted == true)
            .Select(ar => ar.ProposalId)
            .ToListAsync(ct);

        var query = db.Proposals
            .Include(p => p.ApprovalRequests)
            .Include(p => p.Circle)
                .ThenInclude(c => c.CircleMembers)
                    .ThenInclude(cm => cm.User)
            .Where(p => acceptedProposalIds.Contains(p.Id))
            .Where(p => p.ApprovalRequests
                            .Any(ar => ar.StaffId == staffId
                                    && ar.ProposalStatusSnapshot == ProposalStatusEnum.SecondApproval));

        query = isArchieved
            ? query.Where(p => p.Circle.Status == CircleStatusEnum.Archived)
            : query.Where(p => p.Circle.Status == CircleStatusEnum.Active);

        var circles = await query
            .Select(p => p.Circle)
            .ToListAsync(ct);

        return circles;
    }

    public async Task<ICollection<GetCircleAsRowResponse>> GetStaffActiveCircles(string staffId, CancellationToken ct)
    {
        var circles = await GetStaffCircles(staffId, false, ct);

        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        return circles.Select(c => new GetCircleAsRowResponse
        {
            Id = c.Id,
            Name = c.Name,
            Avatar = fileService.GetFileUrl(c.Id.ToString(), FileTypes.Avatar),
            OwnerName = c.CircleMembers.First(cm => cm.IsOwner).User.FirstName + " "
                        + c.CircleMembers.First(cm => cm.IsOwner).User.LastName
        }).ToList();
    }

    public async Task<ICollection<GetCircleAsCardResponse>> GetStaffArchievedCircles(string staffId, CancellationToken ct)
    {
        var circles = await GetStaffCircles(staffId, true, ct);

        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Circle);

        return circles.Select(c => new GetCircleAsCardResponse
        {
            Id = c.Id,
            Name = c.Name,
            Rate = c.Rate,
            TechStack = c.Skills.Select(sk => sk.Name).ToList(),
            Summary = c.SummaryData?.Summary,
            ArchivedOn = c.ArchivedOn,
            CreationDate = c.CreationDate,
            Avatar = fileService.GetFileUrl(c.Id.ToString(), FileTypes.Avatar),
            Github = c.Links.FirstOrDefault(l => l.Type == LinkTypesEnum.GitHub)?.Url,
            OwnerName = c.CircleMembers.First(cm => cm.IsOwner).User.FirstName + " "
                        + c.CircleMembers.First(cm => cm.IsOwner).User.LastName,
            Supervisor = c.CircleMembers.FirstOrDefault(cm => cm.IsSupervisor)!.User.FirstName + " " 
                        + c.CircleMembers.FirstOrDefault(cm => cm.IsSupervisor)!.User.LastName
        }).ToList();
    }

    public void UpdateUserData(User user, UpdateProfileRequest request)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;
        user.About = request.About;
        user.Gender = request.Gender ?? GenderEnum.Unknown;
        user.City = request.City;
        user.PhoneNumber = request.Phone;

        db.Links.RemoveRange(user.Links);
        user.Links = request.Links?.Select(l => new Link { UserId = user.Id, Url = l.Url, Type = l.Type }).ToList() ?? [];
    }
}