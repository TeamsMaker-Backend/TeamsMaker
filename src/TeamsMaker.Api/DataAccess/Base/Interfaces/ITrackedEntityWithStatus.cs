namespace TeamsMaker.Api.DataAccess.Base.Interfaces;

public interface ITrackedEntityWithStatus<TId, TStatus> : ITrackedEntity<TId>, ILastStatus<TStatus> where TStatus : IStatus
{
}
