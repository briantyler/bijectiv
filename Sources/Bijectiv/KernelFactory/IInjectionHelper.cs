namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    public interface IInjectionHelper
    {
        void AddTransformExpressionToScaffold(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberInfo member,
            [NotNull] Expression sourceExpression);

        void AddMergeExpressionToScaffold(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberInfo member,
            [NotNull] Expression sourceExpression);
    }
}