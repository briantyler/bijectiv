namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    public class MemberValueSourceMergeSubtask : MemberSourceMergeSubtask<ValueSourceMemberShard>
    {
        protected override Expression GetMemberSource(ValueSourceMemberShard shard)
        {
            return Expression.Constant(shard.Value);
        }
    }
}