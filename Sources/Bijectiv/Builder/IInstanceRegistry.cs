// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInstanceRegistry.cs" company="Bijectiv">
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
//   Defines the IInstanceRegistry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a registry that contains instances.
    /// </summary>
    public interface IInstanceRegistry
    {
        /// <summary>
        /// Registers <paramref name="instance"/> as an instance of type <paramref name="instanceType"/>.
        /// </summary>
        /// <param name="instanceType">
        /// The type to register <paramref name="instance"/> as.
        /// </param>
        /// <param name="instance">
        /// The instance to register.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="instance"/> is not assignable to a variable of type
        /// <paramref name="instanceType"/>.
        /// </exception>
        void Register([NotNull] Type instanceType, [NotNull] object instance);

        /// <summary>
        /// Resolves all instances of type <typeparamref name="TInstance"/> in the order in which they were
        /// registered.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The type of instance to resolve.
        /// </typeparam>
        /// <returns>
        /// The collection of all instances of type <typeparamref name="TInstance"/> in the order in which they were
        /// registered.
        /// </returns>
        IEnumerable<TInstance> Resolve<TInstance>();

        void Register([NotNull] Tuple<Type, object> registration);
    }
}