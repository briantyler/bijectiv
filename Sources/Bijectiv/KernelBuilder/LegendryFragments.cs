// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegendryFragments.cs" company="Bijectiv">
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
//   Defines the LegendryFragments type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelBuilder
{
    using System;

    /// <summary>
    /// Fragments that are known throught the library.
    /// </summary>
    /// <remarks>
    /// Back when 8-bit was cool b113c714 == bijectiv.
    /// </remarks>
    public static class LegendryFragments
    {
        /// <summary>
        /// Represents a fragment that describes inheritance.
        /// </summary>
        public static readonly Guid Inherits = new Guid("b113c714-FE8C-498B-B371-52E468949A04");

        /// <summary>
        /// Represents a fragment that describes construction.
        /// </summary>
        public static readonly Guid Factory = new Guid("b113c714-6576-4D9B-BE31-98E886FC2082");

        /// <summary>
        /// Represents a fragment that describes an auto-injection.
        /// </summary>
        public static readonly Guid AutoInjection = new Guid("b113c714-D396-425B-B0DA-355DA43AD570");

        /// <summary>
        /// Represents a fragment that determines how to treat a <c>null</c> source.
        /// </summary>
        public static readonly Guid NullSource = new Guid("b113c714-21C5-428C-B5CA-84372F03102D");
    }
}