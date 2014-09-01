namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    public interface ISourceExpressionFactory<in TShard> 
        where TShard : SourceMemberShard
    {
        Expression Create(InjectionScaffold scaffold, MemberFragment fragment, TShard shard);
    }
}