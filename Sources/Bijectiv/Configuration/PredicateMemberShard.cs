namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    public class PredicateMemberShard : MemberShard
    {
        public PredicateMemberShard(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] MemberInfo member,
            [NotNull] object predicate)
            : base(source, target, member)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            //// TODO: assert predicate has the correct signature (either weak or strong).
            
            this.Predicate = predicate;
        }

        public override Guid ShardCategory
        {
            get { return LegendaryShards.Condition; }
        }

        public object Predicate { get; private set; }
    }
}