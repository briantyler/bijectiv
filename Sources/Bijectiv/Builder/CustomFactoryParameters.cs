// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomFactoryParameters.cs" company="Bijectiv">
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
//   Defines the CustomFactoryParameters type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents the parameters to a custom factory method.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source for which the target is being created.
    /// </typeparam>
    public class CustomFactoryParameters<TSource>
    {
        /// <summary>
        /// The source for which the target is being created.
        /// </summary>
        private readonly TSource source;

        /// <summary>
        /// The transform context.
        /// </summary>
        private readonly IInjectionContext context;

        /// <summary>
        /// Initialises a new instance of the <see cref="CustomFactoryParameters{TSource}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public CustomFactoryParameters([NotNull] TSource source, [NotNull] IInjectionContext context)
        {
            if (ReferenceEquals(source, null))
            {
                throw new ArgumentNullException("source");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.source = source;
            this.context = context;
        }

        /// <summary>
        /// Gets the source for which the target is being created.
        /// </summary>
        public TSource Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the transform context.
        /// </summary>
        public IInjectionContext Context
        {
            get { return this.context; }
        }
    }
}