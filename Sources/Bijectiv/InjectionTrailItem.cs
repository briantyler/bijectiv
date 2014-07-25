// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTrailItem.cs" company="Bijectiv">
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
//   Defines the InjectionTrailItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// An item that forms part of an injection trail.
    /// </summary>
    public class InjectionTrailItem
    {
        /// <summary>
        /// The <see cref="IInjection"/> that injects <see cref="source"/> to <see cref="target"/>.
        /// </summary>
        private readonly IInjection injection;

        /// <summary>
        /// The injection source.
        /// </summary>
        private readonly object source;

        /// <summary>
        /// The injection target.
        /// </summary>
        private readonly object target;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionTrailItem"/> class.
        /// </summary>
        /// <param name="injection">
        /// The <see cref="IInjection"/> that injects <paramref name="source"/> to <paramref name="target"/>.
        /// </param>
        /// <param name="source">
        /// The injection source.
        /// </param>
        /// <param name="target">
        /// The injection target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="injection"/> is null.
        /// </exception>
        public InjectionTrailItem([NotNull] IInjection injection, object source, object target)
        {
            if (injection == null)
            {
                throw new ArgumentNullException("injection");
            }

            this.injection = injection;
            this.source = source;
            this.target = target;
        }

        /// <summary>
        /// Gets the <see cref="IInjection"/> that injects <see cref="Source"/> to <see cref="Target"/>.
        /// </summary>
        public IInjection Injection
        {
            get { return this.injection; }
        }

        /// <summary>
        /// Gets the injection source.
        /// </summary>
        public object Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the injection target.
        /// </summary>
        public object Target
        {
            get { return this.target; }
        }
    }
}