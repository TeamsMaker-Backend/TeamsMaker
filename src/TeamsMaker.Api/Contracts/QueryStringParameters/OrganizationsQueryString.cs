namespace TeamsMaker.Api.Contracts.QueryStringParameters;

public class OrganizationsQueryString : BaseQueryStringParametersWithPagination
{
    public int? OrganizationId { get; init; }
}
