﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformArtifactBuilder.cs" company="Bijectiv">
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
//   Defines the ITransformArtifactBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    /// <summary>
    /// Represents a builder of <see cref="TransformArtifact"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public interface ITransformArtifactBuilder<TSource, TTarget>
    {
        /// <summary>
        /// Creates an inheritance relationship for the transform: this transform will inherit all of the fragments
        /// from the base transform if it exists.
        /// </summary>
        /// <typeparam name="TSourceBase">
        /// The base type that source inherits from.
        /// </typeparam>
        /// <typeparam name="TTargetBase">
        /// The base type that target inherits from.
        /// </typeparam>
        /// <returns>
        /// The an object that allows further configuration of the <see cref="TransformArtifact"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="TSourceBase"/> is not assignable from <typeparamref name="TSource"/>.
        /// Thrown when <typeparamref name="TTargetBase"/> is not assignable from <typeparamref name="TTarget"/>.
        /// </exception>
        /// <remarks>
        /// There is no way to enforce this relationship at compile time without reversing the relationship; i.e.
        /// calling this method "Derived", but that is not how it should be defined.
        /// </remarks>
        ITransformArtifactBuilder<TSource, TTarget> Inherits<TSourceBase, TTargetBase>();
    }
}