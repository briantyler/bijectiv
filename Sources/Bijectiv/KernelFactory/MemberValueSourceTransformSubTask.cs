﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberValueSourceTransformSubtask.cs" company="Bijectiv">
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
//   Defines the MemberValueSourceTransformSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A subtask that transforms a value into a member.
    /// </summary>
    public class MemberValueSourceTransformSubtask 
        : SingleInstanceShardCategorySubtask<ValueSourceMemberShard>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MemberValueSourceTransformSubtask"/> class.
        /// </summary>
        public MemberValueSourceTransformSubtask()
            : base(LegendaryShards.Source)
        {
        }

        /// <summary>
        /// Processes a shard.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <param name="shard">
        /// The shard to process.
        /// </param>
        public override void ProcessShard(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberFragment fragment,
            [NotNull] ValueSourceMemberShard shard)
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

            var sourceType = shard.Value.GetType();
            var targetMemberType = fragment.Member.GetReturnType();

            var expression = ((Expression<Action>)(() =>
                Placeholder.Of<IInjectionContext>("context")
                            .InjectionStore.Resolve<ITransform>(sourceType, targetMemberType)
                            .Transform(shard.Value, Placeholder.Of<IInjectionContext>("context"), null))).Body;

            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);
            expression = Expression.Assign(
                fragment.Member.GetAccessExpression(scaffold.Target),
                Expression.Convert(expression, targetMemberType));

            scaffold.Expressions.Add(expression);
        }
    }
}