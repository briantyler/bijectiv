namespace Bijectiv.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    public class IdenticalKeyTargetFinder : ITargetFinder
    {
        private readonly IDictionary<object, object> targetCache;

        private readonly Func<object, object> sourceKeyLocator;

        private readonly Func<object, object> targetKeyLocator;

        public IdenticalKeyTargetFinder(
            Func<object, object> sourceKeyLocator, 
            Func<object, object> targetKeyLocator,
            IEqualityComparer<object> comparer)
        {
            this.sourceKeyLocator = sourceKeyLocator;
            this.targetKeyLocator = targetKeyLocator;
            this.targetCache = new Dictionary<object, object>(comparer);
        }

        public Func<object, object> SourceKeyLocator
        {
            get { return this.sourceKeyLocator; }
        }

        public Func<object, object> TargetKeyLocator
        {
            get { return this.targetKeyLocator; }
        }

        public void Initialize([NotNull] IEnumerable targets, IInjectionContext context)
        {
            if (targets == null)
            {
                throw new ArgumentNullException("targets");
            }

            foreach (var target in targets)
            {
                if (target == null)
                {
                    continue;
                }

                var key = this.TargetKeyLocator(target);
                this.targetCache[key] = target;
            }
        }

        public bool TryFind(object source, out object target)
        {
            target = null;
            if (source == null)
            {
                return false;
            }

            var key = this.SourceKeyLocator(source);
            if (!this.targetCache.ContainsKey(key))
            {
                return false;
            }

            target = this.targetCache[key];
            return true;
        }
    }
}