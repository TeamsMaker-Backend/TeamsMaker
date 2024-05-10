using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class React : BaseEntity<Guid>
    {
        public Guid PostId { get; set; }

        public string UserId { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public virtual Post Post { get; set; } = null!;
    }
}
