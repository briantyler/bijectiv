namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    public class CreateLabelSubtask<TFragment> : IInjectionSubtask<TFragment> where TFragment : InjectionFragment
    {
        private readonly Guid category;

        public CreateLabelSubtask(Guid category)
        {
            this.category = category;
        }

        public Guid Category
        {
            get { return this.category; }
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

            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(fragment, this.Category)));
        }
    }
}