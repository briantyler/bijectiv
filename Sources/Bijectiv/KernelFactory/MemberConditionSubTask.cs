// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberConditionSubtask.cs" company="Bijectiv">
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
//   Defines the MemberConditionSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a task that is performed as part of processing a <see cref="MemberFragment"/> that conditionally
    /// injects the member based on the result of some predicate.
    /// </summary>
    public class MemberConditionSubtask : IInjectionSubtask<MemberFragment>
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute(InjectionScaffold scaffold, MemberFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var shards = fragment.UnprocessedShards.OfType<PredicateConditionMemberShard>().ToArray();
            var shard = shards.FirstOrDefault();
            if (shard != null)
            {
                this.ProcessShard(scaffold, fragment, shard);
            }

            fragment.ProcessedShards.AddRange(shards);
        }

        /// <summary>
        /// Processes a <see cref="PredicateConditionMemberShard"/> for the <paramref name="fragment"/> on the 
        /// <paramref name="scaffold"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the injection is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this sub-task is being executed..
        /// </param>
        /// <param name="shard">
        /// The shard that describes the predicate condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any argument is null.
        /// </exception>
        public virtual void ProcessShard(
            [NotNull] InjectionScaffold scaffold, 
            [NotNull] MemberFragment fragment,
            [NotNull] PredicateConditionMemberShard shard)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            var label = scaffold.GetLabel(fragment, LegendaryLabels.End);
            var parameters = scaffold.Variables.First(candidate => candidate.Name == "injectionParameters");

            var castParameters = Expression.Convert(parameters, shard.PredicateParameterType);
            var delegateType = shard.Predicate.GetType();
            var method = delegateType.GetMethod("Invoke");

            var @delegate = Expression.Constant(shard.Predicate, delegateType);
            var negatePredicate = Expression.Not(Expression.Call(@delegate, method, new Expression[] { castParameters }));

            scaffold.Expressions.Add(Expression.IfThen(negatePredicate, Expression.Goto(label)));
        }
    }
}