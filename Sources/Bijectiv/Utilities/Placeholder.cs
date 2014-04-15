// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Placeholder.cs" company="Bijectiv">
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
//   Defines the Placeholder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    /// <summary>
    /// Represents a type placeholder in an expression or 'any old type' in a generic.
    /// </summary>
    public static class Placeholder
    {
        /// <summary>
        /// A dummy method that has return type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the placeholder to create.
        /// </typeparam>
        /// <returns>
        /// An arbitrary object that can be cast to type <typeparamref name="T"/>.
        /// </returns>
        public static T Of<T>()
        {
            return default(T);
        }
    }
}