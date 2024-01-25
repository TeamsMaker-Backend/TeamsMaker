namespace Core.ValueObjects;

public class TranslatableFullName
{
    public static readonly TranslatableFullName Empty = new(TranslatableString.Empty, TranslatableString.Empty, TranslatableString.Empty, TranslatableString.Empty, TranslatableString.Empty);

    public TranslatableString FirstName { get; } = null!;
    public TranslatableString SecondName { get; } = null!;
    public TranslatableString ThirdName { get; } = null!;
    public TranslatableString FourthName { get; } = null!;
    public TranslatableString FifthName { get; } = null!;

    public string FullEngName => $"{FirstName.Eng ?? string.Empty} {SecondName.Eng ?? string.Empty} {ThirdName.Eng ?? string.Empty} {FourthName.Eng ?? string.Empty}".Trim();
    public string FullLocName => $"{FirstName.Loc ?? string.Empty} {SecondName.Loc ?? string.Empty} {ThirdName.Loc ?? string.Empty} {FourthName.Eng ?? string.Empty}".Trim();

    protected TranslatableFullName()
    { }

    public TranslatableFullName(TranslatableString firstName, 
        TranslatableString secondName, TranslatableString thirdName, 
        TranslatableString fourthName, TranslatableString fifthName) : this()
    {
        FirstName = firstName;
        SecondName = secondName;
        ThirdName = thirdName;
        FourthName = fourthName;
        FifthName = fifthName;
    }
}