using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.Profiles.Utilities;

public static class ProfileUtilities
{
    public static void GetUserData(User user, GetProfileResponse response)
    {
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
        response.Links = user.Links.Select(l => l.Url).ToList();
    }

    public static void GetOtherUserData(User user, GetOtherProfileResponse response)
    {
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;
        response.Email = user.Email!;
        response.Bio = user.Bio;
        response.About = user.About;
        response.Gender = user.Gender;
        response.City = user.City;
        response.Phone = user.PhoneNumber;
        response.Links = user.Links.Select(x => x.Url).ToList();
    }

    public static void GetStudentData(Student student, GetProfileResponse response)
    {
        var studentInfo = new StudentInfo
        {
            CollegeId = student.CollegeId,
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,

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
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList()
        };

        response.StudentInfo = studentInfo;
    }

    public static void GetOtherStudentData(Student student, GetOtherProfileResponse response)
    {
        var otherStudentInfo = new OtherStudentInfo
        {
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,

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
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList()
        };

        response.OtherStudentInfo = otherStudentInfo;
    }

    public static void GetStaffData(Staff staff, GetProfileResponse response)
    {
        var staffInfo = new StaffInfo
        {
            Classification = staff.Classification
        };
        response.StaffInfo = staffInfo;
    }

    public static void GetOtherStaffData(Staff staff, GetOtherProfileResponse response)
    {
        var otherStaffInfo = new OtherStaffInfo
        {
            Classification = staff.Classification
        };
        response.OtherStaffInfo = otherStaffInfo;
    }

    public static async void UpdateUserDataAsync(User user, UpdateProfileRequest request, string folder, CancellationToken ct)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;
        user.About = request.About;
        user.Gender = request.Gender ?? GenderEnum.Unknown;
        user.City = request.City;
        user.PhoneNumber = request.Phone;
        user.Links = request.Links?.Select(l => new Link { UserId = user.Id, Url = l }).ToList() ?? [];

        user.Avatar = await FileUtilities.UpdateFileAsync(user.Avatar?.Name, request.Avatar, FileUtilities.CreateName(user.Id, request.Avatar?.FileName),
            Path.Combine(folder, FileTypes.Avatar), ct);

        user.Header = await FileUtilities.UpdateFileAsync(user.Header?.Name, request.Header, FileUtilities.CreateName(user.Id, request.Header?.FileName),
            Path.Combine(folder, FileTypes.Header), ct);

        if (user is Student student)
        {
            student.CV = await FileUtilities.UpdateFileAsync(student.CV?.Name, request.CV, FileUtilities.CreateName(student.Id, request.CV?.FileName),
                Path.Combine(folder, FileTypes.CV), ct);
        }
    }
}