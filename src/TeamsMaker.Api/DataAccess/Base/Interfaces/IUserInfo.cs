namespace DataAccess.Base.Interfaces;

public interface IUserInfo
{
    public string UserName { get; }
    public string UserId { get; }
    public IEnumerable<string> Roles { get; }
    public int OrganizationId { get; }
}
