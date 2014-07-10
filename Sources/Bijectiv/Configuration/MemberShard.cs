// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberShard.cs" company="Bijectiv">
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
//   Defines the MemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// The base class for shards that comprise the injection of a target member.
    /// </summary>
    public abstract class MemberShard
    {
        /// <summary>
        /// The source type.
        /// </summary>
        private readonly Type source;

        /// <summary>
        /// The target type.
        /// </summary>
        private readonly Type target;

        /// <summary>
        /// The member on the target type to which the shard corresponds.
        /// </summary>
        private readonly MemberInfo member;

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberShard"/> class.
        /// </summary>
        protected MemberShard()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberShard"/> class.
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
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        protected MemberShard([NotNull] Type source, [NotNull] Type target, [NotNull] MemberInfo member)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (member.DeclaringType == null)
            {
                throw new ArgumentException(string.Format("Member '{0}' has no declaring type", member), "member");
            }

            if (!member.DeclaringType.IsAssignableFrom(target))
            {
                throw new ArgumentException(
                    string.Format(
                        "Declaring type '{0}' of member '{1}' is not assignable from target '{2}'",
                        member.DeclaringType,
                        member,
                        target),
                    "member");
            }
            
            this.source = source;
            this.target = target;
            this.member = member;
        }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public Type Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// Gets the source type.
        /// </summary>
        public Type Source
        {
            get { return this.source; }
        }
        
        /// <summary>
        /// Gets the member on the target type to which the shard corresponds.
        /// </summary>
        public MemberInfo Member
        {
            get { return this.member; }
        }

        /// <summary>
        /// Gets the shard category.
        /// </summary>
        public abstract Guid ShardCategory { get; }
    }
}