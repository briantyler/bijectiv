﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITargetFinder.cs" company="Bijectiv">
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
//   Defines the ITargetFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System.Collections;

    /// <summary>
    /// Finds the element in a collection that is the target for a source object.
    /// </summary>
    public interface ITargetFinder
    {
        /// <summary>
        /// Initializes the finder for a given target collection.
        /// </summary>
        /// <param name="targets">
        /// The collection of target elements.
        /// </param>
        /// <param name="context">
        /// The context in which the targets are being found.
        /// </param>
        /// <remarks>
        /// The consumer should cache the items in the collection as the collection may change during the lifetime
        /// of the consumer.
        /// </remarks>
        void Initialize(IEnumerable targets, IInjectionContext context);

        /// <summary>
        /// Tries to find the target element for <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source element.
        /// </param>
        /// <param name="target">
        /// The target element; this is only valid if the method returns <see langword="true" />.
        /// </param>
        /// <returns>
        /// A value indicating success.
        /// </returns>
        bool TryFind(object source, out object target);
    }
}