namespace Bijectiv.Factory
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class UninheritableFragmentFilterTask : ITransformTask
    {
        public void Execute([NotNull] TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var inheritedFragments = scaffold.UnprocessedFragments
                .SkipWhile(candidate => 
                    candidate.Source == scaffold.Definition.Source && candidate.Target == scaffold.Definition.Target)
                .ToArray();

            var factoryFragments = inheritedFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.Factory);
            scaffold.ProcessedFragments.AddRange(factoryFragments);
        }
    }
}