namespace TeamsMaker.Api.Configurations;

public class JwtConfig
{
    public string Secret { get; set; } = string.Empty;
    public TimeSpan ExpireyTimeFrame { get; set; }
}
