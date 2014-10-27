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

    /// <summary>
    /// The default injection resolution strategy.
    /// </summary>
    public class InjectionResolutionStrategy : IInjectionResolutionStrategy
    {
        /// <summary>
        /// Chooses a <see cref="TInjection"/> from a collection of candidates.
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
        /// The chosen <see cref="TInjection"/>. Returns <see langword="null"/> if there is no suitable candidate.
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
                .OrderByDescending(grouping => grouping.Key, new HierarchicalTypeComparer())
                .FirstOrDefault();

            if (sourceCandidates == null)
            {
                return default(TInjection);
            }

            var targetCandidates = sourceCandidates
                .Where(candidate => target == candidate.item.Target || candidate.item.Target.IsSubclassOf(target))
                .Collect(item => item.item.Target, new HierarchicalTypeEqualityComparer())
                .Where(candidate => candidate.Equivalent(item => item.item.Target, new HierarchicalTypeEqualityComparer()))
                .OrderByDescending(grouping => grouping.Max(item => item.index))
                .FirstOrDefault();

            return targetCandidates == null
                ? default(TInjection)
                : targetCandidates
                    .OrderByDescending(item => item.item.Target, new HierarchicalTypeComparer())
                    .ThenByDescending(item => item.index)
                    .First()
                    .item;
        }

        private class HierarchicalTypeComparer : IComparer<Type>
        {
            public int Compare(Type x, Type y)
            {
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
        }

        private class HierarchicalTypeEqualityComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return x == y || x.IsSubclassOf(y) || y.IsSubclassOf(x);
            }

            public int GetHashCode(Type obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}