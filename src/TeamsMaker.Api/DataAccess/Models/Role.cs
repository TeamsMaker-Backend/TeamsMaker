namespace DataAccess.Models;

public class Role : IdentityRole
{
    private Role()
    {
        IsActive = true;
    }

    public bool IsActive { get; private set; }

    public static Role Create() => new();
}
