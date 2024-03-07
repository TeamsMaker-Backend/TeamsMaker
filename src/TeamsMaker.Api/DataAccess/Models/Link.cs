using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Base;

namespace TeamsMaker.Api.DataAccess.Models;

public class Link : BaseEntity<int>
{
    public string Url { get; set; } = null!;
    public LinkTypesEnum Type { get; set; }

    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

