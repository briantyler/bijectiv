namespace Bijectiv.KernelBuilder
{
    using System;

    using JetBrains.Annotations;

    public class InjectionTriggerFragment : InjectionFragment
    {
        private readonly InjectionTriggerCause cause;

        private readonly IInjectionTrigger trigger;

        public InjectionTriggerFragment(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] IInjectionTrigger trigger,
            InjectionTriggerCause cause)
            : base(source, target)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.cause = cause;
            this.trigger = trigger;
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

        public InjectionTriggerCause Cause
        {
            get { return this.cause; }
        }

        public IInjectionTrigger Trigger
        {
            get { return this.trigger; }
        }
    }
}