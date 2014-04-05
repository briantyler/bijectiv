﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransform.cs" company="Bijectiv">
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
//   Defines the ITransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    /// <summary>
    /// Represents a transform from a source to target.
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Gets the source type supported by the transform.
        /// </summary>
        Type Source { get; }

        /// <summary>
        /// Gets the target type created by the transform.
        /// </summary>
        Type Target { get; }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <seealso cref="Target"/>;  using the 
        /// transformation rules defined by <seealso cref="Source"/> --&lt; <seealso cref="Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <returns>
        /// The newly minted target instance.
        /// </returns>
        object Transform(object source, TransformContext context); 
    }
}