namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    public class LabelCache : ILabelCache
    {
        private readonly Dictionary<Tuple<object, Guid>, LabelTarget> cache = 
            new Dictionary<Tuple<object, Guid>, LabelTarget>();

        public LabelTarget GetLabel(object scope, Guid category)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }

            var key = Tuple.Create(scope, category);
            if (!this.cache.ContainsKey(key))
            {
                this.CreateLabel(key);
            }

            return this.cache[key];
        }

        private void CreateLabel(Tuple<object, Guid> key)
        {
            var label = Expression.Label(string.Format("{0}_{1}_{2:D}", this.cache.Count(), key.Item1, key.Item2));

            this.cache.Add(key, label);
        }
    }
}