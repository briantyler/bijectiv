// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskDetail.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskDetail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="AutoInjectionTask"/> implementation detail.
    /// </summary>
    public abstract class AutoInjectionTaskDetail
    {
        /// <summary>
        /// Creates the (source, target) member pairs that will be auto injected.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="strategies">
        /// The strategies to apply.
        /// </param>
        /// <returns>
        /// The collection of (source, target) member pairs that will be auto injected.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public virtual IEnumerable<Tuple<MemberInfo, MemberInfo>> CreateSourceTargetPairs(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] IEnumerable<IAutoInjectionStrategy> strategies)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (strategies == null)
            {
                throw new ArgumentNullException("strategies");
            }

            var enumeratedStrategies = strategies.ToArray();
            foreach (var targetMember in scaffold.UnprocessedTargetMembers)
            {
                foreach (var strategy in enumeratedStrategies)
                {
                    MemberInfo sourceMember;
                    if (!strategy.TryGetSourceForTarget(scaffold.SourceMembers, targetMember, out sourceMember))
                    {
                        continue;
                    }

                    yield return Tuple.Create(sourceMember, targetMember);
                    break;
                }
            }
        }

        /// <summary>
        /// Processes a pair of members into the scaffold.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="pair">
        /// The pair of members.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any argument is null.
        /// </exception>
        public virtual void ProcessPair(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] Tuple<MemberInfo, MemberInfo> pair)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (pair == null)
            {
                throw new ArgumentNullException("pair");
            }

            var sourceMember = pair.Item1;
            var targetMember = pair.Item2;

            var expression = this.CreateExpression(scaffold, sourceMember, targetMember);

            scaffold.Expressions.Add(expression);
            scaffold.ProcessedTargetMembers.Add(targetMember);
        }

        /// <summary>
        /// Creates the member mapping <see cref="Expression"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="sourceMember">
        /// The source member.
        /// </param>
        /// <param name="targetMember">
        /// The target member.
        /// </param>
        /// <returns>
        /// The member mapping <see cref="Expression"/>.
        /// </returns>
        protected internal abstract Expression CreateExpression(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberInfo sourceMember,
            [NotNull] MemberInfo targetMember);
    }
}