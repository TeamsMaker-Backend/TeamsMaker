﻿namespace TeamsMaker.Api.Core.ResultMessages;

public class ResultMessage
{
    public required string EngMsg { get; set; }
    public bool Success { get; set; }
    public object? Exception { get; set; }
    public object? ReturnObject { get; set; }
}


public class ResultMessage<T> where T : notnull
{
    public required string EngMsg { get; set; }
    public bool Success { get; set; }
    public object? Exception { get; set; }
    public required T ReturnObject { get; set; }
}