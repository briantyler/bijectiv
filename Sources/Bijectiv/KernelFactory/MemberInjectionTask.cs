namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class MemberInjectionTask : IInjectionTask
    {
        private readonly IEnumerable<IInjectionSubTask<MemberFragment>> subTasks;

        public MemberInjectionTask([NotNull] IEnumerable<IInjectionSubTask<MemberFragment>> subTasks)
        {
            if (subTasks == null)
            {
                throw new ArgumentNullException("subTasks");
            }

            this.subTasks = subTasks;
        }

        public IEnumerable<IInjectionSubTask<MemberFragment>> SubTasks
        {
            get { return this.subTasks; }
        }

        public void Execute(InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var fragments = scaffold.UnprocessedFragments.OfType<MemberFragment>().ToArray();

            //// TODO: Ensure that duplicates are ignored.
            fragments
                .Where(candidate => scaffold.UnprocessedTargetMembers.Contains(candidate.Member))
                .ForEach(item => this.ProcessFragment(scaffold, item));

            scaffold.ProcessedFragments.AddRange(fragments);
        }

        public virtual void ProcessFragment([NotNull] InjectionScaffold scaffold, [NotNull] MemberFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            this.SubTasks.ForEach(item => item.Execute(scaffold, fragment));
            scaffold.ProcessedTargetMembers.Add(fragment.Member);
        }
    }
}