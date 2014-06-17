namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class MemberConditionSubTask : IInjectionSubTask<MemberFragment>
    {
        public void Execute(InjectionScaffold scaffold, MemberFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var shards = fragment.UnprocessedShards.OfType<PredicateMemberShard>().ToArray();

            var shard = shards.FirstOrDefault();
            if (shard != null)
            {
                this.ProcessShard(scaffold, fragment, shard);
            }

            fragment.ProcessedShards.AddRange(shards);
        }

        public virtual void ProcessShard(InjectionScaffold scaffold, MemberFragment fragment, PredicateMemberShard shard)
        {
            ///shard.Predicate
        }
    }
}