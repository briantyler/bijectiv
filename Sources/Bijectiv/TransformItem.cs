namespace Bijectiv
{
    using System;

    using JetBrains.Annotations;

    public class TransformItem
    {
        private readonly object source;

        public TransformItem([NotNull] object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
        }

        public object Source
        {
            get { return this.source; }
        }

        public object Target { get; set; }
    }
}