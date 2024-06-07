using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class ArchiveQueryString : BaseQueryStringWithQ
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? ArchivedOn { get; set; }
    public string? SupervisorId { get; set; }
    public string? Technologies { get; set; }
    public int? DepartmentId { get; set; }
    public bool? SortByUpvotesDesc { get; set; }
    public bool? SortByUpvotesAsc { get; set; }
    public bool? SortByCreationDateAsc { get; set; } // desc default
}