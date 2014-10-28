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

        /// <summary>
        /// Collects the group of equivalent elements for each element in <paramref name="source"/>; where equivalence
        /// is determined by the equivalence of the element keys.
        /// </summary>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="keySelector">
        /// A delegate that selects the key.
        /// </param>
        /// <typeparam name="TSource">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The key type.
        /// </typeparam>
        /// <returns>
        /// A collection consisting of a group of equivalent elements for each element in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static IEnumerable<IGrouping<TKey, TSource>> Collect<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector)
        {
            return source.Collect(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Collects the group of equivalent elements for each element in <paramref name="source"/>; where equivalence
        /// is determined by the <paramref name="comparer"/> action on the element keys.
        /// </summary>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="keySelector">
        /// A delegate that selects the key.
        /// </param>
        /// <param name="comparer">
        /// The comparer that determines equivalence.
        /// </param>
        /// <typeparam name="TSource">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The key type.
        /// </typeparam>
        /// <returns>
        /// A collection consisting of a group of equivalent elements for each element in <paramref name="source"/>; 
        /// where equivalence is determined by the <paramref name="comparer"/> action on the element keys.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static IEnumerable<IGrouping<TKey, TSource>> Collect<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            var enumerable = source as IList<TSource> ?? source.ToArray();
            return enumerable.Select(
                    item =>
                    new Grouping<TKey, TSource>(
                        keySelector(item),
                        enumerable.Where(candidate => comparer.Equals(keySelector(item), keySelector(candidate)))));
        }

        /// <summary>
        /// Determines whether all the keys in <paramref name="source"/> are equivalent.
        /// </summary>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="keySelector">
        /// A delegate that selects the key.
        /// </param>
        /// <typeparam name="TSource">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The key type.
        /// </typeparam>
        /// <returns>
        /// A value indicating equivalence of the keys in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static bool Equivalent<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector)
        {
            return source.Equivalent(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Determines whether all the keys in <paramref name="source"/> are equivalent with respect to the action of
        /// <paramref name="comparer"/>.
        /// </summary>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="keySelector">
        /// A delegate that selects the key.
        /// </param>
        /// <param name="comparer">
        /// The comparer that determines equivalence.
        /// </param>
        /// <typeparam name="TSource">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The key type.
        /// </typeparam>
        /// <returns>
        /// A value indicating equivalence of the keys in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when Any parameter is null.
        /// </exception>
        public static bool Equivalent<TSource, TKey>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            var collection = source.Select(keySelector).ToArray();
            return collection.All(element => collection.All(candidate => comparer.Equals(element, candidate)));
        }

        /// <summary>
        /// A collection of objects that have a common key.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of the key of the <see cref="Grouping{TKey,TElement}" />.
        /// </typeparam>
        /// <typeparam name="TSource">
        /// The type of the values in the <see cref="Grouping{TKey,TElement}" />.
        /// </typeparam>
        [ExcludeFromCodeCoverage]
        private class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
        {
            /// <summary>
            /// The underlying collection.
            /// </summary>
            private readonly IEnumerable<TSource> collection;

            /// <summary>
            /// Initialises a new instance of the <see cref="Grouping{TKey,TSource}"/> class.
            /// </summary>
            /// <param name="key">
            /// The key.
            /// </param>
            /// <param name="collection">
            /// The collection.
            /// </param>
            public Grouping(TKey key, IEnumerable<TSource> collection)
            {
                this.collection = collection;
                this.Key = key;
            }

            /// <summary>
            /// Gets the key of the <see cref="Grouping{TKey,TSource}"/> element.
            /// </summary>
            public TKey Key { get; private set; }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<TSource> GetEnumerator()
            {
                return this.collection.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator"/> that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}