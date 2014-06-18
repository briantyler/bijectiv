namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class MemberConditionSubTask : IInjectionSubTask<MemberFragment>
    {
        private readonly Func<MemberFragment, string> labelNameFactory;

        public MemberConditionSubTask(Func<MemberFragment, string> labelNameFactory)
        {
            this.labelNameFactory = labelNameFactory;
        }

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
            var labelName = this.labelNameFactory(fragment);
            var label = Expression.Label(labelName);
            scaffold.Labels.Add(label);
            var parameters = scaffold.Variables.First(candidate => candidate.Name == "injectionParameters");

            var delegateType = shard.Predicate.GetType();
            var method = delegateType.GetMethod("Invoke");

            var @delegate = Expression.Constant(shard.Predicate, delegateType);
            var negatePredicate = Expression.Not(Expression.Call(@delegate, method, parameters));

            scaffold.Expressions.Add(Expression.IfThen(negatePredicate, Expression.Goto(label)));
        }
    }
}