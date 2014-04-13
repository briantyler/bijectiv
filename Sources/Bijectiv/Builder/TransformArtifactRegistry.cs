namespace Bijectiv.Builder
{
    using System.Collections;
    using System.Collections.Generic;

    public class TransformArtifactRegistry : ITransformArtifactRegistry
    {
        private readonly List<TransformArtifact> artifacts = new List<TransformArtifact>();

        public IEnumerator<TransformArtifact> GetEnumerator()
        {
            return this.artifacts.GetEnumerator();
        }

        public void Add(TransformArtifact artifact)
        {
            this.artifacts.Add(artifact);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.artifacts).GetEnumerator();
        }
    }
}