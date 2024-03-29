﻿using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.JoinRequest;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
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
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var studentInfo = new StudentInfo
        {
            CollegeId = student.CollegeId,
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
                    Description = prj.Description,
                    StartDate = prj.StartDate,
                    EndDate = prj.EndDate,
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList(),

            CircleJoinRequests =
                student.JoinRequests
                .Where(jr => jr.Sender == InvitationTypes.Student) // join request from users
                .OrderByDescending(jr => jr.CreationDate)
                .Take(3)
                .Select(jr => new GetCircleJoinRequestResponse
                {
                    Id = jr.Id,
                    CircleId = jr.CircleId,
                    Name = jr.Circle.Name,
                    Avatar = fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar)
                })
                .ToList(),

            Invitations =
                student.JoinRequests
                    .Where(jr => jr.Sender == InvitationTypes.Circle) // invitation from circle
                    .OrderByDescending(jr => jr.CreationDate)
                    .Take(3)
                    .Select(jr => new GetCircleJoinRequestResponse
                    {
                        Id = jr.Id,
                        CircleId = jr.CircleId,
                        Name = jr.Circle.Name,
                        Avatar = fileService.GetFileUrl(jr.CircleId.ToString(), FileTypes.Avatar)
                    })
                    .ToList(),
        };

        response.StudentInfo = studentInfo;
        response.Avatar = fileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(student.Id, FileTypes.Header);
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

        response.OtherStudentInfo = otherStudentInfo;
        response.Avatar = fileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(student.Id, FileTypes.Header);
    }

    public void GetStaffData(Staff staff, GetProfileResponse response)
    {
        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

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
        response.OtherStaffInfo = otherStaffInfo;
    }

    public async Task UpdateUserDataAsync(User user, UpdateProfileRequest request, string folder, CancellationToken ct)
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

        var fileService = serviceProvider.GetRequiredKeyedService<IFileService>(GetBaseType(user));

        await fileService
            .UpdateFileAsync(user.Id, FileTypes.Avatar, new UpdateFileRequest { File = request.Avatar }, ct);

        await fileService
            .UpdateFileAsync(user.Id, FileTypes.Header, new UpdateFileRequest { File = request.Header }, ct);

        if (user is Student student)
            await fileService
                .UpdateFileAsync(student.Id, FileTypes.CV, new UpdateFileRequest { File = request.CV }, ct);
    }

    private static string? GetBaseType(User user)
        => user is Student ? BaseTypes.Student : BaseTypes.Staff;
}