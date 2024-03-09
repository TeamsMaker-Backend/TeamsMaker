namespace TeamsMaker.Api.Core.Consts;

public static class AppRoles
{
    public static readonly string Admin = "Admin";
    public static readonly string HeadOfDept = "HeadOfDept";
    public static readonly string Professor = "Professor";
    public static readonly string Assistant = "Assistant";
    public static readonly string Student = "Student";

    public static readonly List<string> StaffRoles =
    [
        Admin,
        HeadOfDept,
        Professor,
        Assistant
    ];

    public static readonly List<string> OrdinaryRoles =
    [
        HeadOfDept,
        Professor,
        Assistant,
        Student
    ];
}
