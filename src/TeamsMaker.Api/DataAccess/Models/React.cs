using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class React:BaseEntity<Guid>
    {
        public string UserId { get; set; } = null!;
        public Guid CircleId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Circle Circle { get; set; } = null!;
    }
}
