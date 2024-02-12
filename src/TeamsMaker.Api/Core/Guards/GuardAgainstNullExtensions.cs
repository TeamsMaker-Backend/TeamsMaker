using System.Runtime.CompilerServices;

using TeamsMaker.Api.Core.Guards.Exceptions;
using TeamsMaker.Api.Core.Guards.Interfaces;

namespace TeamsMaker.Api.Core.Guards;


public static partial class GuardAgainstNullExtensions
{
    public static T Null<T>(this IGuard guard,
        T input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null)
    {
        return input == null
            ? throw new GuardException(message ?? $"{parameterName} can't be null")
            : input;
    }
}
