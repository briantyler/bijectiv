// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceInjectionStoreFactory.cs" company="Bijectiv">
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
//   Defines the InstanceInjectionStoreFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.Builder;

    using JetBrains.Annotations;

    /// <summary>
    /// A <see cref="IInjectionStoreFactory"/> that creates a <see cref="IInjectionStore"/> by returning a
    /// registered instance.
    /// </summary>
    public class InstanceInjectionStoreFactory : IInjectionStoreFactory
    {
        /// <summary>
        /// The <see cref="IInjectionStore"/> instance.
        /// </summary>
        private readonly IInjectionStore instance;

        /// <summary>
        /// Initialises a new instance of the <see cref="InstanceInjectionStoreFactory"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InstanceInjectionStoreFactory([NotNull] IInjectionStore instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this.instance = instance;
        }

        /// <summary>
        /// Gets the <see cref="IInjectionStore"/> instance.
        /// </summary>
        public IInjectionStore Instance
        {
            get { return this.instance; }
        }

        /// <summary>
        /// Returns the registered <see cref="IInjectionStore"/>.
        /// </summary>
        /// <param name="registry">
        /// Not used.
        /// </param>
        /// <returns>
        /// The registered <see cref="IInjectionStore"/>.
        /// </returns>
        public IInjectionStore Create(IInstanceRegistry registry)
        {
            return this.Instance;
        }
    }
}