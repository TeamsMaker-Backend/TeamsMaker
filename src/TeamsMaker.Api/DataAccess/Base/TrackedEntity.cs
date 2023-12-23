using DataAccess.Base.Interfaces;

namespace DataAccess.Base;

#nullable disable
/// <summary>
///     Is an entity that keeps track of the adminstrative data like creation and modification info
/// </summary>
/// <seealso cref="IEntity<TId>"/>
/// <seealso cref="ICreationInfo"/>
/// <seealso cref="IModificationInfo"/>
public abstract class TrackedEntity<TId> : BaseEntity<TId>, ITrackedEntity<TId>
{
    public string CreatedBy { get; protected set; }

    public DateTime? CreationDate { get; protected set; }

    public string ModifiedBy { get; protected set; }

    public DateTime? LastModificationDate { get; protected set; }
}
