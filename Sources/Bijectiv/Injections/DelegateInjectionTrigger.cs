namespace Bijectiv.Injections
{
    using System;

    using JetBrains.Annotations;

    public class DelegateInjectionTrigger : IInjectionTrigger
    {
        private readonly Action<IInjectionTriggerParameters> trigger;
 
        public virtual void Pull([NotNull] IInjectionTriggerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            this.trigger(parameters);
        }
    }
}