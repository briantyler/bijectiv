namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class MemberValueSourceTransformSubTask : IInjectionSubTask<MemberFragment>
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

            var shard = fragment.UnprocessedShards.OfType<ValueSourceMemberShard>().FirstOrDefault();
            if (shard == null)
            {
                return;
            }

            var expression = CreateExpressionTemplate(shard.Value, fragment.Member.GetReturnType()).Body;
            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);
            expression = Expression.Assign(
                fragment.Member.GetAccessExpression(scaffold.Target),
                Expression.Convert(expression, fragment.Member.GetReturnType()));

            scaffold.Expressions.Add(expression);
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