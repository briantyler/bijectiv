namespace Bijectiv.KernelFactory
{
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class ExpressionSourceExpressionFactory : ISourceExpressionFactory<ExpressionSourceMemberShard>
    {
        public Expression Create(InjectionScaffold scaffold, MemberFragment fragment, ExpressionSourceMemberShard shard)
        {
            return new ParameterExpressionVisitor(
                shard.Expression.Parameters.Single(),
                Expression.Convert(scaffold.Source, shard.Source))
                .Visit(shard.Expression.Body);
        }
    }
}