using DataAccess.Base.Interfaces;

namespace DataAccess.Base;

#nullable disable
/// <summary>
///     The smallest unit of Entity types, every entity should have a unique & non-null identifier
/// </summary>
/// <typeparam name="TId">Entity Id, e.g int, long, Guid</typeparam>
public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; }
}
