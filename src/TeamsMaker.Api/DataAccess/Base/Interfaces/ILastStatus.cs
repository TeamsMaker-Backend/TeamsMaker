namespace DataAccess.Base.Interfaces;

/// <summary>
///     Interface for entities that traks last status,
///     it contains LastStatus properties
/// </summary>
/// <seealso cref="IStatus"/>
/// <typeparam name="TStatus">a Status Entity which is implementing an `IStatus` interface</typeparam>
public interface ILastStatus<TStatus> where TStatus : IStatus
{
    int LastStatusId { get; }
    string LastStatusCreatedBy { get; }
    DateTime? LastStatusDateTime { get; }
    ICollection<TStatus> StatusHistory { get; }
}
