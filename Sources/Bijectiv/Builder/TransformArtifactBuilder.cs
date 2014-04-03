namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    public class TransformArtifactBuilder<TSource, TTarget> : ITransformArtifactBuilder<TSource, TTarget>
    {
        private readonly TransformArtifact artifact;

        public TransformArtifactBuilder([NotNull] TransformArtifact artifact)
        {
            if (artifact == null)
            {
                throw new ArgumentNullException("artifact");
            }

            this.artifact = artifact;
        }
    }
}