﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredicateConditionMemberShard.cs" company="Bijectiv">
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
//   Defines the PredicateConditionMemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// A member shard that contains a predicate that decides whether or not to inject the member it references.
    /// </summary>
    public class PredicateConditionMemberShard : MemberShard
    {
        /// <summary>
        /// The calculated predicate type; this being the type calculated from the <see cref="MemberShard.Source"/> and
        /// <see cref="MemberShard.Target"/> types.
        /// </summary>
        private readonly Type predicateType;

        /// <summary>
        /// The predicate.
        /// </summary>
        private readonly object predicate;

        /// <summary>
        /// Initialises a new instance of the <see cref="PredicateConditionMemberShard"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="member">
        /// The member on the target type to which the shard refers.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the predicate is not an instance of the expected type.
        /// </exception>
        public PredicateConditionMemberShard(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] MemberInfo member,
            [NotNull] object predicate)
            : base(source, target, member)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var parametersType = typeof(IInjectionParameters<,>).MakeGenericType(source, target);
            this.predicateType = typeof(Func<,>).MakeGenericType(parametersType, typeof(bool));
            if (!this.PredicateType.IsInstanceOfType(predicate))
            {
                throw new ArgumentException(
                    string.Format(
                        "The predicate's type '{0}' is not an instance of the expected type '{1}'",
                        predicate.GetType(),
                        this.PredicateType));

                // Consider also the possibility that the parameters are weakly typed; functionally this has less
                // importance, so can wait for a later version.
            }

            this.predicate = predicate;
        }

        /// <summary>
        /// Gets the shard category.
        /// </summary>
        public override Guid ShardCategory
        {
            get { return LegendaryShards.Condition; }
        }

        /// <summary>
        /// Gets the predicate.
        /// </summary>
        public object Predicate
        {
            get { return this.predicate; }
        }

        /// <summary>
        /// Gets the calculated predicate type; this being the type calculated from the 
        /// <see cref="MemberShard.Source"/> and <see cref="MemberShard.Target"/> types.
        /// </summary>
        public Type PredicateType
        {
            get { return this.predicateType; }
        }
    }
}