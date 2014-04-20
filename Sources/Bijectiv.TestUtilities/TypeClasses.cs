// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeClasses.cs" company="Bijectiv">
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
//   Defines the TypeClasses type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities
{
    using System;

    using Bijectiv.TestUtilities.TestTypes;

    /// <summary>
    /// Contains collections that represent classes of types.
    /// </summary>
    public class TypeClasses
    {
        /// <summary>
        /// All types that can be converted to from a <see cref="IConvertible"/>.
        /// </summary>
        public static readonly Type[] ConvertibleTargetTypes =
        {
            typeof(bool), typeof(char), typeof(sbyte),
            typeof(byte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal),
            typeof(DateTime), typeof(string)
        };

        /// <summary>
        /// All in-built types that implement <see cref="IConvertible"/>.
        /// </summary>
        public static readonly Type[] ConvertibleSourceTypes =
        {
            typeof(bool), typeof(char), typeof(sbyte),
            typeof(byte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal),
            typeof(DateTime), typeof(string), typeof(IConvertible)
        };

        /// <summary>
        /// Types that identify themselves as primitive by having a non-object <see cref="TypeCode"/>.
        /// </summary>
        public static readonly Type[] PrimitiveTypes =
        {
            typeof(bool), typeof(char), typeof(sbyte),
            typeof(byte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal),
            typeof(DateTime), typeof(string), typeof(TestEnum1)
        };
    }
}