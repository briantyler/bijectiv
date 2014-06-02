namespace Bijectiv.Stores
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Builder;

    using JetBrains.Annotations;

    public class MasterInjectionStore : IInjectionStore, IInstanceRegistry
    {
        private readonly IInjectionStore slaveStore;

        private readonly IInstanceRegistry registry;

        public MasterInjectionStore([NotNull] IInjectionStore slaveStore, [NotNull] IInstanceRegistry registry)
        {
            if (slaveStore == null)
            {
                throw new ArgumentNullException("slaveStore");
            }

            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            this.slaveStore = slaveStore;
            this.registry = registry;
        }

        public TInjection Resolve<TInjection>(Type source, Type target) where TInjection : class, IInjection
        {
            return this.slaveStore.Resolve<TInjection>(source, target);
        }

        public void Register(Tuple<Type, object> registration)
        {
            this.registry.Register(registration);
        }

        public void Register(Type instanceType, object instance)
        {
            this.registry.Register(instanceType, instance);
        }

        public IEnumerable<TInstance> Resolve<TInstance>()
        {
            return this.Resolve<TInstance>();
        }
    }
}