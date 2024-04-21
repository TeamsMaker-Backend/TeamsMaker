using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Post : TrackedEntity<Guid>
{
    public string Content { get; set; } = null!;
    public long Likes { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? ParentPostId { get; set; }

    public virtual Author Author { get; set; } = null!;
    public virtual Post? ParentPost { get; set; }
    public virtual ICollection<Post> Comments { get; set; } = [];
}
