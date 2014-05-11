﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionStoreBuilder.cs" company="Bijectiv">
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
//   Defines the InjectionStoreBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Builder;
    using Bijectiv.Stores;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a fine grained factory that can be used to incrementally build a <see cref="IInjectionStore"/>.
    /// </summary>
    public class InjectionStoreBuilder
    {
        /// <summary>
        /// The registry.
        /// </summary>
        private readonly IInjectionDefinitionRegistry registry;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionStoreBuilder"/> class.
        /// </summary>
        public InjectionStoreBuilder()
            : this(new InjectionDefinitionRegistry())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionStoreBuilder"/> class.
        /// </summary>
        /// <param name="registry">
        /// The registry.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionStoreBuilder([NotNull] IInjectionDefinitionRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            this.registry = registry;
        }

        /// <summary>
        /// Gets the registry.
        /// </summary>
        protected internal virtual IInjectionDefinitionRegistry Registry
        {
            get { return this.registry; }
        }

        /// <summary>
        /// Registers a callback with the store builder: an extensibility point.
        /// </summary>
        /// <param name="callback">
        /// The configuration callback.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public virtual void RegisterCallback([NotNull] Action<IInjectionDefinitionRegistry> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            callback(this.Registry);
        }

        /// <summary>
        /// Builds a store according to the current specification.
        /// </summary>
        /// <param name="factories">
        /// The factories that create the transform stores.
        /// </param>
        /// <returns>
        /// A <see cref="IInjectionStore"/> that matches the current specification.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public virtual IInjectionStore Build([NotNull] IEnumerable<IInjectionStoreFactory> factories)
        {
            if (factories == null)
            {
                throw new ArgumentNullException("factories");
            }

            var store = new CompositeInjectionStore();

            factories.ForEach(factory => store.Add(factory.Create(this.Registry)));

            return store;
        }
    }
}