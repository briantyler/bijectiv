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

namespace Bijectiv.Factory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    /// <summary>
    /// The <see cref="IMerge"/> specific <see cref="AutoInjectionTask"/> implementation detail.
    /// </summary>
    public class AutoInjectionTaskMergeDetail : AutoInjectionTaskTransformDetail
    {
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
             * if (target.Member == null)
             * {
             *     target.Member = transform(source.Member)
             * }
             * else
             * {
             *      var merge = store.Resolve<IMerge>(source.Member.Type, target.Member.Type)
             *      if (merge == null)
             *      {
             *          target.Member = transform(source.Member)
             *      }
             *      else
             *      {
             *          var result = merge(source.Member, target.Member)
             *          if (result.Action == Replace)
             *          {
             *              target.Member = result.Target
             *          }
             *      }
             * }
             */
            
            var sourceMemberType = sourceMember.GetReturnType();
            var targetMemberType = targetMember.GetReturnType();

            var sourceMemberTypeExpression = SourceMemberTypeExpression(sourceMemberType);
            var targetMemberTypeExpression = TargetMemberTypeExpression(targetMemberType);
            var resolveExpression = ResolveExpression();
            var isMergeNullExpression = IsMergeNullExpression();

            Expression<Func<IMergeResult>> performMergeExpression = () =>
                Placeholder.Of<IMerge>("merge").Merge(
                    Placeholder.Of<object>("sourceMember"), 
                    Placeholder.Of<object>("targetMember"),
                    Placeholder.Of<IInjectionContext>("context"));

            Expression<Func<bool>> isTargetMemberNull = () => Placeholder.Of<object>("targetMember") == null;
            
            var mergeVariable = Expression.Variable(typeof(IMerge));
            var assignMerge = Expression.Assign(mergeVariable, resolveExpression.Body);

            var resultVariable = Expression.Variable(typeof(IMergeResult));
            var assignMergeResult = Expression.Assign(resultVariable, performMergeExpression.Body);

            Expression<Func<bool>> isReplaceTargetExpression = () => 
                Placeholder.Of<IMergeResult>("result").Action == PostMergeAction.Replace;

            Expression<Func<object>> resultTargetExpression = () => Placeholder.Of<IMergeResult>("result").Target;

            var conditionalReplaceTargetExpression = Expression.IfThen(
                isReplaceTargetExpression.Body,
                Expression.Assign(targetMember.GetAccessExpression(scaffold.Target), Expression.Convert(resultTargetExpression.Body, targetMemberType)));

            var transformExpression = base.CreateExpression(scaffold, sourceMember, targetMember);

            var postMergeExpression = Expression.Block(
                new[] { resultVariable },
                assignMergeResult,
                Expression.IfThen(isReplaceTargetExpression.Body, conditionalReplaceTargetExpression));

            var mergeExpression = Expression.Block(
                new[] { mergeVariable },
                new Expression[] { assignMerge, Expression.IfThenElse(isMergeNullExpression.Body, transformExpression, postMergeExpression) });

            var sourceMemberExpression = Expression.Convert(sourceMember.GetAccessExpression(scaffold.Source), typeof(object));
            var targetMemberExpression = Expression.Convert(targetMember.GetAccessExpression(scaffold.Target), typeof(object));

            Expression expression = Expression.IfThenElse(
                isTargetMemberNull.Body, 
                transformExpression,
                mergeExpression);

            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);
            expression = new PlaceholderExpressionVisitor("sourceMember", sourceMemberExpression).Visit(expression);
            expression = new PlaceholderExpressionVisitor("targetMember", targetMemberExpression).Visit(expression);
            expression = new PlaceholderExpressionVisitor("targetMemberType", targetMemberTypeExpression.Body).Visit(expression);
            expression = new PlaceholderExpressionVisitor("sourceMemberType", sourceMemberTypeExpression.Body).Visit(expression);
            expression = new PlaceholderExpressionVisitor("merge", mergeVariable).Visit(expression);
            return new PlaceholderExpressionVisitor("result", resultVariable).Visit(expression);
        }

        private static Expression<Func<bool>> IsMergeNullExpression()
        {
            Expression<Func<bool>> isMergeNull = () => Placeholder.Of<IMerge>("merge") == null;
            return isMergeNull;
        }

        private static Expression<Func<IMerge>> ResolveExpression()
        {
            Expression<Func<IMerge>> resolveExpression =
                () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<IMerge>(
                        Placeholder.Of<Type>("sourceMemberType"),
                        Placeholder.Of<Type>("targetMemberType"));
            return resolveExpression;
        }

        private static Expression<Func<Type>> TargetMemberTypeExpression(Type targetMemberType)
        {
            var targetMemberTypeExpression = (targetMemberType.IsValueType || targetMemberType.IsSealed)
                                                 ? (Expression<Func<Type>>)(() => targetMemberType)
                                                 : (() => Placeholder.Of<object>("targetMember").GetType());
            return targetMemberTypeExpression;
        }

        private static Expression<Func<Type>> SourceMemberTypeExpression(Type sourceMemberType)
        {
            var sourceMemberTypeExpression = (sourceMemberType.IsValueType || sourceMemberType.IsSealed)
                                                 ? (Expression<Func<Type>>)(() => sourceMemberType)
                                                 : (() =>
                                                    (Placeholder.Of<object>("sourceMember") == null
                                                         ? sourceMemberType
                                                         : Placeholder.Of<object>("sourceMember").GetType()));
            return sourceMemberTypeExpression;
        }
    }
}