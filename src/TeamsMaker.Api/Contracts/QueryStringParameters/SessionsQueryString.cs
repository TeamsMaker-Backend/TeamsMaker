namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class SessionsQueryString : BaseQueryStringParametersWithPagination
{
    public SessionStatus? Status { get; init; }
}
