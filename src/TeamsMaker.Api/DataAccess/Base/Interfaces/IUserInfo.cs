namespace DataAccess.Base.Interfaces;

public interface IUserInfo
{
    public string UserId { get; } 
    public string UserName { get; }
    public string Email { get; }
    public IEnumerable<string> Roles { get; }
    public int OrganizationId { get; }
}
