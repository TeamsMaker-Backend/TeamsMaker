namespace TeamsMaker.Api.Contracts.Requests.JoinRequest
{
    public class AddJoinRequest
    {
        public string UserId { get; set; } = null!;
        public Guid CircleId { get; set; }
        public string EntityType { get; set; } = null!;
    }
}
