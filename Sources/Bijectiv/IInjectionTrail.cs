﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionTrail.cs" company="Bijectiv">
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
//   Defines the IInjectionTrail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a trail of injection operations.
    /// </summary>
    public interface IInjectionTrail : IEnumerable<InjectionTrailItem>
    {
        /// <summary>
        /// Adds a new item to the trail.
        /// </summary>
        /// <param name="item">
        /// The item to add to the trail.
        /// </param>
        /// <returns>
        /// A value indicating whether the target was seen for the first time.
        /// </returns>
        bool Add([NotNull] InjectionTrailItem item);

        /// <summary>
        /// Gets a value indicating whether <paramref name="target"/> has been added as the target of a 
        /// <see cref="InjectionTrailItem"/>.
        /// </summary>
        /// <param name="target">
        /// The target to check.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="target"/> has been added as the target of a
        /// <see cref="InjectionTrailItem"/>.
        /// </returns>
        bool ContainsTarget(object target);
    }
}