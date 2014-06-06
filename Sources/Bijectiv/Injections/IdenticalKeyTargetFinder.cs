// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdenticalKeyTargetFinder.cs" company="Bijectiv">
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
//   Defines the IdenticalKeyTargetFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// A <see cref="ITargetFinder"/> that matches a source and target where the keys are identical 
    /// (same <see cref="Type"/> and value).
    /// </summary>
    public class IdenticalKeyTargetFinder : ITargetFinder
    {
        /// <summary>
        /// The cache of target values.
        /// </summary>
        private readonly IDictionary<object, object> targetCache;

        /// <summary>
        /// A delegate that selects the key from a source object.
        /// </summary>
        private readonly Func<object, object> sourceKeySelector;

        /// <summary>
        /// A delegate that selects the key from a target object.
        /// </summary>
        private readonly Func<object, object> targetKeySelector;

        /// <summary>
        /// Initialises a new instance of the <see cref="IdenticalKeyTargetFinder"/> class.
        /// </summary>
        /// <param name="sourceKeySelector">
        /// A delegate that selects the key from a source object.
        /// </param>
        /// <param name="targetKeySelector">
        /// A delegate that selects the key from a target object.
        /// </param>
        /// <param name="comparer">
        /// An <see cref="IEqualityComparer"/> that determines whether two keys are equal.
        /// </param>
        public IdenticalKeyTargetFinder(
            [NotNull] Func<object, object> sourceKeySelector,
            [NotNull] Func<object, object> targetKeySelector,
            [NotNull] IEqualityComparer<object> comparer)
        {
            if (sourceKeySelector == null)
            {
                throw new ArgumentNullException("sourceKeySelector");
            }

            if (targetKeySelector == null)
            {
                throw new ArgumentNullException("targetKeySelector");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            this.sourceKeySelector = sourceKeySelector;
            this.targetKeySelector = targetKeySelector;
            this.targetCache = new Dictionary<object, object>(comparer);
        }

        /// <summary>
        /// Gets a delegate that selects the key from a source object.
        /// </summary>
        public Func<object, object> SourceKeySelector
        {
            get { return this.sourceKeySelector; }
        }

        /// <summary>
        /// Gets a delegate that selects the key from a target object.
        /// </summary>
        public Func<object, object> TargetKeySelector
        {
            get { return this.targetKeySelector; }
        }

        /// <summary>
        /// Gets the cache of target values.
        /// </summary>
        public IDictionary<object, object> TargetCache
        {
            get { return this.targetCache; }
        }

        /// <summary>
        /// Initializes the finder for a given target collection.
        /// </summary>
        /// <param name="targets">
        /// The collection of target elements.
        /// </param>
        /// <param name="context">
        /// The context in which the targets are being found.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="targets"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="targets"/> contains duplicate keys.
        /// </exception>
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

                var key = this.TargetKeySelector(target);
                if (this.TargetCache.ContainsKey(key))
                {
                    var currentTarget = this.TargetCache[key];
                    throw new InvalidOperationException(
                        string.Format(
                        "An item with key '{0}' already exists: current '{1}', new '{2}'. The collection is not "
                        + "suitable as a merge target; to merge into a collection the key of every item in the "
                        + "collection must be unique.",
                        key,
                        currentTarget,
                        target));
                }

                this.TargetCache[key] = target;
            }
        }

        /// <summary>
        /// Tries to find the target element for <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source element.
        /// </param>
        /// <param name="target">
        /// The target element; this is only valid if the method returns <see langword="true" />.
        /// </param>
        /// <returns>
        /// A value indicating success.
        /// </returns>
        public bool TryFind(object source, out object target)
        {
            target = null;
            if (source == null)
            {
                return false;
            }

            var key = this.SourceKeySelector(source);
            if (!this.TargetCache.ContainsKey(key))
            {
                return false;
            }

            target = this.TargetCache[key];
            return true;
        }
    }
}