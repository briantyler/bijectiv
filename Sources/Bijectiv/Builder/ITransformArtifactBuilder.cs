// --------------------------------------------------------------------------------------------------------------------
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

    using JetBrains.Annotations;

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
        : ITransformArtifactBuilderF<TSource, TTarget>
    {
        /// <summary>
        /// Instructs the transform to construct the target via activation (this is the default option if no other
        /// construction method is specified).
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        ITransformArtifactBuilderF<TSource, TTarget> Activate();

        /// <summary>
        /// Instructs the transform to construct the target via the default factory.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        ITransformArtifactBuilderF<TSource, TTarget> DefaultFactory();

        /// <summary>
        /// Instructs the transform to construct the target via a custom factory delegate.
        /// </summary>
        /// <param name="factory">
        /// The factory delegate that constructs the target.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        ITransformArtifactBuilderF<TSource, TTarget> CustomFactory(
            [NotNull] Func<CustomFactoryParameters<TSource>, TTarget> factory);
    }
}