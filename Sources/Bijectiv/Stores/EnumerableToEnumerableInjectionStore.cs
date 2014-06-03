namespace Bijectiv.Stores
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Injections;
    using Bijectiv.Utilities;

    public class EnumerableToEnumerableInjectionStore : IInjectionStore
    {
        public TInjection Resolve<TInjection>(Type source, Type target) where TInjection : class, IInjection
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!typeof(IEnumerable).IsAssignableFrom(source))
            {
                return null;
            }

            var isEnumerableType = target
                .GetInterfaces()
                .Concat(target)
                .Where(candidate => candidate.IsGenericType)
                .Select(item => item.GetGenericTypeDefinition())
                .Contains(typeof(IEnumerable<>));

            if (isEnumerableType)
            {
                return new EnumerableToEnumerableInjection(source, target, new CollectionMerger()) as TInjection;
            }

            return null;
        }
    }
}