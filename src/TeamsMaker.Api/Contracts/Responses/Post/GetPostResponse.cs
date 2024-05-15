namespace TeamsMaker.Api.Contracts.Responses.Post;

public class GetPostResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public long LikesNumber { get; set; }
    public long CommentsNumber { get; set; }
    public Guid AuthorId { get; set; }
    public string CreatedBy { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? LastModificationDate { get; set; }
    public virtual ICollection<GetPostResponse> Comments { get; set; } = [];
}