namespace Bijectiv.Factory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class ActivateTask : ITransformTask
    {
        public void Execute([NotNull] TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var factoryFragments = scaffold.UnprocessedFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.Factory)
                .ToArray();

            if (!(factoryFragments.FirstOrDefault() is ActivateFragment))
            {
                return;
            }

            scaffold.ProcessedFragments.AddRange(factoryFragments);

            var createTarget = Expression.New(scaffold.Definition.Target);
            var assignTarget = Expression.Assign(scaffold.Target, createTarget);

            var targetToObject = Expression.Convert(scaffold.Target, typeof(object));
            var assignTargetAsObject = Expression.Assign(scaffold.TargetAsObject, targetToObject);

            scaffold.Expressions.Add(assignTarget);
            scaffold.Expressions.Add(assignTargetAsObject);
        }
    }
}