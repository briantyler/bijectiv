namespace Bijectiv.Kernel
{
    using System;
    using System.Collections.Generic;

    public class InjectionResolutionStrategy : IInjectionResolutionStrategy
    {
        public TInjection Choose<TInjection>(Type source, Type target, IEnumerable<TInjection> candidates) 
            where TInjection : IInjection
        {
            throw new NotImplementedException();
        }
    }
}