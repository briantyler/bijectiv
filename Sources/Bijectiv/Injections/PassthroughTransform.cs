// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PassThroughTransform.cs" company="Bijectiv">
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
//   Defines the PassThroughTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a transform that returns its original source as target.
    /// </summary>
    public class PassThroughTransform : ITransform
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PassThroughTransform"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="source"/> type is not assignable to the <paramref name="target"/> type.
        /// </exception>
        public PassThroughTransform([NotNull] Type source, [NotNull] Type target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!target.IsAssignableFrom(source))
            {
                throw new ArgumentException(
                    string.Format(@"Source type '{0}' cannot be assigned to target type '{1}'", source, target),
                    "target");
            }

            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        /// Gets the source type supported by the transform.
        /// </summary>
        public Type Source { get; private set; }

        /// <summary>
        /// Gets the target type created by the transform.
        /// </summary>
        public Type Target { get; private set; }

        /// <summary>
        /// Returns the original source instance.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <returns>
        /// The original source instance.
        /// </returns>
        public object Transform(object source, IInjectionContext context)
        {
            return source;
        }
    }
}