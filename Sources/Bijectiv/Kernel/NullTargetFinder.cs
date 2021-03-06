﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullTargetFinder.cs" company="Bijectiv">
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
//   Defines the NullTargetFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System.Collections;

    /// <summary>
    /// A target finder that always returns <see langword="false"/>.
    /// </summary>
    public class NullTargetFinder : ITargetFinder
    {
        /// <summary>
        /// Initializes the finder for a given target collection.
        /// </summary>
        /// <param name="targets">
        /// The collection of target elements.
        /// </param>
        /// <param name="context">
        /// The context in which the target is being found.
        /// </param>
        public void Initialize(IEnumerable targets, IInjectionContext context)
        {
        }

        /// <summary>
        /// Tries to find the target element for <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source element.
        /// </param>
        /// <param name="target">
        /// Returns <see langword="null"/>.
        /// </param>
        /// <returns>
        /// Returns <see langword="false"/>.
        /// </returns>
        public bool TryFind(object source, out object target)
        {
            target = default(object);
            return false;
        }
    }
}