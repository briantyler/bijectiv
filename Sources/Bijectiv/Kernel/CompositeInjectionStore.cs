// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeInjectionStore.cs" company="Bijectiv">
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
//   Defines the CompositeInjectionStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// A store that resolves <see cref="IInjection"/> from other <see cref="IInjectionStore"/>s.
    /// </summary>
    public class CompositeInjectionStore : IInjectionStore, IEnumerable<IInjectionStore>
    {
        /// <summary>
        /// The stores from which this store is composed.
        /// </summary>
        private readonly List<IInjectionStore> stores = new List<IInjectionStore>();

        /// <summary>
        /// TAdds a new store to the composite store.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Add([NotNull] IInjectionStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.stores.Add(store);
        }

        /// <summary>
        /// Returns a strongly typed enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IInjectionStore> GetEnumerator()
        {
            return this.stores.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
            // This method is performance critical as it gets hit all the time and should not use LINQ.

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < this.stores.Count; ++index)
            {
                var transform = this.stores[index].Resolve<TInjection>(source, target);
                if (transform != null)
                {
                    return transform;
                }
            }

            return null;
        }
    }
}