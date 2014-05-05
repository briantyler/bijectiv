// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTransformTaskDetail.cs" company="Bijectiv">
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
//   Defines the AutoTransformTaskDetail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="AutoTransformTask"/> implementation detail.
    /// </summary>
    public class AutoTransformTaskDetail
    {
        /// <summary>
        /// Creates the (source, target) member pairs that will be auto transformed.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the transform is being built.
        /// </param>
        /// <param name="strategies">
        /// The strategies to apply.
        /// </param>
        /// <returns>
        /// The collection of (source, target) member pairs that will be auto transformed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public virtual IEnumerable<Tuple<MemberInfo, MemberInfo>> CreateSourceTargetPairs(
            [NotNull] TransformScaffold scaffold,
            [NotNull] IEnumerable<IAutoTransformStrategy> strategies)
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

        public virtual void ProcessPair(
            [NotNull] TransformScaffold scaffold,
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

            var template = CreateExpressionTemplate(sourceMember, targetMember);
            var transform = CreateTransformExpression(scaffold, template, sourceMember);
            var assign = Expression.Assign(
                targetMember.GetAccessExpression(scaffold.Target), 
                Expression.Convert(transform, targetMember.GetReturnType()));

            scaffold.Expressions.Add(assign);
            scaffold.ProcessedTargetMembers.Add(targetMember);
        }

        private static Expression<Action> CreateExpressionTemplate(MemberInfo sourceMember, MemberInfo targetMember)
        {
            var sourceMemberType = sourceMember.GetReturnType();
            var targetMemberType = targetMember.GetReturnType();

            return sourceMemberType.IsValueType || sourceMemberType.IsSealed
                ? CreateStaticExpressionTemplate(sourceMemberType, targetMemberType)
                : CreateDynamicExpressionTemplate(sourceMemberType, targetMemberType);
        }

        private static Expression<Action> CreateStaticExpressionTemplate(Type sourceMemberType, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<ITransformContext>("context")
                    .TransformStore.Resolve(sourceMemberType, targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("sourceMember"),
                        Placeholder.Of<ITransformContext>("context"));
        }

        private static Expression<Action> CreateDynamicExpressionTemplate(Type sourceMemberType, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<ITransformContext>("context")
                    .TransformStore.Resolve(
                        Placeholder.Of<object>("sourceMember") == null
                            ? sourceMemberType
                            : Placeholder.Of<object>("sourceMember").GetType(),
                        targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("sourceMember"),
                        Placeholder.Of<ITransformContext>("context"));
        }

        private static Expression CreateTransformExpression(
           TransformScaffold scaffold,
           Expression<Action> template,
           MemberInfo sourceMember)
        {
            template = (Expression<Action>)
                new PlaceholderExpressionVisitor("context", scaffold.TransformContext).Visit(template);

            template = (Expression<Action>)
                new PlaceholderExpressionVisitor(
                    "sourceMember", 
                    Expression.Convert(sourceMember.GetAccessExpression(scaffold.Source), typeof(object)))
                .Visit(template);

            return template.Body;
        } 
    }
}