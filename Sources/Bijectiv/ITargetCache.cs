// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITargetCache.cs" company="Bijectiv">
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
//   Defines the ITargetCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    /// <summary>
    /// Represents a target instance cache.
    /// </summary>
    public interface ITargetCache
    {
        /// <summary>
        /// Adds a target instance to the cache.
        /// </summary>
        /// <remarks>
        /// The key is composed of <paramref name="sourceType"/>, <paramref name="targetType"/> and
        /// <paramref name="source"/>.
        /// </remarks>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target to add.
        /// </param>
        void Add(Type sourceType, Type targetType, object source, object target);

        /// <summary>
        /// Tries to get a target from the cache.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The found target. This value is only valid when the return value is <c>true</c>.
        /// </param>
        /// <returns>
        /// A value indicating whether a target was found.
        /// </returns>
        bool TryGet(Type sourceType, Type targetType, object source, out object target);
    }
}