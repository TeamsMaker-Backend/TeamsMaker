namespace TeamsMaker.Api.DataAccess.Base.Interfaces;

/// <summary>
///     A marker class contains the most basic unit of any entity which is the Id
/// </summary>
/// <typeparam name="TId">Entity Id, e.g int, long, Guid</typeparam>
public interface IEntity<TId>
{
    public TId Id { get; }
}
