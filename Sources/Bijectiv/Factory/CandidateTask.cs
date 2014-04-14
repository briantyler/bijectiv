namespace Bijectiv.Factory
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class CandidateTask : ITransformTask
    {
        public void Execute([NotNull] TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            scaffold.CandidateFragments.Clear();
            scaffold.CandidateFragments.AddRange(scaffold.Definition);

            var inherits = scaffold.Definition.OfType<InheritsFragment>().FirstOrDefault();
            while (inherits != null)
            {
                scaffold.ProcessedFragments.Add(inherits);

                var baseDefintion = scaffold.DefinitionRegistry
                    .Reverse()
                    .FirstOrDefault(
                        candidate =>
                            candidate.Source == inherits.SourceBase && candidate.Target == inherits.TargetBase);

                if (baseDefintion == null)
                {
                    break;
                }

                scaffold.CandidateFragments.AddRange(baseDefintion);
                inherits = baseDefintion.OfType<InheritsFragment>().FirstOrDefault();
            }
        }
    }
}