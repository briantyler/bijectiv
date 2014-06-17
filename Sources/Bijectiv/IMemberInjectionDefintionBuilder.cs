namespace Bijectiv
{
    using System;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    public interface IMemberInjectionDefintionBuilder<TSource, TTarget, TMember>
    {
        IMemberInjectionDefintionBuilder<TSource, TTarget, TMember> Condidtion(
            [NotNull] Func<IInjectionParameters<TSource, TTarget>, bool> predicate);

        IInjectionDefinitionBuilder<TSource, TTarget> Ignore();

        IInjectionDefinitionBuilder<TSource, TTarget> InjectValue(object value);

        IInjectionDefinitionBuilder<TSource, TTarget> InjectSource<TResult>(
            Expression<Func<TSource, TResult>> expression);

        IInjectionDefinitionBuilder<TSource, TTarget> InjectParameters<TResult>(
            Func<IInjectionParameters<TSource, TTarget>, TResult> @delegate);

        IInjectionDefinitionBuilder<TSource, TTarget> AssignParameters(
            Func<IInjectionParameters<TSource, TTarget>, TMember> @delegate);
    }
}