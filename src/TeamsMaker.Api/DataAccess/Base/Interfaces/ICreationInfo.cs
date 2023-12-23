namespace DataAccess.Base.Interfaces;

public interface ICreationInfo
{
    string CreatedBy { get; }
    DateTime? CreationDate { get; }
}
