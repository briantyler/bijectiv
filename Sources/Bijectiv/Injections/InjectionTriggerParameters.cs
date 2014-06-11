namespace Bijectiv.Injections
{
    public class InjectionTriggerParameters<TSource, TTarget> : IInjectionTriggerParameters<TSource, TTarget>
    {
        private readonly TSource source;

        private readonly TTarget target;

        private readonly IInjectionContext context;

        private readonly object hint;

        public InjectionTriggerParameters(TSource source, TTarget target, IInjectionContext context, object hint)
        {
            this.source = source;
            this.target = target;
            this.context = context;
            this.hint = hint;
        }

        public object SourceAsObject { get { return this.Source; } }

        public object TargetAsObject { get { return this.Target; } }

        public IInjectionContext Context
        {
            get { return this.context; }
        }

        public object Hint
        {
            get { return this.hint; }
        }

        public TSource Source
        {
            get { return this.source; }
        }

        public TTarget Target
        {
            get { return this.target; }
        }
    }
}