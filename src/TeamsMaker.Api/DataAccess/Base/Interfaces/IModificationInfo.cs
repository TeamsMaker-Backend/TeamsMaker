namespace DataAccess.Base.Interfaces;

public interface IModificationInfo
{
    string ModifiedBy { get; }
    DateTime? LastModificationDate { get; }
}
