namespace TeamsMaker.Api.Contracts.Requests;

public class AddDepartmentRequest
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}
