// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICollectionMerger.cs" company="Bijectiv">
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
//   Defines the ICollectionMerger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection merger that should be called via dynamic dispatch.
    /// </summary>
    /// <example>
    /// <code>
    ///     var result = merger.Merge((dynamic)source, (dynamic)target, context);
    /// </code>
    /// </example>
    public interface ICollectionMerger
    {
        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="TTargetElement">
        /// The target element type.
        /// </typeparam>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="target">
        /// The target collection.
        /// </param>
        /// <param name="context">
        /// The context in which the merge will take place.
        /// </param>
        void Merge<TTargetElement>(
            IEnumerable source, 
            ICollection<TTargetElement> target, 
            IInjectionContext context);

        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="TSourceElement">
        /// The source element type.
        /// </typeparam>
        /// <typeparam name="TTargetElement">
        /// The target element type.
        /// </typeparam>
        /// <param name="source">
        /// The source collection.
        /// </param>
        /// <param name="target">
        /// The target collection.
        /// </param>
        /// <param name="context">
        /// The context in which the merge will take place.
        /// </param>
        void Merge<TSourceElement, TTargetElement>(
            IEnumerable<TSourceElement> source,
            ICollection<TTargetElement> target,
            IInjectionContext context);
    }
}