// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteringInstanceFactory.cs" company="Bijectiv">
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
//   Defines the RegisteringInstanceFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;

    using Bijectiv.Utilities;

    /// <summary>
    /// A <see cref="IInstanceFactory"/> that creates instances that are initialised by registering all instances
    /// of type <typeparamref name="TRegistration"/> from the <see cref="IInstanceRegistry"/>.
    /// </summary>
    /// <typeparam name="TRegistration">
    /// The type of registration to resolve from the instance registry.
    /// </typeparam>
    public class RegisteringInstanceFactory<TRegistration> : IInstanceFactory
    {
        /// <summary>
        /// The type with which to identify the instance.
        /// </summary>
        private readonly Type instanceType;

        /// <summary>
        /// The delegate that creates the base instance.
        /// </summary>
        private readonly Func<object> instanceFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="RegisteringInstanceFactory{TRegistration}"/> class.
        /// </summary>
        /// <param name="instanceType">
        /// The type with which to identify the instance.
        /// </param>
        /// <param name="instanceFactory">
        /// The delegate that creates the base instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public RegisteringInstanceFactory([NotNull] Type instanceType, [NotNull] Func<object> instanceFactory)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException("instanceType");
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException("instanceFactory");
            }

            this.instanceFactory = instanceFactory;
            this.instanceType = instanceType;
        }

        /// <summary>
        /// Gets the type with which to identify the instance.
        /// </summary>
        public Type InstanceType
        {
            get { return this.instanceType; }
        }

        /// <summary>
        /// Gets the delegate that creates the base instance.
        /// </summary>
        public Func<object> InstanceFactory
        {
            get { return this.instanceFactory; }
        }

        /// <summary>
        /// Creates an instance from a <see cref="IInstanceRegistry"/> that is identified with a given type.
        /// </summary>
        /// <param name="registry">
        /// The registry from which to create the instance.
        /// </param>
        /// <returns>
        /// A <see cref="Tuple{T,O}"/> where the first item is the type with which to identify the instance and the 
        /// second item is the instance.
        /// </returns>
        public Tuple<Type, object> Create([NotNull] IInstanceRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            dynamic instance = this.InstanceFactory();
            registry.ResolveAll<TRegistration>().ForEach(item => instance.Register(item));
            return new Tuple<Type, object>(this.InstanceType, instance);
        }
    }
}