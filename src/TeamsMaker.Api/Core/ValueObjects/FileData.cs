namespace Core.ValueObjects;

public record class FileData
{
    public required string Name { get; set; }
    public required string ContentType { get; set; }
}
