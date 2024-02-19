using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.ProfileService.Interface;

namespace TeamsMaker.Api.Services.ProfileService;

public class ProfileService : IProfileService
{
    private readonly AppDBContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProfileService(AppDBContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<ProfileResponse> GetProfileAsync(string email, CancellationToken ct)
    {
        ProfileResponse response = new();

        if (await _db.Students.AnyAsync(x => x.Email == email, ct))
        {
            var student = await _db.Students.SingleAsync(x => x.Email == email, ct);

            GetStudentData(student, response);
            Utilities.GetUserData(student, response);
        }
        else if (await _db.Staff.AnyAsync(x => x.Email == email, ct))
        {
            var staff = await _db.Staff.SingleAsync(x => x.Email == email, ct);

            GetStaffData(staff, response);
            Utilities.GetUserData(staff, response);
        }
        else
            throw new ArgumentException("Invalid Data");

        return response;
    }

    public async Task UpdateProfileAsync(Guid id, UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        if (await _db.Students.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var student = await _db.Students.SingleAsync(x => x.Id == id.ToString(), ct);

            Utilities.UpdateUserDataAsync(student, profileRequest, Path.Combine(_hostEnvironment.WebRootPath, FilesPath.StudentFolder), ct);
        }
        else if (await _db.Staff.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var staff = await _db.Staff.SingleAsync(x => x.Id == id.ToString(), ct);

            Utilities.UpdateUserDataAsync(staff, profileRequest, Path.Combine(_hostEnvironment.WebRootPath, FilesPath.StaffFolder), ct);
        }
        else
            throw new ArgumentException("Invalid Data");

        await _db.SaveChangesAsync(ct);
    }

    private static void GetStudentData(Student student, ProfileResponse response)
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

    private static void GetStaffData(Staff staff, ProfileResponse response)
    {
        StaffInfo staffInfo = new()
        {
            Classification = (int)staff.Classification
        };
        response.StaffInfo = staffInfo;
    }
}
