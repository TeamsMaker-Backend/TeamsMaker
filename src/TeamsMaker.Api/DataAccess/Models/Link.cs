namespace TeamsMaker.Api.DataAccess.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;

        public string StudentId { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
