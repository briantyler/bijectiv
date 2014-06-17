namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    public class CreateLabelSubTask<TFragment> : IInjectionSubTask<TFragment> where TFragment : InjectionFragment
    {
        private readonly string name;

        public CreateLabelSubTask([NotNull] string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.name = name;
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

            var label = Expression.Label(this.name);
            
            scaffold.Labels.Add(label);
            scaffold.Expressions.Add(Expression.Label(label));
        }
    }
}