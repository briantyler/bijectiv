// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetTask.cs" company="Bijectiv">
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
//   Defines the CreateTargetTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The transform task that creates target instances from a <see cref="ISelectiveExpressionFactory"/>.
    /// </summary>
    public class CreateTargetTask : ITransformTask
    {
        /// <summary>
        /// The expression factory which will create the expression that creates the target.
        /// </summary>
        private readonly ISelectiveExpressionFactory expressionFactory;

        /// <summary>
        /// Initialises a new instance of the <see cref="CreateTargetTask"/> class.
        /// </summary>
        /// <param name="expressionFactory">
        /// The expression factory which will create the expression that creates the target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public CreateTargetTask([NotNull] ISelectiveExpressionFactory expressionFactory)
        {
            if (expressionFactory == null)
            {
                throw new ArgumentNullException("expressionFactory");
            }

            this.expressionFactory = expressionFactory;
        }

        /// <summary>
        /// Gets the expression factory which will create the expression that creates the target.
        /// </summary>
        public ISelectiveExpressionFactory ExpressionFactory
        {
            get { return this.expressionFactory; }
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="ITransform"/> is being built.
        /// </param>
        public void Execute([NotNull] TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (!this.ExpressionFactory.CanCreateExpression(scaffold))
            {
                return;
            }

            var factoryFragments = scaffold
                .CandidateFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.Factory);
            scaffold.ProcessedFragments.AddRange(factoryFragments);

            var createTarget = this.ExpressionFactory.CreateExpression(scaffold);
            var assignTarget = Expression.Assign(scaffold.Target, createTarget);

            var targetToObject = Expression.Convert(scaffold.Target, typeof(object));
            var assignTargetAsObject = Expression.Assign(scaffold.TargetAsObject, targetToObject);

            scaffold.Expressions.Add(assignTarget);
            scaffold.Expressions.Add(assignTargetAsObject);
        }
    }
}