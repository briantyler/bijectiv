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
    using System.Reflection;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="AutoInjectionTask"/> implementation detail.
    /// </summary>
    public class AutoInjectionTaskDetail
    {
        /// <summary>
        /// The target member injector.
        /// </summary>
        private readonly ITargetMemberInjector injector;

        /// <summary>
        /// A value indicating whether the implementation detail is for a merge injection.
        /// </summary>
        private readonly bool isMerge;

        /// <summary>
        /// Initialises a new instance of the <see cref="AutoInjectionTaskDetail"/> class.
        /// </summary>
        /// <param name="injector">
        /// The target member injector.
        /// </param>
        /// <param name="isMerge">
        /// A value indicating whether the implementation detail is for a merge injection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public AutoInjectionTaskDetail([NotNull] ITargetMemberInjector injector, bool isMerge)
        {
            if (injector == null)
            {
                throw new ArgumentNullException("injector");
            }

            this.injector = injector;
            this.isMerge = isMerge;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AutoInjectionTaskDetail"/> class.
        /// </summary>
        protected AutoInjectionTaskDetail()
        {
        }

        /// <summary>
        /// Gets the target member injector.
        /// </summary>
        public virtual ITargetMemberInjector Injector
        {
            get { return this.injector; }
        }

        /// <summary>
        /// Gets a value indicating whether the implementation detail is for a merge injection.
        /// </summary>
        public virtual bool IsMerge
        {
            get { return this.isMerge; }
        }

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

            if (this.IsMerge)
            {
                this.Injector.AddMergeExpressionToScaffold(
                    scaffold,
                    targetMember,
                    sourceMember.GetAccessExpression(scaffold.Source));
            }
            else
            {
                this.Injector.AddTransformExpressionToScaffold(
                    scaffold,
                    targetMember,
                    sourceMember.GetAccessExpression(scaffold.Source));
            }

            scaffold.ProcessedTargetMembers.Add(targetMember);
        }
    }
}