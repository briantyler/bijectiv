// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionFactory.cs" company="Bijectiv">
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
//   Defines the IInjectionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.InjectionFactory
{
    using System;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    /// <summary>
    /// Represents a factory that creates <typeparamref name="TInjection"/> instances from a 
    /// <see cref="InjectionDefinition"/> and a <see cref="IInstanceRegistry"/>.
    /// </summary>
    /// <typeparam name="TInjection">
    /// The type of <see cref="IInjection"/> to create.
    /// </typeparam>
    public interface IInjectionFactory<out TInjection> where TInjection : IInjection
    {
        /// <summary>
        /// Creates a <typeparamref name="TInjection"/> from a <see cref="InjectionDefinition"/>.
        /// </summary>
        /// <param name="definitionRegistry">
        /// The registry containing all known instances.
        /// </param>
        /// <param name="definition">
        /// The definition from which to create a <typeparamref name="TInjection"/>.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TInjection"/> that is defined by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        TInjection Create(
            [NotNull] IInstanceRegistry definitionRegistry, 
            [NotNull] InjectionDefinition definition);
    }
}