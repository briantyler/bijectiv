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
    using System.Collections.Generic;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="IInjection"/> that injects an <see cref="IEnumerable"/> instance into an 
    /// <see cref="IEnumerable"/>.
    /// </summary>
    public class EnumerableToEnumerableInjection : ITransform, IMerge
    {
        public EnumerableToEnumerableInjection(
            [NotNull] Type source, 
            [NotNull] Type target,
            [NotNull] IEnumerableFactory factory,
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

            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            if (merger == null)
            {
                throw new ArgumentNullException("merger");
            }

            this.Source = source;
            this.Target = target;
            this.Factory = factory;
            this.Merger = merger;
        }

        public Type Source { get; private set; }

        public Type Target { get; private set; }

        public IEnumerableFactory Factory { get; private set; }

        public ICollectionMerger Merger { get; private set; }

        public object Transform(object source, [NotNull] IInjectionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return this.Merge(source, this.Factory.Resolve(this.Target), context).Target;
        }

        public virtual IMergeResult Merge(object source, object target, [NotNull] IInjectionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return this.Merger.Merge((dynamic)source, (dynamic)target, context);
        }
    }
}