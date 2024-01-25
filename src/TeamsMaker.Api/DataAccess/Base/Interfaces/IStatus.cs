namespace DataAccess.Base.Interfaces;

/// <summary>
///     this Interface extends `IEntity` with an `Id` of type `long`.
///     it contains adminstrative data along with the status column
///     this should be used with Status entities -an entity that contains a status log of another entity-
/// </summary>
/// <seealso cref="ICreationInfo"/>
/// <seealso cref="IModificationInfo"/>
/// <seealso cref="IEntity<T>"/>
public interface IStatus : IEntity<long>, ICreationInfo
{
    public int Status { get; }
}
