namespace Core.ValueObjects;

public record class CircleInfoPermissions
{
    public bool UpdateFiles { get; set; }
    public bool UpdateInfo { get; set; }
}