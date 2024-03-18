using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class JoinRequest : TrackedEntity<Guid>
    {
        public string Sender { get; set; } = null!;
        public bool IsAccepted { get; set; } = false;
        public string StudentId { get; set; } = null!;
        public Guid CircleId { get; set; }
        public virtual Student Student { get; set; } = null!;
        public virtual Circle Circle { get; set; } = null!;
    }
}

