// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueSourceMemberShard.cs" company="Bijectiv">
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
//   Defines the ValueSourceMemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// A member shard that contains a value from which the member will be injected.
    /// </summary>
    public class ValueSourceMemberShard : MemberShard
    {
        /// <summary>
        /// The value from which to inject the member.
        /// </summary>
        private readonly object value;

        /// <summary>
        /// Initialises a new instance of the <see cref="ValueSourceMemberShard"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="member">
        /// The member on the target type to which the shard corresponds.
        /// </param>
        /// <param name="value">
        /// The value from which to inject the member.
        /// </param>
        public ValueSourceMemberShard(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] MemberInfo member, 
            object value)
            : base(source, target, member)
        {
            this.value = value;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ValueSourceMemberShard"/> class.
        /// </summary>
        protected ValueSourceMemberShard()
        {
        }

        /// <summary>
        /// Gets the shard category.
        /// </summary>
        public override Guid ShardCategory
        {
            get { return LegendaryShards.Source; }
        }

        /// <summary>
        /// Gets the value from which to inject the member.
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }
    }
}