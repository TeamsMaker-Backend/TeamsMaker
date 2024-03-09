namespace Core.ValueObjects;

public record class SummaryData
{
    public string Summary { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
}