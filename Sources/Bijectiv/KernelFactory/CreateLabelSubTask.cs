namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    public class CreateLabelSubTask<TFragment> : IInjectionSubTask<TFragment> where TFragment : InjectionFragment
    {
        private readonly Func<TFragment, string> labelNameFactory;

        public CreateLabelSubTask(Func<TFragment, string> labelNameFactory)
        {
            this.labelNameFactory = labelNameFactory;
        }

        public void Execute(InjectionScaffold scaffold, TFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var label = scaffold.GetOrAddLabel(this.labelNameFactory(fragment));
            scaffold.Expressions.Add(Expression.Label(label));
        }
    }
}