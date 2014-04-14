namespace Bijectiv.Factory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    public class InitializeTask : ITransformTask
    {
        public void Execute([NotNull] TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            scaffold.Variables.Add(Expression.Variable(scaffold.Definition.Source, "source"));
            scaffold.Source = scaffold.Variables.Last();

            scaffold.Variables.Add(Expression.Variable(scaffold.Definition.Target, "target"));
            scaffold.Target = scaffold.Variables.Last();

            scaffold.Variables.Add(Expression.Variable(typeof(object), "targetAsObject"));
            scaffold.TargetAsObject = scaffold.Variables.Last();

            var sourceToType = Expression.Convert(scaffold.SourceAsObject, scaffold.Definition.Source);
            var assignSource = Expression.Assign(scaffold.Source, sourceToType);

            scaffold.Expressions.Add(assignSource);
        }
    }
}