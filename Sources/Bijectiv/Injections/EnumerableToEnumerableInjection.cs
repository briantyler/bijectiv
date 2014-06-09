// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToEnumerableInjection.cs" company="Bijectiv">
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
//   Defines the EnumerableToEnumerableInjection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;
    using System.Collections;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="IInjection"/> that injects an <see cref="IEnumerable"/> instance into an 
    /// <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableToEnumerableInjection : ITransform, IMerge
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EnumerableToEnumerableInjection"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="merger">
        /// The merger that merges items from one collection into another collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public EnumerableToEnumerableInjection(
            [NotNull] Type source, 
            [NotNull] Type target,
            [NotNull] ICollectionMerger merger)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (merger == null)
            {
                throw new ArgumentNullException("merger");
            }

            this.Source = source;
            this.Target = target;
            this.Merger = merger;
        }

        /// <summary>
        /// Gets the source type supported by the injection.
        /// </summary>
        public Type Source { get; private set; }

        /// <summary>
        /// Gets the target type supported by the injection.
        /// </summary>
        public Type Target { get; private set; }

        /// <summary>
        /// Gets the merger that merges items from one collection into another collection..
        /// </summary>
        public ICollectionMerger Merger { get; private set; }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <see cref="IInjection.Target"/>;  
        /// using the transformation rules defined by <see cref="IInjection.Source"/> --&gt; 
        /// <see cref="IInjection.Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the injection will take place.
        /// </param>
        /// <param name="hint">
        /// A hint that can be used to pass additional information to the injection.
        /// </param>
        /// <returns>
        /// The newly created target instance.
        /// </returns>
        public object Transform(object source, [NotNull] IInjectionContext context, object hint)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var enumerableFactory = context.InstanceRegistry.Resolve<IEnumerableFactory>();
            return this.Merge(source, enumerableFactory.Resolve(this.Target), context, null).Target;
        }

        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>; using the transformation rules 
        /// defined by <see cref="IInjection.Source"/> --&gt; <see cref="IInjection.Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="target">
        /// The target object.
        /// </param>
        /// <param name="context">
        /// The context in which the injection will take place.
        /// </param>
        /// <param name="hint">
        /// A hint that can be used to pass additional information to the injection.
        /// </param>
        /// <returns>
        /// A <see cref="IMergeResult"/> representing the result of the merge.
        /// </returns>
        public virtual IMergeResult Merge(object source, object target, [NotNull] IInjectionContext context, object hint)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var action = PostMergeAction.None;
            if (target == null)
            {
                var enumerableFactory = context.InstanceRegistry.Resolve<IEnumerableFactory>();
                target = enumerableFactory.Resolve(this.Target);
                action = PostMergeAction.Replace;
            }
            
            this.Merger.Merge((dynamic)source, (dynamic)target, context);
            return new MergeResult(action, target);
        }
    }
}