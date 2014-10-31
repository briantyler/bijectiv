// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingInjectionStore.cs" company="Bijectiv">
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
//   Defines the CachingInjectionStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// A store that resolves <see cref="IInjection"/> from an other <see cref="IInjectionStore"/>s and caches the
    /// result.
    /// </summary>
    public class CachingInjectionStore : IInjectionStore
    {
        /// <summary>
        /// The underlying store from which injections are cached.
        /// </summary>
        private readonly IInjectionStore underlyingStore;

        /// <summary>
        /// The injection cache.
        /// </summary>
        private readonly IInjectionCache cache;

        /// <summary>
        /// Initialises a new instance of the <see cref="CachingInjectionStore"/> class.
        /// </summary>
        /// <param name="underlyingStore">
        /// The underlying store from which injections are cached.
        /// </param>
        /// <param name="cache">
        /// The injection cache.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public CachingInjectionStore([NotNull] IInjectionStore underlyingStore, [NotNull] IInjectionCache cache)
        {
            if (underlyingStore == null)
            {
                throw new ArgumentNullException("underlyingStore");
            }

            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            this.underlyingStore = underlyingStore;
            this.cache = cache;
        }

        /// <summary>
        /// Gets the The underlying store from which injections are cached.
        /// </summary>
        public IInjectionStore UnderlyingStore
        {
            get { return this.underlyingStore; }
        }

        /// <summary>
        /// Gets the injection cache.
        /// </summary>
        public IInjectionCache Cache
        {
            get { return this.cache; }
        }

        /// <summary>
        /// Resolves a <typeparamref name="TInjection"/> that injects instances of type <paramref name="source"/> into
        /// instances of type <paramref name="target"/> if one exists, or; returns <c>null</c> otherwise.
        /// </summary>
        /// <typeparam name="TInjection">
        /// The type of <see cref="IInjection"/> to resolve.
        /// </typeparam>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <returns>
        /// A <typeparamref name="TInjection"/> that injects instances of type <paramref name="source"/> into 
        /// instances of type <paramref name="target"/> if one exists, or; <c>null</c> otherwise.
        /// </returns>
        public TInjection Resolve<TInjection>(Type source, Type target) where TInjection : class, IInjection
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return this.Cache.GetOrAdd<TInjection>(source, target, this.UnderlyingStore);
        }
    }
}