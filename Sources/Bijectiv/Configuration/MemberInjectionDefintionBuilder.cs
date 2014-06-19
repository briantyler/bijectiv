namespace Bijectiv
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

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

        public IMemberInjectionDefintionBuilder<TSource, TTarget, TMember> Condidtion(
            Func<IInjectionParameters<TSource, TTarget>, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            this.fragment.Add(
                new PredicateConditionMemberShard(
                    typeof(TSource), 
                    typeof(TTarget), 
                    this.fragment.Member,
                    predicate));

            return this;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> Ignore()
        {
            this.Condidtion(p => false);
            return this.builder;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> InjectValue(object value)
        {
            this.fragment.Add(
                new ConstantSourceMemberShard(
                    typeof(TSource), 
                    typeof(TTarget), 
                    this.fragment.Member,
                    value));

            return this.builder;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> InjectSource<TResult>(
            Expression<Func<TSource, TResult>> expression)
        {
            this.fragment.Add(
                new ExpressionSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.fragment.Member,
                    expression));

            return this.builder;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> InjectParameters<TResult>(
            Func<IInjectionParameters<TSource, TTarget>, TResult> @delegate)
        {
            this.fragment.Add(
                new DelegateSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.fragment.Member,
                    @delegate,
                    false));

            return this.builder;
        }

        public IInjectionDefinitionBuilder<TSource, TTarget> AssignParameters(
            Func<IInjectionParameters<TSource, TTarget>, TMember> @delegate)
        {
            this.fragment.Add(
                new DelegateSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.fragment.Member,
                    @delegate,
                    true));

            return this.builder;
        }
    }
}