namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Linq;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class UninheritableFragmentFilterHammer : ITransformHammer
    {
        public void Strike([NotNull] TransformOre ore)
        {
            if (ore == null)
            {
                throw new ArgumentNullException("ore");
            }

            var inheritedFragments = ore.UnprocessedFragments
                .SkipWhile(candidate => 
                    candidate.Source == ore.Artifact.Source && candidate.Target == ore.Artifact.Target)
                .ToArray();

            var factoryFragments = inheritedFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.Factory);
            ore.ProcessedFragments.AddRange(factoryFragments);
        }
    }
}