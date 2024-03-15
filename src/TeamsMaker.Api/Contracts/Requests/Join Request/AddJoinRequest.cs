namespace TeamsMaker.Api.Contracts.Requests.Join_Request
{
    public class AddJoinRequest
    {
        public string UserId { get; set; } = null!;
        public Guid CircleId { get; set; }
    }
}
