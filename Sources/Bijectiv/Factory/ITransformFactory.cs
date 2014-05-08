// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransformFactory.cs" company="Bijectiv">
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
//   Defines the ITransformFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;

    using Bijectiv.Builder;
    using Bijectiv.Transforms;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a factory that creates <see cref="DelegateTransform"/> instances.
    /// </summary>
    public interface ITransformFactory
    {
        /// <summary>
        /// Creates a <see cref="ITransform"/> from a <see cref="TransformDefinition"/>.
        /// </summary>
        /// <param name="definitionRegistry">
        /// The registry containing all known <see cref="TransformDefinition"/> instances.
        /// </param>
        /// <param name="definition">
        /// The definition from which to create a <see cref="ITransform"/>.
        /// </param>
        /// <returns>
        /// The <see cref="ITransform"/> that is defined by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        ITransform Create(
            [NotNull] ITransformDefinitionRegistry definitionRegistry, 
            [NotNull] TransformDefinition definition);
    }
}