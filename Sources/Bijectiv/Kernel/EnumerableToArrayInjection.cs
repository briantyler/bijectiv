// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToArrayInjection.cs" company="Bijectiv">
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
//   Defines the EnumerableToArrayInjection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="IInjection"/> that injects an <see cref="IEnumerable"/> instance into an array.
    /// </summary>
    public class EnumerableToArrayInjection : ITransform, IMerge
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EnumerableToArrayInjection"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="merger">
        /// The merger.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="source"/> does not implement <see cref="IEnumerable"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="target"/> is not an array.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="target"/> is a multi-dimensional array.
        /// </exception>
        public EnumerableToArrayInjection(
            [NotNull] Type source, 
            [NotNull] Type target,
            [CanBeNull] ICollectionMerger merger)
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

            if (!typeof(IEnumerable).IsAssignableFrom(source))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' does not implement IEnumerable.", source),
                    "source");
            }

            if (!target.IsArray)
            {
                throw new ArgumentException(string.Format("Type '{0}' is not an array.", target), "target");
            }

            if (target.GetArrayRank() != 1)
            {
                throw new ArgumentException(
                    string.Format("Array rank must be 1, but is {0}.", target.GetArrayRank()),
                    "target");
            }

            this.Target = target;
            this.Merger = merger;
            this.TargetElement = target.GetElementType();
            this.EnumerableTarget = typeof(IEnumerable<>).MakeGenericType(this.TargetElement);
            this.ResultFactory = this.CreateResultFactory();
        }

        /// <summary>
        /// Gets the source type supported by the injection.
        /// </summary>
        public Type Source
        {
            get { return typeof(IEnumerable); }
        }

        /// <summary>
        /// Gets the target type supported by the injection.
        /// </summary>
        public Type Target { get; private set; }

        /// <summary>
        /// Gets the target type element type.
        /// </summary>
        public Type TargetElement { get; private set; }

        /// <summary>
        /// Gets the target type enumerable type.
        /// </summary>
        public Type EnumerableTarget { get; private set; }

        /// <summary>
        /// Gets the merger that merges items from one collection into another collection.
        /// </summary>
        public ICollectionMerger Merger { get; private set; }

        /// <summary>
        /// Gets a factory that creates the appropriately typed array result.
        /// </summary>
        public Func<object, object> ResultFactory { get; private set; }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <see cref="IInjection.Target"/>;  
        /// using the transformation rules defined by <see cref="IInjection.Source"/> --&lt; 
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
        public virtual object Transform(object source, [NotNull] IInjectionContext context, object hint)
        {
            if (source == null)
            {
                return null;
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var target = context.InstanceRegistry.Resolve<IEnumerableFactory>().Resolve(this.EnumerableTarget);
            this.Merger.Merge((dynamic)source, (dynamic)target, context);

            return this.ResultFactory(target);
        }

        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>; using the transformation rules 
        /// defined by <see cref="IInjection.Source"/> --&lt;  <see cref="IInjection.Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="target">
        /// The target object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <param name="hint">
        /// A hint that can be used to pass additional information to the injection.
        /// </param>
        /// <returns>
        /// A <see cref="IMergeResult"/> representing the result of the merge.
        /// </returns>
        /// <remarks>
        /// Merge is not possible for array types so <see cref="PostMergeAction.Replace"/> is always returned.
        /// </remarks>
        public IMergeResult Merge(object source, object target, IInjectionContext context, object hint)
        {
            return new MergeResult(PostMergeAction.Replace, this.Transform(source, context, null));
        }

        /// <summary>
        /// Creates a factory that creates the appropriately typed array result.
        /// </summary>
        /// <returns>
        /// A factory that creates the appropriately typed array result.
        /// </returns>
        private Func<object, object> CreateResultFactory()
        {
            var itemsParameter = Expression.Parameter(typeof(object), "items");
            var castToEnumerable = Expression.Convert(itemsParameter, this.EnumerableTarget);
            var toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(this.TargetElement);
            var body = Expression.Convert(Expression.Call(toArrayMethod, castToEnumerable), typeof(object));

            return Expression.Lambda<Func<object, object>>(body, itemsParameter).Compile();
        }
    }
}