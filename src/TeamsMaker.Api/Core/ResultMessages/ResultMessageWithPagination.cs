namespace Core.ResultMessages;

public class ResultMessageWithPagination : ResultMessage
{
    public ResultMessageWithPagination()
    {
        pagination = new PaginationData();
    }

    public PaginationData pagination { get; set; }
}


public class ResultMessageWithPagination<T> : ResultMessage<T> where T : notnull
{
    public ResultMessageWithPagination()
    {
        pagination = new PaginationData();
    }

    public PaginationData pagination { get; set; }
}


public class PaginationData
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
