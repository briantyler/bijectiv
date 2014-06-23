// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberFragment.cs" company="Bijectiv">
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
//   Defines the MemberFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an instruction that determines how a member will be injected.
    /// </summary>
    public class MemberFragment : InjectionFragment, IEnumerable<MemberShard>
    {
        /// <summary>
        /// The shards that comprise the member fragment.
        /// </summary>
        private readonly List<MemberShard> shards = new List<MemberShard>();

        /// <summary>
        /// The member on the target to inject into.
        /// </summary>
        private readonly MemberInfo member;

        /// <summary>
        /// The shards processed that have already been processed.
        /// </summary>
        private readonly ISet<MemberShard> processedShards = new HashSet<MemberShard>();

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberFragment"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="member">
        /// The member on the target type to inject into.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public MemberFragment([NotNull] Type source, [NotNull] Type target, [NotNull] MemberInfo member)
            : base(source, target)
        {
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

            this.member = member;
        }

        /// <summary>
        /// Gets a value indicating whether the fragment is inherited.
        /// </summary>
        public override bool Inherited
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the fragment category.
        /// </summary>
        public override Guid FragmentCategory
        {
            get { return LegendaryFragments.Member; }
        }

        /// <summary>
        /// Gets the member on the target type to inject into.
        /// </summary>
        public virtual MemberInfo Member
        {
            get { return this.member; }
        }

        /// <summary>
        /// Gets the shards that have not been processed.
        /// </summary>
        public virtual IEnumerable<MemberShard> UnprocessedShards
        {
            get { return this.Reverse().Where(candidate => !this.ProcessedShards.Contains(candidate)).ToArray(); }
        }

        /// <summary>
        /// Gets the shards processed that have already been processed.
        /// </summary>
        public virtual ISet<MemberShard> ProcessedShards
        {
            get { return this.processedShards; }
        }

        /// <summary>
        /// Adds a shard to the fragment.
        /// </summary>
        /// <param name="shard">
        /// The shard to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Add([NotNull] MemberShard shard)
        {
            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            if (shard.Source != this.Source)
            {
                throw new ArgumentException(
                    string.Format("Shard Source '{0}' != Fragment Source '{1}'", shard.Source, this.Source),
                    "shard");
            }

            if (shard.Target != this.Target)
            {
                throw new ArgumentException(
                    string.Format("Shard Target '{0}' != Fragment Target '{1}'", shard.Target, this.Target),
                    "shard");
            }

            if (shard.Member != this.Member)
            {
                throw new ArgumentException(
                    string.Format("Shard Member '{0}' != Fragment Member '{1}'", shard.Member, this.Member),
                    "shard");
            }

            this.shards.Add(shard);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the shards in the fragment.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the shards in the collection.
        /// </returns>
        public IEnumerator<MemberShard> GetEnumerator()
        {
            return this.shards.GetEnumerator();
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the shards in the fragment.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the shards in the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.shards.GetEnumerator();
        }
    }
}