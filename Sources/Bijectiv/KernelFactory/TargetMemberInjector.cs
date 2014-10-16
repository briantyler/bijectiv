// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMemberInjector.cs" company="Bijectiv">
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
//   Defines the TargetMemberInjector type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    /// <summary>
    /// Represents a service that augments the injection that is currently being built with an expression 
    /// that injects a source expression into a target member.
    /// </summary>
    public class TargetMemberInjector : ITargetMemberInjector
    {
        /// <summary>
        /// Adds an expression to the scaffold that transforms the <paramref name="sourceExpression"/> into the 
        /// referenced <paramref name="member"/> of the <paramref name="scaffold"/> target.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="member">
        /// The member in the target that is the target of the transform.
        /// </param>
        /// <param name="sourceExpression">
        /// The source expression that will be transformed.
        /// </param>
        public virtual void AddTransformExpressionToScaffold(
            InjectionScaffold scaffold,
            MemberInfo member,
            Expression sourceExpression)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (sourceExpression == null)
            {
                throw new ArgumentNullException("sourceExpression");
            }

            if (!member.CanRead() && !member.CanWrite())
            {
                throw new ArgumentException(
                    string.Format("Member '{0}' has no accessors. It must be ignored by the injection.", member),
                    "member");
            }

            if (!member.CanWrite())
            {
                this.AddMergeExpressionToScaffold(scaffold, member, sourceExpression);
                return;
            }

            var targetMemberType = member.GetReturnType();

            var memberSource = Expression.Variable(typeof(object));
            var assignMemberSource = Expression.Assign(
                memberSource, Expression.Convert(sourceExpression, typeof(object)));

            var transform = ((Expression<Action>)(() =>
                 Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(
                        Placeholder.Of<Type>("memberSourceType"),
                        targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("memberSource"),
                        Placeholder.Of<IInjectionContext>("context"),
                        null))).Body;

            transform = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(transform);
            transform = new PlaceholderExpressionVisitor("memberSource", memberSource).Visit(transform);
            transform = new PlaceholderExpressionVisitor(
                "memberSourceType", CreateTypeExpression(memberSource, sourceExpression.Type)).Visit(transform);

            var accessExpression = member.GetAccessExpression(scaffold.Target);
            var assignTransformToMember = Expression.Assign(accessExpression, Expression.Convert(transform, targetMemberType));

            scaffold.Expressions.Add(Expression.Block(new[] { memberSource }, assignMemberSource, assignTransformToMember));
        }

        /// <summary>
        /// Adds an expression to the scaffold that merges the <paramref name="sourceExpression"/> into the 
        /// referenced <paramref name="member"/> of the <paramref name="scaffold"/> target.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="member">
        /// The member in the target that is the target of the merge.
        /// </param>
        /// <param name="sourceExpression">
        /// The source expression that will be merged.
        /// </param>
        public virtual void AddMergeExpressionToScaffold(
            InjectionScaffold scaffold,
            MemberInfo member,
            Expression sourceExpression)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (sourceExpression == null)
            {
                throw new ArgumentNullException("sourceExpression");
            }

            if (!member.CanWrite() && !member.CanRead())
            {
                throw new ArgumentException(
                    string.Format("Member '{0}' has no accessors. It must be ignored by the injection.", member),
                    "member");
            }

            if (!member.CanRead())
            {
                this.AddTransformExpressionToScaffold(scaffold, member, sourceExpression);
                return;
            }

            var memberSource = Expression.Variable(typeof(object));

            var merge = ((Expression<Func<IMergeResult>>)(() =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<IMerge>(
                        Placeholder.Of<Type>("memberSourceType"),
                        Placeholder.Of<Type>("targetMemberType"))
                    .Merge(
                        Placeholder.Of<object>("memberSource"),
                        Placeholder.Of<object>("targetMember"),
                        Placeholder.Of<IInjectionContext>("context"),
                        null))).Body;

            var targetMember = member.GetAccessExpression(scaffold.Target);

            merge = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(merge);
            merge = new PlaceholderExpressionVisitor("memberSource", memberSource).Visit(merge);
            merge = new PlaceholderExpressionVisitor(
                "memberSourceType", CreateTypeExpression(memberSource, sourceExpression.Type)).Visit(merge);
            merge = new PlaceholderExpressionVisitor(
                "targetMember", Expression.Convert(targetMember, typeof(object))).Visit(merge);
            merge = new PlaceholderExpressionVisitor(
                "targetMemberType", CreateTypeExpression(targetMember, member.GetReturnType())).Visit(merge);

            var mergeResult = Expression.Variable(typeof(IMergeResult));

            var replaceTarget = ((Expression<Func<bool>>)(
                () => Placeholder.Of<IMergeResult>("mergeResult").Action == PostMergeAction.Replace)).Body;
            replaceTarget = new PlaceholderExpressionVisitor("mergeResult", mergeResult).Visit(replaceTarget);

            if (!member.CanWrite())
            {
                replaceTarget = Expression.Throw(
                    Expression.Constant(
                        new InvalidOperationException(
                            string.Format(
                                "Unable to merge into member '{0}'. The member is read-only, but a replace-merge is required. "
                                + "Either make the member writeable, or if that is not possible, or desirable ignore the "
                                + "member in the injection configuration.",
                                member))));
            }

            var mergeTarget = ((Expression<Func<object>>)(() => Placeholder.Of<IMergeResult>("mergeResult").Target)).Body;
            mergeTarget = new PlaceholderExpressionVisitor("mergeResult", mergeResult).Visit(mergeTarget);
            var convertedMergeTarget = Expression.Convert(mergeTarget, member.GetReturnType());

            var block = Expression.Block(
                new[] { memberSource, mergeResult },
                Expression.Assign(memberSource, Expression.Convert(sourceExpression, typeof(object))),
                Expression.Assign(mergeResult, merge),
                Expression.IfThen(replaceTarget, Expression.Assign(targetMember, convertedMergeTarget)));

            scaffold.Expressions.Add(block);
        }

        /// <summary>
        /// The create type expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="fallback">
        /// The most derived type that the any result of <paramref name="expression"/> can be.
        /// </param>
        /// <returns>
        /// An expression that returns the type of the result of <paramref name="expression"/>.
        /// </returns>
        private static Expression CreateTypeExpression(Expression expression, Type fallback)
        {
            if (fallback.IsValueType || fallback.IsSealed)
            {
                return Expression.Constant(fallback);
            }

            Expression<Func<object, Type>> x = o => o == null ? fallback : o.GetType();
            return new ParameterExpressionVisitor(
                x.Parameters.Single(), Expression.Convert(expression, typeof(object))).Visit(x.Body);
        }
    }
}