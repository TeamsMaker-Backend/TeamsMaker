using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class React:BaseEntity<Guid>
    {
        public Guid PostId { get; set; }
        public string? UserId { get; set; }
        public Guid? CircleId { get; set; }

        public virtual User? User { get; set; }
        public virtual Circle? Circle { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
