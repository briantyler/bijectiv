// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Bijectiv">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Brian Tyler
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//   THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the EnumerableExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents extensions to types that implement <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates two sequences, where the second sequence is constructed from a collection of arguments.
        /// </summary>
        /// <param name="this">
        /// The first sequence.
        /// </param>
        /// <param name="args">
        /// The collection of arguments to concatenate with the first.
        /// </param>
        /// <typeparam name="T">
        /// The type of items in the collection.
        /// </typeparam>
        /// <returns>
        /// The concatenated sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="this"/> is null.
        /// </exception>
        public static IEnumerable<T> Concat<T>([NotNull] this IEnumerable<T> @this, params T[] args)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return Enumerable.Concat(@this, args);
        }

        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="this">
        /// The collection.
        /// </param>
        /// <param name="action">
        /// The action to perform.
        /// </param>
        /// <typeparam name="T">
        /// The type of items in the collection.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any parameter is null.
        /// </exception>
        public static void ForEach<T>(
            [NotNull, InstantHandle] this IEnumerable<T> @this, 
            [NotNull, InstantHandle] Action<T> action)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
           
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in @this)
            {
                action(item);
            }
        }

        public static IEnumerable<IGrouping<TKey, TSource>> Collect<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer = null)
        {        
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var enumerable = source as IList<TSource> ?? source.ToArray();
            return enumerable.Select(
                    item =>
                    new Grouping<TKey, TSource>(
                        keySelector(item),
                        enumerable.Where(candidate => comparer.Equals(keySelector(item), keySelector(candidate)))));
        }

        public static bool Equivalent<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("collection");
            }

            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var collection = source as IList<TSource> ?? source.ToArray();
            return collection.All(element => collection.All(candidate => comparer.Equals(keySelector(element), keySelector(candidate))));
        }

        private class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
        {
            private readonly IEnumerable<TSource> collection;

            public Grouping(TKey key, IEnumerable<TSource> collection)
            {
                this.collection = collection;
                this.Key = key;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                return this.collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public TKey Key { get; private set; }
        }
    }
}