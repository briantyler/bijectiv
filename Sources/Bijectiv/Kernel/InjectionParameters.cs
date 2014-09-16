// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionParameters.cs" company="Bijectiv">
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
//   Defines the InjectionParameters type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    /// <summary>
    /// Standard strongly parameters to an operation that happens as part of a <see cref="IInjection"/>.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public class InjectionParameters<TSource, TTarget> : IInjectionParameters<TSource, TTarget>
    {
        /// <summary>
        /// The source type.
        /// </summary>
        private readonly TSource source;

        /// <summary>
        /// The target type.
        /// </summary>
        private readonly TTarget target;

        /// <summary>
        /// The context in which the operation occurs.
        /// </summary>
        private readonly IInjectionContext context;

        /// <summary>
        /// The hint that was passed to the <see cref="IInjection"/>.
        /// </summary>
        private readonly object hint;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionParameters{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source instance.
        /// </param>
        /// <param name="target">
        /// The target instance.
        /// </param>
        /// <param name="context">
        /// The context in which the operation occurs.
        /// </param>
        /// <param name="hint">
        /// The hint that was passed to the <see cref="IInjection"/>.
        /// </param>
        public InjectionParameters(TSource source, TTarget target, IInjectionContext context, object hint)
        {
            this.source = source;
            this.target = target;
            this.context = context;
            this.hint = hint;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionParameters{TSource,TTarget}"/> class.
        /// </summary>
        internal InjectionParameters()
        {
        }

        /// <summary>
        /// Gets the context in which the operation occurs.
        /// </summary>
        public IInjectionContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets the hint that was passed to the <see cref="IInjection"/>.
        /// </summary>
        public object Hint
        {
            get { return this.hint; }
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public TSource Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        public TTarget Target
        {
            get { return this.target; }
        }
    }
}