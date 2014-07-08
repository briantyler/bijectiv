// --------------------------------------------------------------------------------------------------------------------
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
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    public class MemberValueSourceTransformSubtask : IInjectionSubtask<MemberFragment>
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

            var shard = fragment.UnprocessedShards.OfType<ValueSourceMemberShard>().FirstOrDefault();
            if (shard == null)
            {
                return;
            }

            var expression = CreateExpressionTemplate(shard.Value, fragment.Member.GetReturnType()).Body;
            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);
            expression = Expression.Assign(
                fragment.Member.GetAccessExpression(scaffold.Target),
                Expression.Convert(expression, fragment.Member.GetReturnType()));

            scaffold.Expressions.Add(expression);
        }

        private static Expression<Action> CreateExpressionTemplate(object value, Type targetMemberType)
        {
            return () =>
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore.Resolve<ITransform>(value.GetType(), targetMemberType)
                    .Transform(value, Placeholder.Of<IInjectionContext>("context"), null);
        }
    }
}