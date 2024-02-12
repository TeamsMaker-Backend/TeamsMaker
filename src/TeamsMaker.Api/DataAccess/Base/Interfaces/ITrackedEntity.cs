namespace TeamsMaker.Api.DataAccess.Base.Interfaces;

public interface ITrackedEntity<TId> : IEntity<TId>, ICreationInfo, IModificationInfo
{
}
