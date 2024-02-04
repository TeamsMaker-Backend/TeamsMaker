namespace TeamsMaker.Api.Contracts.Requests;

public class UserVerificationRequset
{
    public required string SSN { get; init; }
    public string? CollegeId { get; init; } 
}
