namespace Bijectiv.KernelBuilder
{
    using System;

    using JetBrains.Annotations;

    public class InjectionTriggerFragment : InjectionFragment
    {
        private readonly InjectionTriggerSource source;

        private readonly IInjectionTrigger trigger;

        public InjectionTriggerFragment([NotNull] Type source, [NotNull] Type target)
            : base(source, target)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the fragment is inherited.
        /// </summary>
        public override bool Inherited
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the fragment category.
        /// </summary>
        public override Guid FragmentCategory
        {
            get { return LegendryFragments.Trigger; }
        }
    }
}