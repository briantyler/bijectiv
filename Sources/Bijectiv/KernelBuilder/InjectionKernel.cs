namespace Bijectiv.KernelBuilder
{
    using System;

    using JetBrains.Annotations;

    public class InjectionKernel : IInjectionKernel
    {
        private readonly IInjectionStore store;

        private readonly IInstanceRegistry registry;

        public InjectionKernel([NotNull] IInjectionStore store, [NotNull] IInstanceRegistry registry)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            this.store = store;
            this.registry = registry;
        }

        public IInjectionStore Store
        {
            get { return this.store; }
        }

        public IInstanceRegistry Registry
        {
            get { return this.registry; }
        }
    }
}