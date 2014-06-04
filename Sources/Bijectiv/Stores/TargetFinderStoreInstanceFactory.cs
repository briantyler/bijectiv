namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Utilities;

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