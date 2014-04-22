// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSourceMemberStrategy.cs" company="Bijectiv">
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
//   Defines the NullSourceMemberStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A <see cref="ISourceMemberStrategy"/> that handles every member and sets it to null.
    /// </summary>
    public class NullSourceMemberStrategy : ISourceMemberStrategy
    {
        /// <summary>
        /// Tries to gets the source <see cref="MemberInfo"/> that will identified with 
        /// <paramref name="targetMember"/>.
        /// </summary>
        /// <param name="sourceMembers">
        /// The collection of all source members from which <paramref name="sourceMember"/> can be chosen.
        /// </param>
        /// <param name="targetMember">
        /// The target member with which to identify one of <paramref name="sourceMembers"/>.
        /// </param>
        /// <param name="sourceMember">
        /// The source member with which <paramref name="targetMember"/> is identified. If this parameter is 
        /// <c>null</c> then <paramref name="targetMember"/> is not identified with any source member under this 
        /// strategy.
        /// </param>
        /// <returns>
        /// A value indicating whether the strategy was successful.
        /// If the result is <c>true</c> then <paramref name="targetMember"/> will be identified with
        /// <paramref name="sourceMember"/>; <c>otherwise</c> further strategies will be considered.
        /// </returns>
        public bool TryGetSourceForTarget(
            IEnumerable<MemberInfo> sourceMembers, MemberInfo targetMember, out MemberInfo sourceMember)
        {
            sourceMember = null;
            return true;
        }
    }
}