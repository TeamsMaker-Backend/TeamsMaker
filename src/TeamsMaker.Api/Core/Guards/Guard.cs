using TeamsMaker.Api.Core.Guards.Interfaces;

namespace TeamsMaker.Api.Core.Guards;

public class Guard : IGuard
{
    public static IGuard Against { get; } = new Guard();
    private Guard() { }
}
