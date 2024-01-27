using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Base;

/// <summary>
///     EntityWithStatus: Is an abstract class for entities that store LastStatus data and keeps track of
/// </summary>
/// <typeparam name="TId">Entity Id, e.g int, long, Guid</typeparam>
/// <typeparam name="TStatus">a Status Entity which is implementing an `IStatus` interface</typeparam>
public abstract class EntityWithStatus<TId, TStatus> : BaseEntity<TId>, ILastStatus<TStatus> where TStatus : IStatus
{
    public int LastStatusId { get; private set; }

    public string LastStatusCreatedBy { get; private set; } = String.Empty;

    public DateTime? LastStatusDateTime { get; private set; }

    public TStatus LastStatus { get; private set; } = default!;

    public virtual ICollection<TStatus> StatusHistory { get; private set; } = new HashSet<TStatus>();


    protected void _ModifyStatus(string userName, int statusId)
    {
        _AddStatusHistoryRow(userName, statusId);

        this.LastStatusId = statusId;
        this.LastStatusCreatedBy = userName;
        this.LastStatusDateTime = DateTime.UtcNow;
    }

    protected abstract void _AddStatusHistoryRow(string userName, int statusId);

    public abstract IEntity<TId> AddNewStatus(string userName, int statusId);
}
