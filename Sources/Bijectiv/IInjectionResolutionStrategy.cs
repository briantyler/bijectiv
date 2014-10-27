// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionResolutionStrategy.cs" company="Bijectiv">
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
//   Defines the IInjectionResolutionStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an injection resolution strategy.
    /// </summary>
    public interface IInjectionResolutionStrategy
    {
        /// <summary>
        /// Chooses a <see cref="TInjection"/> from a collection of candidates.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="candidates">
        /// The candidate injections.
        /// </param>
        /// <typeparam name="TInjection">
        /// The type of the injection to choose.
        /// </typeparam>
        /// <returns>
        /// The chosen <see cref="TInjection"/>. Returns <see langword="null"/> if there is no suitable candidate.
        /// </returns>
        [Pure]
        TInjection Choose<TInjection>(
            [NotNull] Type source,
            [NotNull] Type target,
            [NotNull] IEnumerable<TInjection> candidates) 
            where TInjection : IInjection;
    }
}