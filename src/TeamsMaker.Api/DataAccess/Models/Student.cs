﻿using Core.ValueObjects;

namespace TeamsMaker.Api.DataAccess.Models;

public class Student : User
{
    public string CollegeId { get; set; } = null!;
    public float GPA { get; set; }
    public DateOnly? GraduationYear { get; set; }
    public int Level { get; set; }
    public FileData? CV { get; set; }
    public int DepartmentId { get; set; }
    public virtual Department? Department { get; set; }

    public virtual ICollection<Experience> Experiences { get; set; } = [];
    public virtual ICollection<Project> Projects { get; set; } = [];
    public virtual ICollection<JoinRequest> JoinRequests { get; set; } = [];
}
