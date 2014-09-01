namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    public class ValueSourceExpressionFactory : ISourceExpressionFactory<ValueSourceMemberShard>
    {
        public Expression Create(InjectionScaffold scaffold, MemberFragment fragment, ValueSourceMemberShard shard)
        {
            return Expression.Constant(shard.Value);
        }
    }
}