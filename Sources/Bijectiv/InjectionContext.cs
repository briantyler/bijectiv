// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionContext.cs" company="Bijectiv">
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
//   Defines the InjectionContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Globalization;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents the context in which a <see cref="IInjection"/> is happening.
    /// </summary>
    public class InjectionContext : IInjectionContext
    {
        /// <summary>
        /// The culture in which the transform is taking place.
        /// </summary>
        private readonly CultureInfo culture;

        /// <summary>
        /// The resolve delegate.
        /// </summary>
        private readonly Func<Type, object> resolveDelegate;

        /// <summary>
        /// The injection kernel.
        /// </summary>
        private readonly IInjectionKernel injectionKernel;

        /// <summary>
        /// The target cache.
        /// </summary>
        private readonly ITargetCache targetCache = new TargetCache();

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionContext"/> class.
        /// </summary>
        /// <param name="culture">
        /// The culture in which the transform is taking place.
        /// </param>
        /// <param name="resolveDelegate">
        /// The resolve delegate.
        /// </param>
        /// <param name="injectionKernel">
        /// The injection kernel.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionContext(
            [NotNull] CultureInfo culture, 
            [NotNull] Func<Type, object> resolveDelegate,
            [NotNull] IInjectionKernel injectionKernel)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            if (resolveDelegate == null)
            {
                throw new ArgumentNullException("resolveDelegate");
            }

            if (injectionKernel == null)
            {
                throw new ArgumentNullException("injectionKernel");
            }

            this.culture = culture;
            this.resolveDelegate = resolveDelegate;
            this.injectionKernel = injectionKernel;
        }

        /// <summary>
        /// Gets the culture in which the transform is taking place.
        /// </summary>
        public CultureInfo Culture
        {
            get { return this.culture; }
        }

        /// <summary>
        /// Gets the target cache.
        /// </summary>
        public ITargetCache TargetCache
        {
            get { return this.targetCache; }
        }

        /// <summary>
        /// Gets the resolve delegate.
        /// </summary>
        public Func<Type, object> ResolveDelegate
        {
            get { return this.resolveDelegate; }
        }

        /// <summary>
        /// Gets the <see cref="IInjection"/> store.
        /// </summary>
        public IInjectionStore InjectionStore
        {
            get { return this.injectionKernel.Store; }
        }

        /// <summary>
        /// Gets the instance registry.
        /// </summary>
        public IInstanceRegistry InstanceRegistry
        {
            get { return this.injectionKernel.Registry; }
        }

        /// <summary>
        /// Retrieve a service from the default factory.
        /// </summary>
        /// <param name="service">
        /// The service to retrieve.
        /// </param>
        /// <returns>
        /// The component instance that provides the service.
        /// </returns>
        public object Resolve([NotNull] Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            return this.ResolveDelegate(service);
        }
    }
}