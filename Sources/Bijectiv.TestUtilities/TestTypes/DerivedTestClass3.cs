﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DerivedTestClass3.cs" company="Bijectiv">
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
//   Defines the DerivedTestClass3 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities.TestTypes
{
    using System;

    /// <summary>
    /// The derived test class 3.
    /// </summary>
    public class DerivedTestClass3 : BaseTestClass3
    {
        /// <summary>
        /// The type of the class.
        /// </summary>
        public static new readonly Type T = typeof(DerivedTestClass3);

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public override int Value { get; set; }

        /// <summary>
        /// Gets or sets the measure.
        /// </summary>
        public new double Measure { get; set; }
    }
}