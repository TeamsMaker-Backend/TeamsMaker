﻿namespace Core.ValueObjects;

// TODO: refactor
public record class TranslatableString
{
    public static readonly TranslatableString Empty = new();

    public string Eng { get; } = string.Empty;
    public string Loc { get; } = string.Empty;

    protected TranslatableString() { }

    public TranslatableString(string engName, string locName) : this()
    {
        Eng = engName;
        Loc = locName;
    }
}
