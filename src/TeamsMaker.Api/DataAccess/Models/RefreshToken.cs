using DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class RefreshToken : BaseEntity<int>
{
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool IsInvoked { get; set; }
    public DateTime AddedOn { get; set; }
    public DateTime ExpiresOn { get; set; }

    public Ulid UserId { get; set; }
    public User User { get; set; } = null!;
}
