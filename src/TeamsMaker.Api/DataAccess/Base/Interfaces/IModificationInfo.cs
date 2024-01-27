namespace TeamsMaker.Api.DataAccess.Base.Interfaces;

public interface IModificationInfo
{
    public string ModifiedBy { get; }
    public DateTime? LastModificationDate { get; }
}
