namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class ActivateHammer : ITransformHammer
    {
        public void Strike([NotNull] TransformOre ore)
        {
            if (ore == null)
            {
                throw new ArgumentNullException("ore");
            }

            var factoryFragments = ore.UnprocessedFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.Factory)
                .ToArray();

            if (!(factoryFragments.FirstOrDefault() is ActivateFragment))
            {
                return;
            }

            ore.ProcessedFragments.AddRange(factoryFragments);

            var createTarget = Expression.Convert(Expression.New(ore.Artifact.Target), typeof(object));
            var assignTarget = Expression.Assign(ore.CurrentTargetExpression(), createTarget);
            ore.Expressions.Add(assignTarget);
        }
    }
}