﻿using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.Responses.Profile;

public class GetProfileResponse
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string SSN { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Bio { get; set; }
    public string? About { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public string? City { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    public string? Phone { get; set; }
    public ICollection<string> Links { get; set; } = [];

    public string? Avatar { get; set; }
    public string? Header { get; set; }

    public StudentInfo? StudentInfo { get; set; }
    public StaffInfo? StaffInfo { get; set; }
}

public class StudentInfo
{
    public string CollegeId { get; set; } = null!;
    public float GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public int Level { get; set; } = 4;
    public string? DepartmentName { get; set; }

    public string? CV { get; set; }
}

public class StaffInfo
{
    public StaffClassificationsEnum Classification { get; set; }
}