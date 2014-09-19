namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class SourceMemberAssignSubtask<TShard> 
        : SingleInstanceShardCategorySubtask<TShard>
        where TShard : SourceMemberShard
    {

        protected SourceMemberAssignSubtask()
            : base(LegendaryShards.Source)
        {
        }

        public SourceMemberAssignSubtask([NotNull] ISourceExpressionFactory<TShard> sourceExpressionFactory)
            : this()
        {
            if (sourceExpressionFactory == null)
            {
                throw new ArgumentNullException("sourceExpressionFactory");
            }

            this.SourceExpressionFactory = sourceExpressionFactory;
        }

        public virtual ISourceExpressionFactory<TShard> SourceExpressionFactory { get; private set; } 

        public override void ProcessShard(
            [NotNull] InjectionScaffold scaffold, [NotNull] MemberFragment fragment, [NotNull] TShard shard)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            scaffold.Expressions.Add(
                Expression.Assign(
                    fragment.Member.GetAccessExpression(scaffold.Target),
                    Expression.Convert(this.SourceExpressionFactory.Create(scaffold, fragment, shard), fragment.Member.GetReturnType())));
        }

        protected internal override bool CanProcess(TShard shard)
        {
            return !shard.Inject;
        }
    }
}