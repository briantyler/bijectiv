// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Bijectiv">
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
//   Defines the EnumerableExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JetBrains.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents extensions to types that implement <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates two sequences, where the second sequence is constructed from a collection of arguments.
        /// </summary>
        /// <param name="this">
        /// The first sequence.
        /// </param>
        /// <param name="args">
        /// The collection of arguments to concatenate with the first.
        /// </param>
        /// <typeparam name="T">
        /// The type of items in the collection.
        /// </typeparam>
        /// <returns>
        /// The concatenated sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="this"/> is null.
        /// </exception>
        public static IEnumerable<T> Concat<T>([NotNull] this IEnumerable<T> @this, params T[] args)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return Enumerable.Concat(@this, args);
        }

        /// <summary>
        /// Performs the specified action on each element of the collection.
        /// </summary>
        /// <param name="this">
        /// The collection.
        /// </param>
        /// <param name="action">
        /// The action to perform.
        /// </param>
        /// <typeparam name="T">
        /// The type of items in the collection.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any parameter is null.
        /// </exception>
        public static void ForEach<T>([NotNull] this IEnumerable<T> @this, [NotNull] Action<T> action)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
           
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in @this)
            {
                action(item);
            }
        }
    }
}