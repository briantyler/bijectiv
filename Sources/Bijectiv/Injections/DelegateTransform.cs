// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateTransform.cs" company="Bijectiv">
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
//   Defines the DelegateTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="ITransform"/> that performs its transformation via a delegate.
    /// </summary>
    public class DelegateTransform : ITransform
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateTransform"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="delegate">
        /// The delegate that performs the transform.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public DelegateTransform(
            [NotNull] Type source, 
            [NotNull] Type target,
            [NotNull] DTransform @delegate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            this.Source = source;
            this.Target = target;
            this.Delegate = @delegate;
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
        /// Gets the delegate that performs the transform.
        /// </summary>
        public DTransform Delegate { get; private set; }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <seealso cref="Target"/>;  using the 
        /// transformation rules defined by <seealso cref="Source"/> --&lt; <seealso cref="Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
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

            return this.Delegate(source, context, hint);
        }
    }
}