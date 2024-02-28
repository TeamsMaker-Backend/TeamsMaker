using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models
{
    public class Skill : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;
    }
}