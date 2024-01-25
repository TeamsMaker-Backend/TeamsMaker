namespace DataAccess.Base.Interfaces;

public interface IUserInfo
{
    public string UserName { get; }
    public Ulid UserId { get; }
    public IEnumerable<string> Roles { get; }
    public int OrganizationId { get; }
}
