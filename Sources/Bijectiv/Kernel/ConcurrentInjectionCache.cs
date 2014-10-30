// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentInjectionCache.cs" company="Bijectiv">
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
//   Defines the ConcurrentInjectionCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A thread safe, read optimized <see cref="IInjectionCache"/>.
    /// </summary>
    public class ConcurrentInjectionCache : IInjectionCache
    {
        /// <summary>
        /// The cache that holds the injections keyed by type signature.
        /// </summary>
        private readonly IDictionary<Tuple<Type, Type, Type>, IInjection> cache =
            new Dictionary<Tuple<Type, Type, Type>, IInjection>();

        /// <summary>
        /// Gets the <typeparamref name="TInjection"/> for the given <paramref name="source"/> and
        /// <paramref name="target"/> pair; resolving, caching and returning the <typeparamref name="TInjection"/>
        /// provided by the <paramref name="store"/> when no <typeparamref name="TInjection"/> is found in the cache.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="store">
        /// The store from which to resolve when no <typeparamref name="TInjection"/> is found in the cache.
        /// </param>
        /// <typeparam name="TInjection">
        /// The type of the injection to resolve.
        /// </typeparam>
        /// <returns>
        /// The <typeparamref name="TInjection"/> for the given <paramref name="source"/> and
        /// <paramref name="target"/> pair.
        /// </returns>
        public TInjection GetOrAdd<TInjection>(Type source, Type target, IInjectionStore store)
            where TInjection : class, IInjection
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            var key = CreateKey(source, target, typeof(TInjection));

            // Almost all of the time the injection will exist in the cache so this is optimized so that almost
            // always no lock is taken.
            IInjection injection;
            if (!this.cache.TryGetValue(key, out injection))
            {
                lock (this)
                {
                    // There is a race condition that an injection could be cached between trying to get the injection
                    // the first time and now, so it is necessary to try again.
                    if (!this.cache.TryGetValue(key, out injection))
                    {
                        this.cache[key] = injection = store.Resolve<TInjection>(source, target);
                    }
                }
            }

            return (TInjection)injection;
        }

        /// <summary>
        /// Creates a key for the injection in the cache.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="injection">
        /// The injection type.
        /// </param>
        /// <returns>
        /// A key that uniquely identifies an injection in the cache.
        /// </returns>
        private static Tuple<Type, Type, Type> CreateKey(Type source, Type target, Type injection)
        {
            return Tuple.Create(source, target, injection);
        }
    }
}