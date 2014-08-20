namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A subtask that transforms a source shard into a member.
    /// </summary>
    public abstract class MemberSourceTransformSubtask<TShard>
        : SingleInstanceShardCategorySubtask<TShard>
        where TShard : MemberShard
    {
        protected MemberSourceTransformSubtask()
            : base(LegendaryShards.Source)
        {
        }

        /// <summary>
        /// Processes a shard.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <param name="shard">
        /// The shard to process.
        /// </param>
        public override void ProcessShard(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberFragment fragment,
            [NotNull] TShard shard)
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

            var targetMemberSource = this.GetMemberSource(scaffold, shard);

            this.AddTransformExpressionToScaffold(scaffold, fragment, targetMemberSource);
        }

        protected abstract Expression GetMemberSource(InjectionScaffold scaffold, TShard shard);

        protected internal virtual void AddTransformExpressionToScaffold(
            InjectionScaffold scaffold,
            MemberFragment fragment,
            Expression targetMemberSource)
        {
            var targetMemberType = fragment.Member.GetReturnType();

            var memberSource = Expression.Variable(typeof(object));
            var assignMemberSource = Expression.Assign(
                memberSource, Expression.Convert(targetMemberSource, typeof(object)));

            var transform = ((Expression<Action>) (() =>
                 Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(
                        Placeholder.Of<object>("memberSource").GetType(), 
                        targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("memberSource"),
                        Placeholder.Of<IInjectionContext>("context"),
                        null))).Body;

            transform = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(transform);
            transform = new PlaceholderExpressionVisitor("memberSource", memberSource).Visit(transform);

            var accessExpression = fragment.Member.GetAccessExpression(scaffold.Target);
            transform = Expression.Assign(accessExpression, Expression.Convert(transform, targetMemberType));

            scaffold.Expressions.Add(
                Expression.Block(new[] { memberSource }, assignMemberSource, transform));
        }
    }
}