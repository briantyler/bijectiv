namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Injections;
    using Bijectiv.KernelBuilder;

    public class InitializeTriggerParametersTask : IInjectionTask
    {
        public void Execute(InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var hasTriggers = scaffold.UnprocessedFragments
                .Any(candidate => candidate.FragmentCategory == LegendryFragments.Trigger);

            if (!hasTriggers)
            {
                return;
            }

            var parametersType = typeof(InjectionTriggerParameters<,>)
                .MakeGenericType(scaffold.Definition.Source, scaffold.Definition.Target);

            var createParameters = Expression.New(
                parametersType.GetConstructors().Single(),
                scaffold.Source,
                scaffold.Target,
                scaffold.InjectionContext,
                scaffold.Hint);

            var downCastParameters = Expression.Convert(createParameters, typeof(IInjectionTriggerParameters));
            var parametersVariable = Expression.Variable(typeof(IInjectionTriggerParameters), "triggerParameters");

            scaffold.Variables.Add(parametersVariable);
            scaffold.Expressions.Add(Expression.Assign(parametersVariable, downCastParameters));
        }
    }
}