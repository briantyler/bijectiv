namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using JetBrains.Annotations;

    public static class CollectionExtensions
    {
        public static void AddRange<T>([NotNull] this ICollection<T> @this, [NotNull] IEnumerable<T> collection)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var item in collection)
            {
                @this.Add(item);
            }
        }
    }
}