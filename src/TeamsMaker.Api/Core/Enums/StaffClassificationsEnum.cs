namespace TeamsMaker.Api;


[Flags] // 0 0 0 0
public enum StaffClassificationsEnum
{
    HeadOfOrg = 0b0001, // 1
    Professor = 0b0010, // 2
    HeadOfDept = 0b0110, // 6
    Assistant = 0b1000, // 8
}