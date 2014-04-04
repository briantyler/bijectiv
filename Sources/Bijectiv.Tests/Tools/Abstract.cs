// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Abstract.cs" company="Bijectiv">
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
//   Defines the Abstract type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Tools
{
    using Moq;

    /// <summary>
    /// Abstract class helpers.
    /// </summary>
    public class Abstract
    {
        /// <summary>
        /// Creates an instance of an abstract type.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The abstract type.
        /// </typeparam>
        /// <param name="args">
        /// The constructor parameters.
        /// </param>
        /// <returns>
        /// An instance of the abstract type.
        /// </returns>
        public static TInstance Instance<TInstance>(params object[] args)
            where TInstance : class
        {
            return new Mock<TInstance>(args) { CallBase = true }.Object;
        } 
    }
}