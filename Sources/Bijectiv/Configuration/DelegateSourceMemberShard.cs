namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    public class DelegateSourceMemberShard : MemberShard
    {
        private readonly object @delegate;

        private readonly bool isDirect;

        public DelegateSourceMemberShard(
            [NotNull] Type source,
            [NotNull] Type target, 
            [NotNull] MemberInfo member,
            [NotNull] object @delegate, 
            bool isDirect)
            : base(source, target, member)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            this.@delegate = @delegate;
            this.isDirect = isDirect;
        }

        public override Guid ShardCategory
        {
            get { return LegendaryShards.Source; }
        }

        public object Delegate
        {
            get { return this.@delegate; }
        }

        public bool IsDirect
        {
            get { return this.isDirect; }
        }
    }
}