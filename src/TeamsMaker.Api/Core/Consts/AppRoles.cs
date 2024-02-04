namespace TeamsMaker.Api.Core.Consts;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string HeadOfDept = "HeadOfDept";
    public const string Professor = "Professor";
    public const string Assistant = "Assistant";
    public const string Student = "Student";

    public static List<string> OrdinaryRoles = 
    [
        HeadOfDept,
        Professor,
        Assistant,
        Student
    ];
}
