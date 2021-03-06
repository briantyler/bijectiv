﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactory.cs" company="Bijectiv">
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
//   Defines the EnumerableFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A factory that creates <see cref="IEnumerable"/> instances.
    /// </summary>
    public class EnumerableFactory : IEnumerableFactory
    {
        /// <summary>
        /// The type registrations.
        /// </summary>
        private readonly Dictionary<Type, Type> registrations = new Dictionary<Type, Type>
        {
            { typeof(IEnumerable<>), typeof(List<>) },
            { typeof(ICollection<>), typeof(List<>) },
            { typeof(IList<>), typeof(List<>) },
            { typeof(ISet<>), typeof(HashSet<>) },
        };

        /// <summary>
        /// Gets the type registrations.
        /// </summary>
        public IDictionary<Type, Type> Registrations
        {
            get { return this.registrations; }
        }

        /// <summary>
        /// Registers a <see cref="EnumerableRegistration"/> with the factory.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Register([NotNull] EnumerableRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            this.Registrations[registration.InterfaceType] = registration.ConcreteType;
        }

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
        public object Resolve([NotNull] Type enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            if (enumerable.IsClass)
            {
                // It may as well be assumed that any upstream process simply wants an instance of the type it 
                // specified; in all vaguely sane cases this should mean that the caller gets what it wants when
                // asking to resolve a type.
                return Activator.CreateInstance(enumerable);
            }

            if (enumerable == typeof(IEnumerable))
            {
                enumerable = typeof(IEnumerable<object>);
            }
            else if (enumerable == typeof(ICollection))
            {
                enumerable = typeof(ICollection<object>);
            }
            else if (enumerable == typeof(IList))
            {
                enumerable = typeof(IList<object>);
            }

            if (!enumerable.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not generic.", enumerable), "enumerable");
            }

            var enumerableGeneric = enumerable.GetGenericTypeDefinition();
            Type collectionGeneric;
            if (!this.Registrations.TryGetValue(enumerableGeneric, out collectionGeneric))
            {
                throw new InvalidOperationException(
                    string.Format("Type '{0}' has no known concrete implementation.", enumerableGeneric));
            }

            var collection = collectionGeneric.MakeGenericType(enumerable.GetGenericArguments());
            return Activator.CreateInstance(collection);
        }
    }
}