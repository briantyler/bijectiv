// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRegistry.cs" company="Bijectiv">
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
//   Defines the InstanceRegistry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// A registry that contains instances.
    /// </summary>
    public class InstanceRegistry : IInstanceRegistry
    {
        /// <summary>
        /// The registrations.
        /// </summary>
        private readonly IDictionary<Type, IList<object>> registrations = new Dictionary<Type, IList<object>>();

        /// <summary>
        /// Gets the registrations.
        /// </summary>
        public IDictionary<Type, IList<object>> Registrations
        {
            get { return this.registrations; }
        }

        public void Register(Tuple<Type, object> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            this.Register(registration.Item1, registration.Item2);
        }

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
        public void Register(Type instanceType, object instance)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException("instanceType");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!instanceType.IsInstanceOfType(instance))
            {
                throw new ArgumentException(
                    string.Format("Instance '{0}' is not an instance of type '{1}", instance, instanceType), "instance");
            }

            if (!this.Registrations.ContainsKey(instanceType))
            {
                this.Registrations.Add(instanceType, new List<object>());
            }

            this.Registrations[instanceType].Add(instance);
        }

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
        public IEnumerable<TInstance> ResolveAll<TInstance>()
        {
            return !this.Registrations.ContainsKey(typeof(TInstance)) 
                ? new TInstance[0] 
                : this.Registrations[typeof(TInstance)].Cast<TInstance>();
        }

        /// <summary>
        /// Resolves the most recently registered instance of type <typeparamref name="TInstance"/>.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The type of instance to resolve.
        /// </typeparam>
        /// <returns>
        /// The most recently registered instance of type <typeparamref name="TInstance"/>.
        /// </returns>
        public TInstance Resolve<TInstance>()
        {
            if (!this.Registrations.ContainsKey(typeof(TInstance)))
            {
                throw new InvalidOperationException(
                    string.Format("The registry contains no instance of type '{0}'", typeof(TInstance)));
            }

            return (TInstance)this.Registrations[typeof(TInstance)].Last();
        }
    }
}