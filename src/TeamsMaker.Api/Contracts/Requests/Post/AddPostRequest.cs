namespace TeamsMaker.Api.Contracts.Requests.Post;

public class AddPostRequest
{
    public Guid? CircleId { get; set; }
    public string Content { get; set; } = null!;
    public Guid? ParentPostId { get; set; }

}

