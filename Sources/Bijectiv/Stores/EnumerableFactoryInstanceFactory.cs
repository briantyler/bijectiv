namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Utilities;

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

            registry.ResolveAll<EnumerableRegistration>().ForEach(instance.Register);

            return new Tuple<Type, object>(typeof(IEnumerableFactory), instance);
        }
    }
}