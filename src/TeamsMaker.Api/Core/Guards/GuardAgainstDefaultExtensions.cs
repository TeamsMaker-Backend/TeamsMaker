using System.Runtime.CompilerServices;
using TeamsMaker.Api.Core.Guards.Exceptions;
using TeamsMaker.Api.Core.Guards.Interfaces;

namespace TeamsMaker.Api;

public static partial class GuardAgainstDefaultExtensions
{
    public static T Default<T>(this IGuard guard, 
        T input, 
        [CallerArgumentExpression("input")] string? parameterName = null, 
        string? message = null)
    {
        return EqualityComparer<T>.Default.Equals(input, default)
            ? throw new GuardException(message ?? $"{parameterName} can't be {default(T)}")
            : input;
    }
}
