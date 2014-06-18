namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
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

            var labelName = this.nameFactory(fragment);
            
            // TODO: this is very wrong, but I want to get an end-to-end transform.
            var label = scaffold.Labels.FirstOrDefault(candidate => candidate.Name == labelName);

            if (label == null)
            {
                return;
            }

            scaffold.Expressions.Add(Expression.Label(label));
        }
    }
}