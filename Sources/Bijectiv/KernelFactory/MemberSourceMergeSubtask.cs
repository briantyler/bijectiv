// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberSourceMergeSubtask.cs" company="Bijectiv">
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
//   Defines the MemberSourceMergeSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public abstract class MemberSourceMergeSubtask<TShard> : SingleInstanceShardCategorySubtask<TShard>
        where TShard : MemberShard
    {
        protected MemberSourceMergeSubtask()
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
            [NotNull] TShard shard)
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

            var targetMemberSource = this.GetMemberSource(scaffold, shard);
            
            this.AddMergeExpressionToScaffold(scaffold, fragment, targetMemberSource);
        }

        protected abstract Expression GetMemberSource(InjectionScaffold scaffold, TShard shard);

        protected internal virtual void AddMergeExpressionToScaffold(
            InjectionScaffold scaffold,
            MemberFragment fragment,
            Expression targetMemberSource)
        {
            var targetMemberType = fragment.Member.GetReturnType();

            var memberSource = Expression.Variable(typeof(object));
            
            var merge = ((Expression<Func<IMergeResult>>)(() =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<IMerge>(
                        Placeholder.Of<object>("memberSource").GetType(),
                        Placeholder.Of<object>("targetMember") == null ? targetMemberType : Placeholder.Of<object>("targetMember").GetType())
                    .Merge(
                        Placeholder.Of<object>("memberSource"),
                        Placeholder.Of<object>("targetMember"),
                        Placeholder.Of<IInjectionContext>("context"),
                        null))).Body;

            var targetMember = fragment.Member.GetAccessExpression(scaffold.Target);

            merge = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(merge);
            merge = new PlaceholderExpressionVisitor("memberSource", memberSource).Visit(merge);
            merge = new PlaceholderExpressionVisitor("targetMember", Expression.Convert(targetMember, typeof(object))).Visit(merge);

            var mergeResult = Expression.Variable(typeof(IMergeResult));

            var replaceTarget = ((Expression<Func<bool>>)(
                () => Placeholder.Of<IMergeResult>("mergeResult").Action == PostMergeAction.Replace)).Body;
            replaceTarget = new PlaceholderExpressionVisitor("mergeResult", mergeResult).Visit(replaceTarget);

            var mergeTarget = ((Expression<Func<object>>)(() => Placeholder.Of<IMergeResult>("mergeResult").Target)).Body;
            mergeTarget = new PlaceholderExpressionVisitor("mergeResult", mergeResult).Visit(mergeTarget);

            var block = Expression.Block(
                new[] { memberSource,  mergeResult },
                Expression.Assign(memberSource, Expression.Convert(targetMemberSource, typeof(object))),
                Expression.Assign(mergeResult, merge),
                Expression.IfThen(replaceTarget, Expression.Assign(targetMember, Expression.Convert(mergeTarget, targetMemberType))));

            scaffold.Expressions.Add(block);
        }
    }
}