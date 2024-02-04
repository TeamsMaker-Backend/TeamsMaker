namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class BaseQueryStringWithQ : BaseQueryStringParametersWithPagination
{
    public string Q { get; init; } = string.Empty;
}
