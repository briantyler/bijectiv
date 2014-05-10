// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionInjectionStore.cs" company="Bijectiv">
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
//   Defines the CollectionInjectionStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Stores
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// A store that contains a collection of <see cref="IInjection"/> instances that are keyed by their source and
    /// target types.
    /// </summary>
    public class CollectionInjectionStore : IInjectionStore, IEnumerable<IInjection>
    {
        /// <summary> TODO this needs to be a more appropriate data type.
        /// The injections.
        /// </summary>
        private readonly List<IInjection> injections = new List<IInjection>();

        /// <summary>
        /// Adds a new <see cref="IInjection"/> to the store.
        /// </summary>
        /// <param name="injection">
        /// The injection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Add([NotNull] IInjection injection)
        {
            if (injection == null)
            {
                throw new ArgumentNullException("injection");
            }

            this.injections.Add(injection);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{ITransform}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IInjection> GetEnumerator()
        {
            return this.injections.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.injections).GetEnumerator();
        }

        /// <summary>
        /// Resolves a <see cref="IInjection"/> that injects instances of type <paramref name="source"/> into
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
        /// A <see cref="IInjection"/> that injects instances of type <paramref name="source"/> into
        /// instances of type <paramref name="target"/> if one exists, or; <c>null</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
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

            return this.injections.OfType<TInjection>().FirstOrDefault(
                candidate => candidate.Source == source && candidate.Target == target);
        }
    }
}