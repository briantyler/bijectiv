﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILabelCache.cs" company="Bijectiv">
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
//   Defines the ILabelCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a cache of labels that are scoped by object and identified by category.
    /// </summary>
    public interface ILabelCache
    {
        /// <summary>
        /// Gets the label from the cache that is scoped to <paramref name="scope"/> and identified by 
        /// <paramref name="category"/>.
        /// </summary>
        /// <param name="scope">
        /// The object that defines the label scope.
        /// </param>
        /// <param name="category">
        /// The label category.
        /// </param>
        /// <returns>
        /// The <see cref="LabelTarget"/> that is scoped to <paramref name="scope"/> and identified by 
        /// <paramref name="category"/>.
        /// </returns>
        LabelTarget GetLabel([NotNull] object scope, Guid category);
    }
}