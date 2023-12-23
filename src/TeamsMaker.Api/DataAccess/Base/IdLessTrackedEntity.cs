namespace DataAccess.Base.Interfaces;

#nullable disable

/// <summary>
///     Is an entity without Id col that keeps track of the adminstrative data like creation and modification info
/// (you may use this if you are using composite keys)
/// </summary>
/// <seealso cref="ICreationInfo"/>
/// <seealso cref="IModificationInfo"/>
public abstract class IdLessTrackedEntity: IdLessEntity, ICreationInfo, IModificationInfo
{
    public string CreatedBy { get; protected set; }

    public DateTime? CreationDate { get; protected set; }

    public string ModifiedBy { get; protected set; }

    public DateTime? LastModificationDate { get; protected set; }
}
