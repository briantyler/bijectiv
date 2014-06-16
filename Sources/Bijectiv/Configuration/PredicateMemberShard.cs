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
            [NotNull] Func<IInjectionParameters, bool> predicate)
            : base(source, target, member)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            this.Predicate = predicate;
        }

        public override Guid ShardCategory
        {
            get { return LegendaryShards.Condition; }
        }

        public Func<IInjectionParameters, bool> Predicate { get; private set; }
    }
}