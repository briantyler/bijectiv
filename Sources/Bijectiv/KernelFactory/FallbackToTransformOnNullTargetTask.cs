namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class FallbackToTransformOnNullTargetTask : IInjectionTask
    {
        public void Execute(InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var isTargetNull = ((Expression<Func<bool>>)(() => Placeholder.Of<object>("targetAsObject") == null)).Body;
            var transform = ((Expression<Func<object>>)(() => 
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore
                    .Resolve<ITransform>(
                        Placeholder.Of<object>("sourceAsObject") == null 
                            ? scaffold.Definition.Source 
                            : Placeholder.Of<object>("sourceAsObject").GetType(),
                        scaffold.Definition.Target)
                    .Transform(
                        Placeholder.Of<object>("sourceAsObject"),
                        Placeholder.Of<IInjectionContext>("context"), 
                        null))).Body;

            var postMergeAction = scaffold.GetVariable("PostMergeAction", typeof(PostMergeAction));
            var isTargetNullAction = Expression.Block(
                Expression.Assign(scaffold.TargetAsObject, transform),
                Expression.Assign(postMergeAction, Expression.Constant(PostMergeAction.Replace)),
                Expression.Goto(scaffold.GetLabel(null, LegendaryLabels.End)));

            Expression expression = Expression.IfThen(isTargetNull, isTargetNullAction);
            expression = new PlaceholderExpressionVisitor("targetAsObject", scaffold.TargetAsObject).Visit(expression);
            expression = new PlaceholderExpressionVisitor("sourceAsObject", scaffold.SourceAsObject).Visit(expression);
            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);

            scaffold.Expressions.Add(expression);
        }
    }
}