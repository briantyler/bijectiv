﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegendaryShards.cs" company="Bijectiv">
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
//   Defines the LegendaryShards type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;

    /// <summary>
    /// Shards that are known throughout the library.
    /// </summary>
    /// <remarks>
    /// Back when 8-bit was cool b113c714 == bijectiv.
    /// </remarks>
    public static class LegendaryShards
    {
        /// <summary>
        /// Represents a member shard that creates a condition on the member.
        /// </summary>
        public static readonly Guid Condition = new Guid("b113c714-90BC-4586-98FA-90CF177CCA94");

        /// <summary>
        /// Represents a member shard that provides the source for a member.
        /// </summary>
        public static readonly Guid Source = new Guid("b113c714-8704-44C8-A8E6-4C56524AC9B1");
    }
}