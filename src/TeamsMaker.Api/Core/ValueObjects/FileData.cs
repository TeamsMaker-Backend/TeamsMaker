namespace Core.ValueObjects;

public record class FileData
{
    public static readonly FileData Empty = new();

    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;

    protected FileData() { }

    public FileData(string name, string contentType) : this()
    {
        Name = name;
        ContentType = contentType;
    }
}
