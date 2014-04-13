namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Linq;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class CandidateHammer : ITransformHammer
    {
        public void Strike([NotNull] TransformOre ore)
        {
            if (ore == null)
            {
                throw new ArgumentNullException("ore");
            }

            ore.CandidateFragments.Clear();
            ore.CandidateFragments.AddRange(ore.Artifact);

            var inherits = ore.Artifact.OfType<InheritsFragment>().FirstOrDefault();
            while (inherits != null)
            {
                ore.ProcessedFragments.Add(inherits);

                var baseArtifact = ore.ArtifactRegistry
                    .Reverse()
                    .FirstOrDefault(
                        candidate =>
                            candidate.Source == inherits.SourceBase && candidate.Target == inherits.TargetBase);

                if (baseArtifact == null)
                {
                    break;
                }

                ore.CandidateFragments.AddRange(baseArtifact);
                inherits = baseArtifact.OfType<InheritsFragment>().FirstOrDefault();
            }
        }
    }
}