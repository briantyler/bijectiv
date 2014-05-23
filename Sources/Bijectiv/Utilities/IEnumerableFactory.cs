// --------------------------------------------------------------------------------------------------------------------
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
    using System.Collections.Generic;

    /// <summary>
    /// A factory that creates <see cref="IEnumerable"/> instances.
    /// </summary>
    public interface IEnumerableFactory
    {
        /// <summary>
        /// Registers an enumerable monad interface to a collection monad concrete type.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The <see cref="IEnumerable{T}"/> interface.
        /// </typeparam>
        /// <typeparam name="TConcrete">
        /// The <see cref="ICollection{T}"/> concrete type.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown when either <typeparamref name="TInterface"/> or <typeparamref name="TConcrete"/> is not a monadic
        /// type.
        /// </exception>
        /// <example>
        ///     Use the <see cref="Placeholder"/> type to register a generic type:
        ///     <code>
        ///         factory.Register&lt;ISet&lt;Placeholder&gt;, HashSet&lt;Placeholder&gt;&gt;();
        ///     </code>
        /// </example>
        /// <remarks>
        /// It would be possible to constuct parameters that do not behave as expected, but you are very unlikely to 
        /// do this by accident.
        /// </remarks>
        void Register<TInterface, TConcrete>()
            where TInterface : IEnumerable<Placeholder>
            where TConcrete : ICollection<Placeholder>, TInterface, new();

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