using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Services.ProfileService;

internal static class Utilities
{
    internal static void GetUserData(User user, ProfileResponse response)
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

    internal static async void UpdateUserDataAsync(User user, UpdateProfileRequest request, string folder, CancellationToken ct)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;
        user.About = request.About;
        user.Gender = (GenderEnum)request.Gender;
        user.City = request.City;
        user.PhoneNumber = request.Phone;

        user.Avatar = await UpdateFileAsync(user.Avatar, request.Avatar, CreateName(user.Id, request.Avatar?.FileName)
            , Path.Combine(folder, FilesPath.AvatarFolder), ct);

        user.Header = await UpdateFileAsync(user.Header, request.Header, CreateName(user.Id, request.Header?.FileName)
            , Path.Combine(folder, FilesPath.HeaderFolder), ct);

        if (user is Student student)
            student.CV = await UpdateFileAsync(student.CV, request.CV, CreateName(student.Id, request.CV?.FileName)
                , Path.Combine(folder, FilesPath.CVFolder), ct);
    }

    internal static async Task<string?> UpdateFileAsync(string? oldFile, IFormFile? newFile, string? newFileName, string folder, CancellationToken ct)
    {
        if (oldFile != null && File.Exists(Path.Combine(folder, oldFile)))
            File.Delete(Path.Combine(folder, oldFile));

        if (newFile == null || newFile.Length == 0)
            return null;

        var newFilePath = Path.Combine(folder, newFileName!);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await newFile.CopyToAsync(stream, ct);
        }

        return newFileName;
    }

    internal static string? CreateName(string id, string? file)
        => $"{id}{Path.GetExtension(file) ?? "NA"}";
}
