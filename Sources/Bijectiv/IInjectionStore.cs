﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionStore.cs" company="Bijectiv">
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
//   Defines the IInjectionStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a store that holds <see cref="IInjection"/> instances.
    /// </summary>
    public interface IInjectionStore
    {
        /// <summary>
        /// Resolves a <typeparamref name="TInjection"/> that injects instances of type <paramref name="source"/> into
        /// instances of type <paramref name="target"/> if one exists, or; returns <c>null</c> otherwise.
        /// </summary>
        /// <typeparam name="TInjection">
        /// The type of <see cref="IInjection"/> to resolve.
        /// </typeparam>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <returns>
        /// A <typeparamref name="TInjection"/> that injects instances of type <paramref name="source"/> into 
        /// instances of type <paramref name="target"/> if one exists, or; <c>null</c> otherwise.
        /// </returns>
        TInjection Resolve<TInjection>([NotNull] Type source, [NotNull] Type target)
            where TInjection : class, IInjection;
    }
}