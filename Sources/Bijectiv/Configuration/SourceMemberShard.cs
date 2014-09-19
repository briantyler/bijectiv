// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberShard.cs" company="Bijectiv">
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
//   Defines the SourceMemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// The base class for <see cref="MemberShard"/>s that describe a member source.
    /// </summary>
    public abstract class SourceMemberShard : MemberShard
    {
        /// <summary>
        /// A value indicating whether to inject the source; when false the source will be assigned to the target 
        /// member, not injected. 
        /// </summary>
        private readonly bool inject;

        /// <summary>
        /// Initialises a new instance of the <see cref="SourceMemberShard"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="member">
        /// The member.
        /// </param>
        /// <param name="inject">
        /// A value indicating whether to inject the source. When <see langref="true"/> the source is injected; 
        /// otherwise it is assigned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        protected SourceMemberShard(
            [NotNull] Type source, [NotNull] Type target, [NotNull] MemberInfo member, bool inject)
            : base(source, target, member)
        {
            this.inject = inject;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SourceMemberShard"/> class.
        /// </summary>
        protected SourceMemberShard()
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
        /// Gets a value indicating whether to inject the source; when false the source will be assigned to the target 
        /// member, not injected. 
        /// </summary>
        public virtual bool Inject
        {
            get { return this.inject; }
        }
    }
}