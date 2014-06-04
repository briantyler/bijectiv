// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactoryInstanceFactory.cs" company="Bijectiv">
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
//   Defines the EnumerableFactoryInstanceFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A <see cref="IInstanceFactory"/> that creates <see cref="IEnumerableFactory"/> instances.
    /// </summary>
    public class EnumerableFactoryInstanceFactory : IInstanceFactory
    {
        /// <summary>
        /// The delegate that creates the base <see cref="IEnumerableFactory"/> .
        /// </summary>
        private readonly Func<IEnumerableFactory> createFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="EnumerableFactoryInstanceFactory"/> class.
        /// </summary>
        /// <param name="createFactory">
        /// The delegate that creates the base <see cref="IEnumerableFactory"/> .
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public EnumerableFactoryInstanceFactory([NotNull] Func<IEnumerableFactory> createFactory)
        {
            if (createFactory == null)
            {
                throw new ArgumentNullException("createFactory");
            }

            this.createFactory = createFactory;
        }

        /// <summary>
        /// Gets the delegate that creates the base <see cref="IEnumerableFactory"/> .
        /// </summary>
        public Func<IEnumerableFactory> CreateFactory
        {
            get { return this.createFactory; }
        }

        /// <summary>
        /// Creates a <see cref="IEnumerableFactory"/> from a <see cref="IInstanceRegistry"/>.
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

            var instance = this.CreateFactory();

            registry.ResolveAll<EnumerableRegistration>().ForEach(instance.Register);

            return new Tuple<Type, object>(typeof(IEnumerableFactory), instance);
        }
    }
}