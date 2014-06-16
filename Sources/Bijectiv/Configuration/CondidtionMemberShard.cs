namespace Bijectiv.Configuration
{
    using System;

    public class CondidtionMemberShard : MemberShard
    {
        public override Guid ShardCategory
        {
            get { return LegendaryShards.Condition; }
        }

        public Func<IInjectionParameters, bool> Predicate { get; private set; }
    }
}