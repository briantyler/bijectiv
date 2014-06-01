// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactoryRegistration.cs" company="Bijectiv">
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
//   Defines the EnumerableFactoryRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a registration for a <see cref="IEnumerableFactory"/>.
    /// </summary>
    public class EnumerableFactoryRegistration
    {
        /// <summary>
        /// The interface type that implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        private readonly Type interfaceType;

        /// <summary>
        /// The concrete type that implements <see cref="ICollection{T}"/>.
        /// </summary>
        private readonly Type concreteType;

        /// <summary>
        /// Initialises a new instance of the <see cref="EnumerableFactoryRegistration"/> class.
        /// </summary>
        /// <param name="interfaceType">
        /// The interface type that implements <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <param name="concreteType">
        /// The concrete type that implements <see cref="ICollection{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="interfaceType"/> is not a monad.
        /// Thrown when <paramref name="concreteType"/> is not a monad.
        /// Thrown when <paramref name="interfaceType"/>does not implement <see cref="IEnumerable{T}"/>.
        /// Thrown when <paramref name="concreteType"/> does not implement <see cref="ICollection{T}"/>.
        /// Thrown when <paramref name="interfaceType"/> is not assignable from <paramref name="concreteType"/>.
        /// </exception>
        public EnumerableFactoryRegistration([NotNull] Type interfaceType, [NotNull] Type concreteType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }

            if (concreteType == null)
            {
                throw new ArgumentNullException("concreteType");
            }

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not an interface.", interfaceType),
                    "interfaceType");
            }

            if (!interfaceType.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not generic.", interfaceType),
                    "interfaceType");
            }

            if (interfaceType.GetGenericArguments().Count() != 1)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not a monad.", interfaceType),
                    "interfaceType");
            }

            if (!concreteType.IsClass)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not a class.", concreteType),
                    "concreteType");
            }

            if (!concreteType.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not generic.", concreteType),
                    "concreteType");
            }

            if (concreteType.GetGenericArguments().Count() != 1)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not a monad.", concreteType),
                    "concreteType");
            }

            var genericInterfaceType = interfaceType.GetGenericTypeDefinition();
            if (!typeof(IEnumerable<Placeholder>)
                .IsAssignableFrom(genericInterfaceType.MakeGenericType(typeof(Placeholder))))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is does not implement IEnumerable<>.", genericInterfaceType),
                    "interfaceType");
            }

            var genericConcreteType = concreteType.GetGenericTypeDefinition();
            if (!typeof(ICollection<Placeholder>)
                .IsAssignableFrom(genericConcreteType.MakeGenericType(typeof(Placeholder))))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is does not implement ICollection<>.", genericConcreteType),
                    "concreteType");
            }

            if (!genericInterfaceType
                    .MakeGenericType(typeof(Placeholder))
                    .IsAssignableFrom(genericConcreteType.MakeGenericType(typeof(Placeholder))))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not assignable from type '{1}'", genericInterfaceType, genericConcreteType),
                    "concreteType");
            }

            this.interfaceType = genericInterfaceType;
            this.concreteType = genericConcreteType;
        }

        /// <summary>
        /// Gets the interface type that implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        public Type InterfaceType
        {
            get { return this.interfaceType; }
        }

        /// <summary>
        /// Gets the concrete type that implements <see cref="ICollection{T}"/>.
        /// </summary>
        public Type ConcreteType
        {
            get { return this.concreteType; }
        }
    }
}