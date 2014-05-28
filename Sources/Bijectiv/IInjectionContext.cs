// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionContext.cs" company="Bijectiv">
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
//   Defines the IInjectionContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents an <see cref="IInjection"/> context.
    /// </summary>
    public interface IInjectionContext
    {
        /// <summary>
        /// Gets the culture in which the injection is taking place. Defaults to 
        /// <see cref="CultureInfo.InvariantCulture"/> when not explicitly set.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets the target cache.
        /// </summary>
        ITargetCache TargetCache { get; }

        /// <summary>
        /// Gets the <see cref="IInjection"/> store.
        /// </summary>
        IInjectionStore InjectionStore { get; }

        /// <summary>
        /// Gets the <see cref="ITargetFinder"/> store.
        /// </summary>
        ITargetFinderStore TargetFinderStore { get; }

        /// <summary>
        /// Retrieve a service from the default factory.
        /// </summary>
        /// <param name="service">
        /// The service to retrieve.
        /// </param>
        /// <returns>
        /// The component instance that provides the service.
        /// </returns>
        object Resolve(Type service);
    }
}