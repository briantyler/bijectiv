namespace Bijectiv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    public interface IInjectionResolutionStrategy
    {
        [Pure]
        TInjection Choose<TInjection>(
            [NotNull] Type source,
            [NotNull] Type target,
            [NotNull] IEnumerable<TInjection> candidates) 
            where TInjection : IInjection;
    }
}