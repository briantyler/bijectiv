namespace Bijectiv.Utilities
{
    using System;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Stores;

    using JetBrains.Annotations;

    public class TargetFinderStoreInstanceFactory : IInstanceFactory
    {
        public Tuple<Type, object> Create([NotNull] IInstanceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var instance = new TargetFinderStore();
            registry.ResolveAll<TargetFinderRegistration>().ForEach(instance.Register);
            return new Tuple<Type, object>(typeof(ITargetFinderStore), instance);
        }
    }
}