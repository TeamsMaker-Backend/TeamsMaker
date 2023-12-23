using DataAccess.Base.Interfaces;

namespace DataAccess.Base;

#nullable disable
public interface ISoftDeletable
{
    bool IsDeleted { get; protected set; }
    DateTime? DeletedOn { get; protected set; }

    void Delete()
    {
        IsDeleted = true;
        DeletedOn = DateTime.Now;
    }

    void UndoDelete()
    {
        IsDeleted = false;
        DeletedOn = null;
    }
}
