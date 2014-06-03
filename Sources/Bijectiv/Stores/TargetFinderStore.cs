namespace Bijectiv.Stores
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Builder;
    using Bijectiv.Injections;

    using JetBrains.Annotations;

    public class TargetFinderStore : ITargetFinderStore
    {
        private readonly IDictionary<Tuple<Type, Type>, Func<ITargetFinder>> stores =
            new Dictionary<Tuple<Type, Type>, Func<ITargetFinder>>();

        public void Register([NotNull] TargetFinderRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            var key = Tuple.Create(registration.SourceElement, registration.TargetElement);
            this.stores[key] = registration.TargetFinderFactory;
        }

        public ITargetFinder Resolve(Type sourceElement, Type targetElement)
        {
            var key = Tuple.Create(sourceElement, targetElement);
            
            Func<ITargetFinder> factory;
            return !this.stores.TryGetValue(key, out factory) ? new NullTargetFinder() : factory();
        }
    }
}