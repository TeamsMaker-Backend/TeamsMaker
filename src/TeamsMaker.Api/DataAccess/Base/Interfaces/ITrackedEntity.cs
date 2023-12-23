namespace DataAccess.Base.Interfaces;

public interface ITrackedEntity<TId> : IEntity<TId>, ICreationInfo, IModificationInfo 
{
}
