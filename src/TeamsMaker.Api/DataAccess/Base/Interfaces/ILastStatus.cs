namespace TeamsMaker.Api.DataAccess.Base.Interfaces;

/// <summary>
///     Interface for entities that traks last status,
///     it contains LastStatus properties
/// </summary>
/// <seealso cref="IStatus"/>
/// <typeparam name="TStatus">a Status Entity which is implementing an `IStatus` interface</typeparam>
public interface ILastStatus<TStatus> where TStatus : IStatus
{
    public int LastStatusId { get; }
    public string LastStatusCreatedBy { get; }
    public DateTime? LastStatusDateTime { get; }
    public ICollection<TStatus> StatusHistory { get; }
}
