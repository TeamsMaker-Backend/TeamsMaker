namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class BaseQueryStringParametersWithPagination
{
    const int MAX_PAGE_SIZE = 200;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 50;

    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            _pageSize = Math.Min(value, MAX_PAGE_SIZE);
        }
    }
}
