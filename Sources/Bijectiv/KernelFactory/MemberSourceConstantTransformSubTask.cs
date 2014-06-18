namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class MemberSourceConstantTransformSubTask : IInjectionSubTask<MemberFragment>
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

            var shard = fragment.UnprocessedShards.OfType<ConstantSourceMemberShard>().FirstOrDefault();
            if (shard == null)
            {
                return;
            }

            var transform = CreateExpressionTemplate(shard.Value, fragment.Member.GetReturnType()).Body;
            transform = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(transform);

            Expression.Assign(
                fragment.Member.GetAccessExpression(scaffold.Target),
                Expression.Convert(transform, fragment.Member.GetReturnType()));
        }

        private static Expression<Action> CreateExpressionTemplate(object value, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(value.GetType(), targetMemberType)
                    .Transform(value, Placeholder.Of<IInjectionContext>("context"), null);
        }


    }
}