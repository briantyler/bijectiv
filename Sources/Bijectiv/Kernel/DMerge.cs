﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DMerge.cs" company="Bijectiv">
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
//   Defines the signature of a merge delegate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    /// <summary>
    /// Defines the signature of a merge delegate.
    /// </summary>
    /// <param name="source">
    /// The source object.
    /// </param>
    /// <param name="target">
    /// The target object.
    /// </param>
    /// <param name="context">
    /// The context in which the injection will take place.
    /// </param>
    /// <param name="hint">
    /// A hint that can be used to pass additional information to the injection.
    /// </param>
    /// <param name="injection">
    /// The injection itself that wraps delegate.
    /// </param>
    /// <returns>
    /// A <see cref="IMergeResult"/> representing the result of the merge.
    /// </returns>
    public delegate IMergeResult DMerge(
        object source, object target, IInjectionContext context, object hint, IInjection injection);
}