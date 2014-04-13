namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class TransformOre
    {
        private readonly List<Expression> expressions = new List<Expression>();
 
        private readonly List<TransformFragment> candidateFragments = new List<TransformFragment>();

        private readonly HashSet<TransformFragment> processedFragments = new HashSet<TransformFragment>();

        private readonly ITransformArtifactRegistry artifactRegistry;

        private readonly TransformArtifact artifact;

        private readonly Expression transformContext;

        private readonly Expression source;

        public TransformOre(ITransformArtifactRegistry artifactRegistry, TransformArtifact artifact, Expression source, Expression transformContext)
        {
            this.artifactRegistry = artifactRegistry;
            this.artifact = artifact;
            this.transformContext = transformContext;
            this.source = source;
        }

        public Expression Source
        {
            get { return this.source; }
        }

        public Expression TransformContext
        {
            get { return this.transformContext; }
        }

        public TransformArtifact Artifact 
        {
            get { return this.artifact; }
        }

        public ITransformArtifactRegistry ArtifactRegistry
        {
            get { return this.artifactRegistry; }
        }

        public IEnumerable<TransformFragment> UnprocessedFragments
        {
            get
            {
                return this.CandidateFragments
                    .Where(candidate => !this.ProcessedFragments.Contains(candidate))
                    .ToArray();
            }
        }

        public IList<TransformFragment> CandidateFragments 
        { 
            get { return this.candidateFragments; }
        }

        public ISet<TransformFragment> ProcessedFragments
        {
            get { return this.processedFragments; }
        }

        public IList<Expression> Expressions
        {
            get { return this.expressions; }
        }
    }
}