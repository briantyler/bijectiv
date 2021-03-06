﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableFactory.cs" company="Bijectiv">
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
//   Defines the IEnumerableFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Collections;

    using Bijectiv.Configuration;

    /// <summary>
    /// A factory that creates <see cref="IEnumerable"/> instances.
    /// </summary>
    public interface IEnumerableFactory
    {
        /// <summary>
        /// Registers a <see cref="EnumerableRegistration"/> with the factory.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        void Register(EnumerableRegistration registration);

        /// <summary>
        /// Resolves an instance of type <paramref name="enumerable"/>.
        /// </summary>
        /// <param name="enumerable">
        /// The (enumerable) type to resolve.
        /// </param>
        /// <returns>
        /// An instance of <paramref name="enumerable"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="enumerable"/> is neither a class nor generic.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no registration exists for creating instances of <paramref name="enumerable"/>.
        /// </exception>
        object Resolve(Type enumerable);
    }
}