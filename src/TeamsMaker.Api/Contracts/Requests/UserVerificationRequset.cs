namespace TeamsMaker.Api;

public class UserVerificationRequset
{
    [Required]
    public required string SSN { get; init; }
    
    [Required]
    public required string CollegeId { get; init; }
}
