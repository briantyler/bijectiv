// --------------------------------------------------------------------------------------------------------------------
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
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// A member shard that contains a predicate that decides whether or not to inject the member it references.
    /// </summary>
    public class PredicateConditionMemberShard : MemberShard
    {
        /// <summary>
        /// The type of the predicate's parameter.
        /// </summary>
        private readonly Type predicateParameterType;

        /// <summary>
        /// The predicate.
        /// </summary>
        private readonly Delegate predicate;

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
            [NotNull] Delegate predicate)
            : base(source, target, member)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            if (predicate.Method.ReturnType != typeof(bool))
            {
                throw new ArgumentException(
                    string.Format("The predicate must return 'bool' but returns '{0}'", predicate.Method.ReturnType));
            }

            var parameters = predicate.Method.GetParameters();
            if (parameters.Count() != 1)
            {
                throw new ArgumentException(
                    string.Format(
                    "Delegate with single parameter expected, but {0} parameters found",
                    parameters.Count()));
            }

            var maximalParametersType = typeof(IInjectionParameters<,>).MakeGenericType(source, target);
            if (!parameters[0].ParameterType.IsAssignableFrom(maximalParametersType))
            {
                throw new ArgumentException(
                    string.Format(
                        "The parameter '{0}' to the predicate the is not assignable from '{1}'.",
                        parameters[0].ParameterType,
                        maximalParametersType),
                    "predicate");
            }

            this.predicateParameterType = parameters[0].ParameterType;
            this.predicate = predicate;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PredicateConditionMemberShard"/> class.
        /// </summary>
        protected PredicateConditionMemberShard()
        {
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
        public virtual Delegate Predicate
        {
            get { return this.predicate; }
        }

        /// <summary>
        /// Gets the type of the predicate parameter.
        /// </summary>
        public virtual Type PredicateParameterType
        {
            get { return this.predicateParameterType; }
        }
    }
}