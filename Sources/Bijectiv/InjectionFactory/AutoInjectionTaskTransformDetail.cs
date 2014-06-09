// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskTransformDetail.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskTransformDetail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    /// <summary>
    /// The <see cref="ITransform"/> specific <see cref="AutoInjectionTask"/> implementation detail.
    /// </summary>
    public class AutoInjectionTaskTransformDetail : AutoInjectionTaskDetail
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

            var template = CreateExpressionTemplate(sourceMember, targetMember);
            var transform = CreateTransformExpression(scaffold, template, sourceMember);

            return Expression.Assign(
                targetMember.GetAccessExpression(scaffold.Target),
                Expression.Convert(transform, targetMember.GetReturnType()));
        }

        /// <summary>
        /// Creates the transform <see cref="Expression"/> template.
        /// </summary>
        /// <param name="sourceMember">
        /// The source member.
        /// </param>
        /// <param name="targetMember">
        /// The target member.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/> template.
        /// </returns>
        private static Expression CreateExpressionTemplate(MemberInfo sourceMember, MemberInfo targetMember)
        {
            var sourceMemberType = sourceMember.GetReturnType();
            var targetMemberType = targetMember.GetReturnType();

            return (sourceMemberType.IsValueType || sourceMemberType.IsSealed
                ? CreateStaticExpressionTemplate(sourceMemberType, targetMemberType)
                : CreateDynamicExpressionTemplate(sourceMemberType, targetMemberType))
                .Body;
        }

        /// <summary>
        /// Creates the transform <see cref="Expression"/> template for source types that have no derived types.
        /// </summary>
        /// <param name="sourceMemberType">
        /// The source member type.
        /// </param>
        /// <param name="targetMemberType">
        /// The target member type.
        /// </param>
        /// <returns>
        /// The transform <see cref="Expression"/> template for source types that have no derived types.
        /// </returns>
        private static Expression<Action> CreateStaticExpressionTemplate(Type sourceMemberType, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(sourceMemberType, targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("sourceMember"),
                        Placeholder.Of<IInjectionContext>("context"), 
                        null);
        }

        /// <summary>
        /// Creates the transform <see cref="Expression"/> template for source types that could have derived types.
        /// </summary>
        /// <param name="sourceMemberType">
        /// The source member type.
        /// </param>
        /// <param name="targetMemberType">
        /// The target member type.
        /// </param>
        /// <returns>
        /// The transform <see cref="Expression"/> template for source types that could have derived types.
        /// </returns>
        private static Expression<Action> CreateDynamicExpressionTemplate(Type sourceMemberType, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(
                        Placeholder.Of<object>("sourceMember") == null
                            ? sourceMemberType
                            : Placeholder.Of<object>("sourceMember").GetType(),
                        targetMemberType)
                    .Transform(
                        Placeholder.Of<object>("sourceMember"),
                        Placeholder.Of<IInjectionContext>("context"), 
                        null);
        }

        /// <summary>
        /// Creates the specific transform <see cref="Expression"/> from a template.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <param name="sourceMember">
        /// The source member.
        /// </param>
        /// <returns>
        /// The specific transform <see cref="Expression"/> from a template.
        /// </returns>
        private static Expression CreateTransformExpression(
           InjectionScaffold scaffold,
           Expression template,
           MemberInfo sourceMember)
        {
            template = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(template);
            template = new PlaceholderExpressionVisitor(
                    "sourceMember",
                    Expression.Convert(sourceMember.GetAccessExpression(scaffold.Source), typeof(object)))
                .Visit(template);

            return template;
        } 
    }
}