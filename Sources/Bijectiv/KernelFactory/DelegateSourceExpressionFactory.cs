namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    public class DelegateSourceExpressionFactory : ISourceExpressionFactory<DelegateSourceMemberShard>
    {
        public Expression Create(InjectionScaffold scaffold, MemberFragment fragment, DelegateSourceMemberShard shard)
        {
            var parameters = scaffold.Variables
                .Single(item => item.Name.Equals("injectionParameters", StringComparison.OrdinalIgnoreCase));

            return shard.Delegate.Method.IsStatic
                       ? Expression.Call(shard.Delegate.Method, Expression.Convert(parameters, shard.ParameterType))
                       : Expression.Call(
                           Expression.Constant(shard.Delegate.Target),
                           shard.Delegate.Method,
                           Expression.Convert(parameters, shard.ParameterType));
        }
    }
}