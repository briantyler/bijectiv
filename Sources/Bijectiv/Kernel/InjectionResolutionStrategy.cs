// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionResolutionStrategy.cs" company="Bijectiv">
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
//   Defines the InjectionResolutionStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The default injection resolution strategy.
    /// </summary>
    public class InjectionResolutionStrategy : IInjectionResolutionStrategy
    {
        /// <summary>
        /// Chooses a <typeparamref name="TInjection"/> from a collection of candidates.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="candidates">
        /// The candidate injections.
        /// </param>
        /// <typeparam name="TInjection">
        /// The type of the injection to choose.
        /// </typeparam>
        /// <returns>
        /// The chosen <typeparamref name="TInjection"/>. Returns <see langword="null"/> if there is no suitable candidate.
        /// </returns>
        public TInjection Choose<TInjection>(Type source, Type target, IEnumerable<TInjection> candidates) 
            where TInjection : IInjection
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (candidates == null)
            {
                throw new ArgumentNullException("candidates");
            }

            var sourceCandidates = candidates
                .Select((item, index) => new { index, item })
                .Where(candidate => source == candidate.item.Source || source.IsSubclassOf(candidate.item.Source))
                .GroupBy(item => item.item.Source)
                .OrderByDescending(grouping => grouping.Key, InheritanceComparer.Instance)
                .FirstOrDefault();

            if (sourceCandidates == null)
            {
                return default(TInjection);
            }

            var targetCandidates = sourceCandidates
                .Where(candidate => 
                    (!(candidate.item is IMerge)
                    && (target == candidate.item.Target || candidate.item.Target.IsSubclassOf(target)))
                    ||
                    (candidate.item is IMerge
                    && (target == candidate.item.Target || target.IsSubclassOf(candidate.item.Target))))
                .Collect(item => item.item.Target, InheritanceComparer.Instance)
                .Where(candidate => candidate.Equivalent(item => item.item.Target, InheritanceComparer.Instance))
                .OrderByDescending(grouping => grouping.Max(item => item.index))
                .FirstOrDefault();

            return targetCandidates == null
                ? default(TInjection)
                : targetCandidates
                    .OrderByDescending(item => item.item.Target, InheritanceComparer.Instance)
                    .ThenByDescending(item => item.index)
                    .First()
                    .item;
        }

        /// <summary>
        /// Represents <see cref="Type"/> comparison operation that uses type inheritance for comparison rules.
        /// <see cref="Type"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private class InheritanceComparer : IComparer<Type>, IEqualityComparer<Type>
        {
            /// <summary>
            /// The instance.
            /// </summary>
            public static readonly InheritanceComparer Instance = new InheritanceComparer();

            /// <summary>
            /// Prevents a default instance of the <see cref="InheritanceComparer"/> class from being created.
            /// </summary>
            private InheritanceComparer()
            {
            }

            /// <summary>
            /// Compares two <see cref="Type"/> instances and returns a value indicating how one is derived from
            /// the other.
            /// </summary>
            /// <param name="x">
            /// The first <see cref="Type"/> to compare.
            /// </param>
            /// <param name="y">
            /// The second <see cref="Type"/> to compare.
            /// </param>
            /// <returns>
            /// A signed integer that is: negative when <paramref name="x"/> is a base class of <paramref name="y"/>;
            /// positive when <paramref name="y"/> is a base class of <paramref name="x"/> and; zero when the types
            /// are identical.
            /// </returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown when <paramref name="x"/> and <paramref name="y"/> are in separate inheritance hierarchies.
            /// </exception>
            public int Compare([NotNull] Type x, [NotNull] Type y)
            {
                if (x == null)
                {
                    throw new ArgumentNullException("x");
                }

                if (y == null)
                {
                    throw new ArgumentNullException("y");
                }

                if (x == y)
                {
                    return 0;
                }
                
                if (x.IsSubclassOf(y))
                {
                    return 1;
                }
                
                if (y.IsSubclassOf(x))
                {
                    return -1;
                }

                throw new InvalidOperationException(
                    string.Format("Types x='{0}' and y='{1}' are not comparable.", x, y));
            }

            /// <summary>
            /// Determines whether the specified <see cref="Type"/> instances are equal up to inheritance.
            /// </summary>
            /// <param name="x">
            /// The first <see cref="Type"/> to compare.
            /// </param>
            /// <param name="y">
            /// The second <see cref="Type"/> to compare.
            /// </param>
            /// <returns>
            /// A value indicating whether the specified <see cref="Type"/> instances are equal up to inheritance.
            /// </returns>
            public bool Equals([NotNull] Type x, [NotNull] Type y)
            {
                if (x == null)
                {
                    throw new ArgumentNullException("x");
                }

                if (y == null)
                {
                    throw new ArgumentNullException("y");
                }

                return x == y || x.IsSubclassOf(y) || y.IsSubclassOf(x);
            }

            /// <summary>
            /// Returns a hash code for the specified <see cref="Type"/>.
            /// </summary>
            /// <param name="obj">
            /// The <see cref="Type"/> for which the hash code is to be returned.
            /// </param>
            /// <returns>
            /// A hash code for the specified <see cref="Type"/>.
            /// </returns>
            public int GetHashCode(Type obj)
            {
                // Since everything is equivalent to object then there can be only one hash code.
                return typeof(object).GetHashCode();
            }
        }
    }
}