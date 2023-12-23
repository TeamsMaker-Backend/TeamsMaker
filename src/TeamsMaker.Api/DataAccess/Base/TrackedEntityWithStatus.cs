using DataAccess.Base.Interfaces;

namespace DataAccess.Base;

/// <summary>
///     Is an entity that keeps track of the adminstrative and lastStatus info.
///     adminstrative: like createDate/createdBy lastModificationDate/modifiedBy|  lastStatus: lastStatus properties.
///     this class extends EntityWithStatus<TId, TStatus> 
/// </summary>
/// <seealso cref="EntityWithStatus"/>
/// <seealso cref="TrackedEntity"/>
/// <seealso cref="ILastStatus"/>
/// <seealso cref="ITrackedEntityWithStatus"/>
/// <typeparam name="TId">Entity Id, e.g int, long, Guid</typeparam>
/// <typeparam name="TStatus">a Status Entity which is implementing an `IStatus` interface</typeparam>
public abstract class TrackedEntityWithStatus<TId, TStatus> : TrackedEntity<TId>, ITrackedEntityWithStatus<TId, TStatus> where TStatus : IStatus
{
    public int LastStatusId { get; protected set; }
    public string LastStatusCreatedBy { get; protected set; } = null!;
    public DateTime? LastStatusDateTime { get; protected set; }
    public virtual ICollection<TStatus> StatusHistory { get; protected set; } = new HashSet<TStatus>();


    protected abstract void _AddStatusHistoryRow(int statusId);


    protected void _ModifyStatus(int statusId)
    {
        _AddStatusHistoryRow(statusId);

        this.LastStatusId = statusId;
        this.LastStatusCreatedBy = this.ModifiedBy;
        this.LastStatusDateTime = this.LastModificationDate;
    }


    public virtual IEntity<TId> AddNewStatus(int statusId)
    {
        this.LastStatusId = statusId;
        this.LastStatusCreatedBy = this.ModifiedBy;
        this.LastStatusDateTime = DateTime.UtcNow;

        _AddStatusHistoryRow(statusId);
        return this;
    }

}


