namespace TeamsMaker.Api.Contracts.Requests.Post;

public class AddPostRequest
{
    public Guid? CircleId { get; set; }
    public string Content { get; set; } = null!;
    public long LikesNumber { get; set; }
    public Guid? ParentPostId { get; set; }

}

