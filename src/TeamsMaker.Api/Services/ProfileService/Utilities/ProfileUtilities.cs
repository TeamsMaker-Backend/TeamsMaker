using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.ProfileService.Utilities;

public static class ProfileUtilities
{
    public static void GetUserData(User user, ProfileResponse response)
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
    }

    public static void GetStudentData(Student student, ProfileResponse response)
    {
        StudentInfo studentInfo = new()
        {
            CollegeId = student.CollegeId,
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,
            Links = student.Links,
        };
        response.StudentInfo = studentInfo;
    }

    public static void GetStaffData(Staff staff, ProfileResponse response)
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

        user.Avatar = await FileUtilities.UpdateFileAsync(user.Avatar, request.Avatar, FileUtilities.CreateName(user.Id, request.Avatar?.FileName),
            Path.Combine(folder, FileTypes.Avatar), ct);

        user.Header = await FileUtilities.UpdateFileAsync(user.Header, request.Header, FileUtilities.CreateName(user.Id, request.Header?.FileName),
            Path.Combine(folder, FileTypes.Header), ct);

        if (user is Student student)
        {
            student.CV = await FileUtilities.UpdateFileAsync(student.CV, request.StudentUpdateInfo!.CV, FileUtilities.CreateName(student.Id, request.StudentUpdateInfo.CV?.FileName),
                Path.Combine(folder, FileTypes.CV), ct);
            student.Links = request.StudentUpdateInfo.Links;
        }

        //TODO: if (user is Staff staff)
    }
}
