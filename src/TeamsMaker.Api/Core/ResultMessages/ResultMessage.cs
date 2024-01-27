namespace TeamsMaker.Api.Core.ResultMessages;

public class ResultMessage
{
    public required string EngMsg { get; set; }
    public required string LocMsg { get; set; }
    public bool Success { get; set; }
    public object? exception { get; set; }
    public object? ReturnObject { get; set; }
}


public class ResultMessage<T> where T : notnull
{
    public required string EngMsg { get; set; }
    public required string LocMsg { get; set; }
    public bool Success { get; set; }
    public object? exception { get; set; }
    public required T ReturnObject { get; set; }
}