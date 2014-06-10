namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class CreateTriggersTask : IInjectionTask
    {
        private readonly InjectionTriggerCause cause;

        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var fragments = scaffold
                .UnprocessedFragments
                .OfType<InjectionTriggerFragment>()
                .Where(candidate => candidate.Cause == this.cause)
                .ToArray();

            foreach (var fragment in fragments)
            {
                this.ProcessFragment(fragment, scaffold);
            }

            scaffold.ProcessedFragments.AddRange(fragments);
        }

        public virtual void ProcessFragment(
            [NotNull] InjectionTriggerFragment fragment,
            [NotNull] InjectionScaffold scaffold)
        {
            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            
        }
    }
}