namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    public class CreateLabelSubTask<TFragment> : IInjectionSubTask<TFragment> where TFragment : InjectionFragment
    {
        private readonly Func<TFragment, string> nameFactory;

        public CreateLabelSubTask(Func<TFragment, string> nameFactory)
        {
            this.nameFactory = nameFactory;
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

            var label = Expression.Label(this.nameFactory(fragment));
            
            scaffold.Labels.Add(label);
            scaffold.Expressions.Add(Expression.Label(label));
        }
    }
}