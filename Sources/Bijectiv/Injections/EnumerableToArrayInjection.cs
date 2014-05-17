// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayTransform.cs" company="Bijectiv">
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
//   Defines the ArrayTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="IInjection"/> that injects an <see cref="IEnumerable"/> instance into an array.
    /// </summary>
    public class EnumerableToArrayInjection : ITransform, IMerge
    {
        private readonly Type source;

        private readonly Type target;

        private readonly Type targetElement;

        private readonly Func<IEnumerable<object>, object> resultFactory;

        public EnumerableToArrayInjection([NotNull] Type source, [NotNull] Type target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!typeof(IEnumerable).IsAssignableFrom(source))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' does not implement IEnumerable.", source), "source");
            }

            if (!target.IsArray)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' does is not an array.", target), "target");
            }

            if (target.GetArrayRank() != 1)
            {
                throw new ArgumentException(
                    string.Format("Array rank must be 1, but is {0}.", target.GetArrayRank()), 
                    "target");
            }

            this.source = source;
            this.target = target;
            this.targetElement = target.GetElementType();
            this.resultFactory = this.CreateResultFactory();
        }

        /// <summary>
        /// Gets the source type supported by the injection.
        /// </summary>
        public Type Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the target type created by the injection.
        /// </summary>
        public Type Target
        {
            get { return this.target; }
        }

        public Type TargetElement
        {
            get { return this.targetElement; }
        }

        public Func<IEnumerable<object>, object> ResultFactory
        {
            get { return this.resultFactory; }
        }

        public object Transform(object source, [NotNull] IInjectionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var result = new List<object>();
            foreach (var item in (IEnumerable)source)
            {
                if (item == null)
                {
                    result.Add(this.TargetElement.GetDefault());
                }
                else
                {
                    var injection = context.InjectionStore.Resolve<ITransform>(item.GetType(), this.TargetElement);
                    result.Add(injection.Transform(item, context));
                }
            }

            return this.ResultFactory(result);
        }

        public IMergeResult Merge(object source, object target, IInjectionContext context)
        {
            if (source == null)
            {
                return new MergeResult(PostMergeAction.Replace, null);
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new MergeResult(PostMergeAction.Replace, this.Transform(source, context));
        }

        private Func<IEnumerable<object>, object> CreateResultFactory()
        {
            var itemsParameter = Expression.Parameter(typeof(IEnumerable<object>), "items");
            var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(this.TargetElement);
            var toArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(this.TargetElement);

            var body = Expression.Convert(
                Expression.Call(toArrayMethod, Expression.Call(castMethod, itemsParameter)), 
                typeof(object));

            return Expression
                .Lambda<Func<IEnumerable<object>, object>>(body, itemsParameter)
                .Compile();
        }
    }
}