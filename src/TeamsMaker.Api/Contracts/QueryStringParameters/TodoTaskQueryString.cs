namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class TodoTaskQueryString : BaseQueryStringParametersWithPagination
{
    public TodoTaskStatus? Status { get; init; }
}
