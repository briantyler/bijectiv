// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateInjectionStoreFactory.cs" company="Bijectiv">
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
//   Defines the DelegateInjectionStoreFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.Utilities;

    /// <summary>
    /// A <see cref="IInjectionStoreFactory"/> that creates a <see cref="IInjectionStore"/> by using a 
    /// <see cref="IInjectionFactory{T}"/> to create delegate transforms.
    /// </summary>
    public class DelegateInjectionStoreFactory : IInjectionStoreFactory
    {
        /// <summary>
        /// The transform factory that creates the delegate transforms.
        /// </summary>
        private readonly IInjectionFactory<IInjection> injectionFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateInjectionStoreFactory"/> class.
        /// </summary>
        /// <param name="injectionFactory">
        /// The injection factory that creates the delegate injections.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public DelegateInjectionStoreFactory([NotNull] IInjectionFactory<IInjection> injectionFactory)
        {
            if (injectionFactory == null)
            {
                throw new ArgumentNullException("injectionFactory");
            }

            this.injectionFactory = injectionFactory;
        }

        /// <summary>
        /// Gets the injection factory that creates the delegate injections.
        /// </summary>
        public IInjectionFactory<IInjection> InjectionFactory
        {
            get { return this.injectionFactory; }
        }

        /// <summary>
        /// Creates a <see cref="IInjectionStore"/> from a <see cref="IInstanceRegistry"/>.
        /// </summary>
        /// <param name="registry">
        /// The registry from which to create the store.
        /// </param>
        /// <returns>
        /// The created <see cref="IInjectionStore"/>.
        /// </returns>
        public IInjectionStore Create([NotNull] IInstanceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var store = new CollectionInjectionStore();

            registry
                .ResolveAll<InjectionDefinition>()
                .ForEach(definition => store.Add(this.InjectionFactory.Create(registry, definition)));

            return store;
        }
    }
}