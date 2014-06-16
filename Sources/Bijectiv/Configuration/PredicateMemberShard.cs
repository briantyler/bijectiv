namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    public class PredicateMemberShard : MemberShard
    {
        public PredicateMemberShard([NotNull] Type source, [NotNull] Type target, [NotNull] MemberInfo member, Func<IInjectionParameters, bool> predicate)
            : base(source, target, member)
        {
            this.Predicate = predicate;
        }

        public override Guid ShardCategory
        {
            get { return LegendaryShards.Condition; }
        }

        public Func<IInjectionParameters, bool> Predicate { get; private set; }
    }
}