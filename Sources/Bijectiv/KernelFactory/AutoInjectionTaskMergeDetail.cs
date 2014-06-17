// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskMergeDetail.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskMergeDetail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IMerge"/> specific <see cref="AutoInjectionTask"/> implementation detail.
    /// </summary>
    public class AutoInjectionTaskMergeDetail : AutoInjectionTaskDetail
    {
        /// <summary>
        /// An expression that performs a merge from a <see cref="IInjectionContext"/>.
        /// </summary>
        private static readonly Expression MergeExpression = ((Expression<Func<IMergeResult>>)(
            () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<IMerge>(
                        Placeholder.Of<Type>("sourceMemberType"),
                        Placeholder.Of<Type>("targetMemberType")).Merge(
                            Placeholder.Of<object>("sourceMember"), 
                            Placeholder.Of<object>("targetMember"),
                            Placeholder.Of<IInjectionContext>("context"), 
                            null))).Body;

        /// <summary>
        /// A <see cref="IMergeResult.Target"/> variable.
        /// </summary>
        private static readonly ParameterExpression ResultVariable = Expression.Variable(typeof(IMergeResult));

        /// <summary>
        /// An expression that accesses <see cref="IMergeResult.Target"/>.
        /// </summary>
        private static readonly Expression ResultTargetExpression = ((Expression<Func<object>>)(
            () => Placeholder.Of<IMergeResult>("result").Target)).Body;

        /// <summary>
        /// An expression that decides whether <see cref="IMergeResult.Action"/> is 
        /// <see cref="PostMergeAction.Replace"/>.
        /// </summary>
        private static readonly Expression IsReplaceActionExpression = ((Expression<Func<bool>>)(
            () => Placeholder.Of<IMergeResult>("result").Action == PostMergeAction.Replace)).Body;

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
        protected internal override Expression CreateExpression(
            InjectionScaffold scaffold,
            MemberInfo sourceMember, 
            MemberInfo targetMember)
        {
            /* Creates an expression that is equivalent to:
             *  
             *  var merge = store.Resolve<IMerge>(source.Member.Type, target.Member.Type)
             *  var result = merge(source.Member, target.Member)
             *  if (result.Action == Replace)
             *  {
             *      target.Member = result.Target
             *  }
             */

            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (sourceMember == null)
            {
                throw new ArgumentNullException("sourceMember");
            }

            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }
            
            var replaceTargetExpression = Expression.Assign(
                targetMember.GetAccessExpression(scaffold.Target),
                Expression.Convert(ResultTargetExpression, targetMember.GetReturnType()));

            Expression template = Expression.Block(
                new[] { ResultVariable },
                Expression.Assign(ResultVariable, MergeExpression),
                Expression.IfThen(IsReplaceActionExpression, replaceTargetExpression));

            return ReplacePlaceholders(scaffold, sourceMember, targetMember, template);
        }

        /// <summary>
        /// Replaces the placeholders in a template expression.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the injection is being built.
        /// </param>
        /// <param name="sourceMember">
        /// The source member.
        /// </param>
        /// <param name="targetMember">
        /// The target member.
        /// </param>
        /// <param name="template">
        /// The template expression.
        /// </param>
        /// <returns>
        /// The merge <see cref="Expression"/> with their placeholders replaced.
        /// </returns>
        private static Expression ReplacePlaceholders(
            InjectionScaffold scaffold,
            MemberInfo sourceMember,
            MemberInfo targetMember,
            Expression template)
        {
            var sourceMemberType = sourceMember.GetReturnType();
            var targetMemberType = targetMember.GetReturnType();
            var sourceMemberTypeExpression = CreateMemberTypeExpression(sourceMemberType, "sourceMember");
            var targetMemberTypeExpression = CreateMemberTypeExpression(targetMemberType, "targetMember");
            var sourceMemberExpression = Expression.Convert(sourceMember.GetAccessExpression(scaffold.Source), typeof(object));
            var targetMemberExpression = Expression.Convert(targetMember.GetAccessExpression(scaffold.Target), typeof(object));

            template = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(template);
            template = new PlaceholderExpressionVisitor("targetMemberType", targetMemberTypeExpression).Visit(template);
            template = new PlaceholderExpressionVisitor("sourceMemberType", sourceMemberTypeExpression).Visit(template);
            template = new PlaceholderExpressionVisitor("sourceMember", sourceMemberExpression).Visit(template);
            template = new PlaceholderExpressionVisitor("targetMember", targetMemberExpression).Visit(template);
            template = new PlaceholderExpressionVisitor("result", ResultVariable).Visit(template);

            return template;
        }

        /// <summary>
        /// Creates an <see cref="Expression"/> that provides a member <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The member type.
        /// </param>
        /// <param name="name">
        /// The name that identifies the member.
        /// </param>
        /// <returns>
        /// An <see cref="Expression"/> that provides a member <see cref="Type"/>.
        /// </returns>
        private static Expression CreateMemberTypeExpression(Type type, string name)
        {
            return ((type.IsValueType || type.IsSealed)
                ? (Expression<Func<Type>>)(() => type)
                : (() =>
                    (Placeholder.Of<object>(name) == null
                        ? type
                        : Placeholder.Of<object>(name).GetType()))).Body;
        }
    }
}