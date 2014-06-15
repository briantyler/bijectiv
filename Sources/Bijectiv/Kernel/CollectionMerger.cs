// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionMerger.cs" company="Bijectiv">
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
//   Defines the CollectionMerger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Utilities;

    /// <summary>
    /// Represents a collection merger that should be called via dynamic dispatch.
    /// </summary>
    /// <example>
    /// <code>
    ///     var result = merger.Merge((dynamic)source, (dynamic)target, context);
    /// </code>
    /// </example>
    public class CollectionMerger : ICollectionMerger
    {
        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="TTargetElement">
        /// The target element type.
        /// </typeparam>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="target">
        /// The target collection.
        /// </param>
        /// <param name="context">
        /// The context in which the merge will take place.
        /// </param>
        public void Merge<TTargetElement>(
            IEnumerable source, ICollection<TTargetElement> target, IInjectionContext context)
        {
            this.Merge((source ?? Enumerable.Empty<object>()).Cast<object>(), target, context);
        }

        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="TSourceElement">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TTargetElement">
        /// The target element type.
        /// </typeparam>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="target">
        /// The target collection.
        /// </param>
        /// <param name="context">
        /// The context in which the merge will take place.
        /// </param>
        public virtual void Merge<TSourceElement, TTargetElement>(
            [NotNull] IEnumerable<TSourceElement> source,
            [NotNull] ICollection<TTargetElement> target,
            [NotNull] IInjectionContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (typeof(TSourceElement).IsValueType && typeof(TTargetElement).IsValueType)
            {
                MergeValueTypeCollections(source, target, context);

                return;
            }

            var finder = context
                .InstanceRegistry
                .Resolve<ITargetFinderStore>()
                .Resolve(typeof(TSourceElement), typeof(TTargetElement));

            finder.Initialize(target, context);

            target.Clear();
            var index = 0;
            foreach (var sourceElement in source)
            {
                object targetElement;
                var hint = new EnumerableInjectionHint(index++);
                if (finder.TryFind(sourceElement, out targetElement))
                {
                    var merger =
                        context.InjectionStore.Resolve<IMerge>(
                            sourceElement == null ? typeof(TSourceElement) : sourceElement.GetType(),
                            targetElement == null ? typeof(TTargetElement) : targetElement.GetType());

                    var result = merger.Merge(sourceElement, targetElement, context, hint);
                    target.Add((TTargetElement)result.Target);
                }
                else
                {
                    var transformer =
                        context.InjectionStore.Resolve<ITransform>(
                            sourceElement == null ? typeof(TSourceElement) : sourceElement.GetType(),
                            typeof(TTargetElement));

                    target.Add((TTargetElement)transformer.Transform(sourceElement, context, hint));
                }
            }
        }

        /// <summary>
        /// Merges two collections of value type elements.
        /// </summary>
        /// <remarks>
        /// This can be done efficiently because value types cannot be merged and their runtime types are known
        /// at compile-time. Some efficiency could be gained where both types were sealed, but there is more 
        /// complexity for less gain.
        /// </remarks>
        /// <typeparam name="TSourceElement">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TTargetElement">
        /// The target element type.
        /// </typeparam>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="target">
        /// The target collection.
        /// </param>
        /// <param name="context">
        /// The context in which the merge will take place.
        /// </param>
        private static void MergeValueTypeCollections<TSourceElement, TTargetElement>(
            IEnumerable<TSourceElement> source,
            ICollection<TTargetElement> target,
            IInjectionContext context)
        {
            var transformer = context.InjectionStore.Resolve<ITransform>(
                typeof(TSourceElement), typeof(TTargetElement));

            target.Clear();
            var index = 0;
            foreach (var sourceElement in source)
            {
                var hint = new EnumerableInjectionHint(index++);
                target.Add((TTargetElement)transformer.Transform(sourceElement, context, hint));
            }
        }
    }
}