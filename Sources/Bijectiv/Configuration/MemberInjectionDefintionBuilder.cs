namespace Bijectiv
{
    using System;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class MemberInjectionDefintionBuilder<TSource, TTarget, TMember> 
        : IMemberInjectionDefintionBuilder<TSource, TTarget, TMember>
    {
        private readonly InjectionDefinitionBuilder<TSource, TTarget> builder;

        private readonly MemberFragment fragment;

        public MemberInjectionDefintionBuilder(
            [NotNull] InjectionDefinitionBuilder<TSource, TTarget> builder,
            [NotNull] MemberFragment fragment)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            this.builder = builder;
            this.fragment = fragment;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> Ignore()
        {
            this.fragment.Add(new CondidtionMemberShard());
            return this.builder;
        }
    }
}