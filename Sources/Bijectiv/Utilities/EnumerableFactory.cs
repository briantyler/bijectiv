// --------------------------------------------------------------------------------------------------------------------
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

namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EnumerableFactory
    {
        private readonly Dictionary<Type, Type> registrations = new Dictionary<Type, Type>
        {
            { typeof(IEnumerable<>), typeof(List<>) },
            { typeof(ICollection<>), typeof(List<>) },
            { typeof(IList<>), typeof(List<>) },
        };

        public IDictionary<Type, Type> Registrations
        {
            get { return this.registrations; }
        }

        public void Register<TInterface, TConcrete>()
            where TInterface : IEnumerable<Placeholder>
            where TConcrete : ICollection<Placeholder>, TInterface, new()
        {
            if (!typeof(TInterface).IsGenericType)
            {
                throw new InvalidOperationException(
                    string.Format("Type TInterface '{0}' is not generic.", typeof(TInterface)));
            }

            if (typeof(TInterface).GetGenericArguments().Count() != 1)
            {
                throw new InvalidOperationException(
                    string.Format("Type TInterface '{0}' is not a monad.", typeof(TInterface)));
            }

            if (!typeof(TConcrete).IsGenericType)
            {
                throw new InvalidOperationException(
                    string.Format("Type TConcrete '{0}' is not generic.", typeof(TConcrete)));
            }

            if (typeof(TConcrete).GetGenericArguments().Count() != 1)
            {
                throw new InvalidOperationException(
                    string.Format("Type TConcrete '{0}' is not a monad.", typeof(TConcrete)));
            }

            this.Registrations[typeof(TInterface).GetGenericTypeDefinition()] = 
                typeof(TConcrete).GetGenericTypeDefinition();
        }

        public object Resolve(Type enumerable)
        {
            if (enumerable.IsClass)
            {
                // It may as well be assumed that any upstream process simply wants an instance of the type it 
                // specified; in all vaguely sane cases this should mean that the caller gets what it wants when
                // asking to resolve a type.
                return Activator.CreateInstance(enumerable);
            }

            if (!enumerable.IsInterface)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not an interface.", enumerable), "enumerable");
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