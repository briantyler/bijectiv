﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITargetFinderStore.cs" company="Bijectiv">
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
//   Defines the ITargetFinderStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;

    using Bijectiv.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// Represents a store that holds <see cref="ITargetFinder"/> instances.
    /// </summary>
    public interface ITargetFinderStore
    {
        /// <summary>
        /// Registers a <see cref="TargetFinderRegistration"/> with the store.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        void Register([NotNull] TargetFinderRegistration registration);

        /// <summary>
        /// Resolves a <see cref="ITargetFinder"/> that finds the target element for a source element in a collection
        /// of elements of type <paramref name="targetElement"/> or <see langword="null"/> if one does not exist.
        /// </summary>
        /// <param name="sourceElement">
        /// The source element type.
        /// </param>
        /// <param name="targetElement">
        /// The target element type.
        /// </param>
        /// <returns>
        /// A <see cref="ITargetFinder"/> that finds the target element for a source element in a collection
        /// of elements of type <paramref name="targetElement"/> or <see langword="null"/> if one does not exist.
        /// </returns>
        ITargetFinder Resolve(Type sourceElement, Type targetElement);
    }
}