// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMergeResult.cs" company="Bijectiv">
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
//   Defines the IMergeResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    /// <summary>
    /// Represents the result of calling <see cref="IMerge.Merge"/>.
    /// </summary>
    public interface IMergeResult
    {
        /// <summary>
        /// Gets the action that the caller must perform after calling <see cref="IMerge.Merge"/> to complete 
        /// the merge.
        /// </summary>
        /// <remarks>
        /// This is necessary because properties cannot be passed by reference and in the case of a <c>null</c>
        /// source, for example, it might be necessary to set the original target to null. There are two options:
        /// <list type="number">
        ///     <item>
        ///         Pass a callback to <see cref="IMerge.Merge"/> that allows it to set the target on
        ///         the parent instance.
        ///     </item>
        ///     <item>
        ///         Tell the caller what to do (i.e. <em>do nothing</em> or <em>replace target with x</em>) and 
        ///         then trust them to do it.
        ///     </item>
        /// </list>
        /// Neither option is without its drawbacks, but the second option, which is the one that has been chosen, is
        /// significantly less complex and does not have the overhead of needing to generate a large number of lambda
        /// functions at runtime.
        /// </remarks>
        PostMergeAction Action { get; }

        /// <summary>
        /// Gets the merged target.
        /// </summary>
        object Target { get; }
    }
}