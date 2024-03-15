using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class JoinRequest :BaseEntity<int>
    {
        public string Sender { get; set; } = null!;
        public bool IsAccepted { get; set; } = false;
        public string UserId { get; set; } = null!;
        public Guid CircleId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Circle Circle { get; set; } = null!;
    }
}
