using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Profile;

public class GetOtherProfileResponse
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
    public string? About { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public string? City { get; set; }
    public string? Phone { get; set; }
    public ICollection<LinkInfo>? Links { get; set; } = [];

    public string? Avatar { get; set; }
    public string? Header { get; set; }

    public OtherStudentInfo? StudentInfo { get; set; }
    public OtherStaffInfo? StaffInfo { get; set; }
}

public class OtherStudentInfo
{
    public float GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public int Level { get; set; } = 4;
    public string? DepartmentName { get; set; }

    public string? CV { get; set; }

    public ICollection<ExperienceInfo>? Experiences { get; set; } = [];
    public ICollection<ProjectInfo>? Projects { get; set; } = [];
}

public class OtherStaffInfo
{
    public StaffClassificationsEnum Classification { get; set; }
    public ICollection<GetCircleAsRowResponse> Circles { get; set; } = [];
    public ICollection<GetCircleAsCardResponse> Archive { get; set; } = [];
}
