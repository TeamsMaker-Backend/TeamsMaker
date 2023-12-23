namespace DataAccess.Base.Interfaces;

public interface IUserInfo
{
    string UserName { get; }
    string UserId { get; }
    IEnumerable<string> Roles { get; }
    int OrganizationId { get; }
}
