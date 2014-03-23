// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformer.cs" company="Bijectiv">
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
//   Defines the ITransformer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    /// <summary>
    /// Represents a utility that can transform an object from one type into or onto another.
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Superimposes <paramref name="source"/> onto <paramref name="target"/>; using the transformation rules 
        /// defined by <paramref name="sourceType"/> --&lt; <paramref name="targetType"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="target">
        /// The target object.
        /// </param>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        void Superimpose(object source, object target, Type sourceType, Type targetType, TransformerContext context);

        /// <summary>
        /// Makes an instance of type <paramref name="targetType"/> from <paramref name="source"/>; using the 
        /// transformation rules defined by <paramref name="sourceType"/> --&lt; <paramref name="targetType"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <returns>
        /// The newly minted target instance.
        /// </returns>
        object Make(object source, Type sourceType, Type targetType, TransformerContext context);
    }
}