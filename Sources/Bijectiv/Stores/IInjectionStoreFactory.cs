// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionStoreFactory.cs" company="Bijectiv">
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
//   Defines the IInjectionStoreFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Stores
{
    using Bijectiv.Builder;

    /// <summary>
    /// Represents a factory that creates <see cref="IInjectionStore"/> instances.
    /// </summary>
    public interface IInjectionStoreFactory
    {
        /// <summary>
        /// Creates a <see cref="IInjectionStore"/> from a <see cref="IInstanceRegistry"/>.
        /// </summary>
        /// <param name="registry">
        /// The registry from which to create the store.
        /// </param>
        /// <returns>
        /// The created <see cref="IInjectionStore"/>.
        /// </returns>
        IInjectionStore Create(IInstanceRegistry registry);
    }
}