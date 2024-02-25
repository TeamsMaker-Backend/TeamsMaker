using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.ProfileService.Utilities;

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
        response.Gender = (int)user.Gender;
        response.City = user.City;
        response.EmailConfirmed = user.EmailConfirmed;
        response.Phone = user.PhoneNumber;
        foreach (var link in user.Links)
            response.Links.Add(link.Url);
    }

    public static void GetStudentData(Student student, GetProfileResponse response)
    {
        StudentInfo studentInfo = new()
        {
            CollegeId = student.CollegeId,
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name
        };
        response.StudentInfo = studentInfo;
    }

    public static void GetStaffData(Staff staff, GetProfileResponse response)
    {
        StaffInfo staffInfo = new()
        {
            Classification = (int)staff.Classification
        };
        response.StaffInfo = staffInfo;
    }

    public static async void UpdateUserDataAsync(User user, UpdateProfileRequest request, string folder, CancellationToken ct)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;
        user.About = request.About;
        user.Gender = (GenderEnum)(request.Gender ?? (int)GenderEnum.Unknown);
        user.City = request.City;
        user.PhoneNumber = request.Phone;

        foreach (var link in request.Links ?? [])
            user.Links.Add(new Link { UserId = user.Id, Url = link });

        user.Avatar = await FileUtilities.UpdateFileAsync(user.Avatar?.Name, request.Avatar, FileUtilities.CreateName(user.Id, request.Avatar?.FileName),
            Path.Combine(folder, FileTypes.Avatar), ct);

        user.Header = await FileUtilities.UpdateFileAsync(user.Header?.Name, request.Header, FileUtilities.CreateName(user.Id, request.Header?.FileName),
            Path.Combine(folder, FileTypes.Header), ct);

        if (user is Student student)
        {
            student.CV = await FileUtilities.UpdateFileAsync(student.CV?.Name, request.CV, FileUtilities.CreateName(student.Id, request.CV?.FileName),
                Path.Combine(folder, FileTypes.CV), ct);
        }

        //TODO: if (user is Staff staff)
    }
}