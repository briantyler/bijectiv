// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FallBackToTransformOnNullTargetTask.cs" company="Bijectiv">
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
//   Defines the FallBackToTransformOnNullTargetTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    /// <summary>
    /// A task that allows the current <see cref="IMerge"/> to fall back to <see cref="ITransform"/> when the target
    /// is null.
    /// </summary>
    public class 
        FallBackToTransformOnNullTargetTask : IInjectionTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute(InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var isTargetNull = ((Expression<Func<bool>>)(() => Placeholder.Of<object>("targetAsObject") == null)).Body;
            var transform = ((Expression<Func<object>>)(() => 
                Placeholder.Of<IInjectionContext>("context")
                    .InjectionStore
                    .Resolve<ITransform>(
                        Placeholder.Of<object>("sourceAsObject") == null 
                            ? scaffold.Definition.Source 
                            : Placeholder.Of<object>("sourceAsObject").GetType(),
                        scaffold.Definition.Target)
                    .Transform(
                        Placeholder.Of<object>("sourceAsObject"),
                        Placeholder.Of<IInjectionContext>("context"), 
                        null))).Body;

            var postMergeAction = scaffold.GetVariable("PostMergeAction", typeof(PostMergeAction));
            var isTargetNullAction = Expression.Block(
                Expression.Assign(scaffold.TargetAsObject, transform),
                Expression.Assign(postMergeAction, Expression.Constant(PostMergeAction.Replace)),
                Expression.Goto(scaffold.GetLabel(null, LegendaryLabels.End)));

            Expression expression = Expression.IfThen(isTargetNull, isTargetNullAction);
            expression = new PlaceholderExpressionVisitor("targetAsObject", scaffold.TargetAsObject).Visit(expression);
            expression = new PlaceholderExpressionVisitor("sourceAsObject", scaffold.SourceAsObject).Visit(expression);
            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);

            scaffold.Expressions.Add(expression);
        }
    }
}