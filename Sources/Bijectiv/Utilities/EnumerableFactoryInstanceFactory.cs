namespace Bijectiv.Utilities
{
    using System;

    using Bijectiv.Stores;

    using JetBrains.Annotations;

    public class EnumerableFactoryInstanceFactory : IInstanceFactory
    {
        public Tuple<Type, object> Create([NotNull] IInstanceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var instance = new EnumerableFactory();

            registry.ResolveAll<EnumerableFactoryRegistration>().ForEach(instance.Register);

            return new Tuple<Type, object>(typeof(IEnumerableFactory), instance);
        }
    }
}