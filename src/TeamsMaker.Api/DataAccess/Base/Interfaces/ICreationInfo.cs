namespace TeamsMaker.Api.DataAccess.Base.Interfaces;


public interface ICreationInfo
{
    public string CreatedBy { get; }
    public DateTime? CreationDate { get; }
}
