namespace TeamsMaker.Api.Contracts.Requests.Post;

public class AddPostRequest
{
    public Guid? CircleId { get; init; }
    public string Content { get; init; } = null!;
    public Guid? ParentPostId { get; init; }
}
