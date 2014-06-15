// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernelBuilder.cs" company="Bijectiv">
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
//   Defines the InjectionKernelBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.Utilities;

    /// <summary>
    /// Represents a fine grained factory that can be used to incrementally build a <see cref="IInjectionKernel"/>.
    /// </summary>
    public class InjectionKernelBuilder
    {
        /// <summary>
        /// The instance registry.
        /// </summary>
        private readonly IInstanceRegistry instanceRegistry;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionKernelBuilder"/> class.
        /// </summary>
        public InjectionKernelBuilder()
            : this(new InstanceRegistry())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionKernelBuilder"/> class.
        /// </summary>
        /// <param name="instanceRegistry">
        /// The instance registry.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionKernelBuilder([NotNull] IInstanceRegistry instanceRegistry)
        {
            if (instanceRegistry == null)
            {
                throw new ArgumentNullException("instanceRegistry");
            }

            this.instanceRegistry = instanceRegistry;
        }

        /// <summary>
        /// Gets the instance registry.
        /// </summary>
        public virtual IInstanceRegistry InstanceRegistry
        {
            get { return this.instanceRegistry; }
        }

        /// <summary>
        /// Builds a store according to the current specification.
        /// </summary>
        /// <param name="storeFactories">
        /// The factories that create the <see cref="IInjectionStore"/>.
        /// </param>
        /// <param name="instanceFactories">
        /// The factories that create helper objects required for injection.
        /// </param>
        /// <returns>
        /// A <see cref="IInjectionKernel"/> that matches the current specification.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public virtual IInjectionKernel Build(
            [NotNull] IEnumerable<IInjectionStoreFactory> storeFactories,
            [NotNull] IEnumerable<IInstanceFactory> instanceFactories)
        {
            if (storeFactories == null)
            {
                throw new ArgumentNullException("storeFactories");
            }

            if (instanceFactories == null)
            {
                throw new ArgumentNullException("instanceFactories");
            }

            var store = new CompositeInjectionStore();
            storeFactories.ForEach(factory => store.Add(factory.Create(this.InstanceRegistry)));

            var registry = new InstanceRegistry();
            instanceFactories.ForEach(factory => registry.Register(factory.Create(this.InstanceRegistry)));

            return new InjectionKernel(store, registry);
        }
    }
}