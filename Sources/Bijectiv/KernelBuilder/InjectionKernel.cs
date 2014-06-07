// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernel.cs" company="Bijectiv">
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
//   Defines the InjectionKernel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelBuilder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// A kernel that provides <see cref="IInjection"/> capabilities.
    /// </summary>
    public class InjectionKernel : IInjectionKernel
    {
        /// <summary>
        /// The store containing all known <see cref="IInjection"/> instances.
        /// </summary>
        private readonly IInjectionStore store;

        /// <summary>
        /// The registry containing all instances that are required to support the <see cref="IInjection"/> 
        /// instances known by the <see cref="IInjectionKernel.Store"/>.
        /// </summary>
        private readonly IInstanceRegistry registry;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionKernel"/> class.
        /// </summary>
        /// <param name="store">
        /// The store containing all known <see cref="IInjection"/> instances.
        /// </param>
        /// <param name="registry">
        /// The registry containing all instances that are required to support the <see cref="IInjection"/> 
        /// instances known by the <see cref="IInjectionKernel.Store"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionKernel([NotNull] IInjectionStore store, [NotNull] IInstanceRegistry registry)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            this.store = store;
            this.registry = registry;
        }

        /// <summary>
        /// Gets the store containing all known <see cref="IInjection"/> instances.
        /// </summary>
        public IInjectionStore Store
        {
            get { return this.store; }
        }

        /// <summary>
        /// Gets the registry containing all instances that are required to support the <see cref="IInjection"/> 
        /// instances known by the <see cref="IInjectionKernel.Store"/>.
        /// </summary>
        public IInstanceRegistry Registry
        {
            get { return this.registry; }
        }
    }
}