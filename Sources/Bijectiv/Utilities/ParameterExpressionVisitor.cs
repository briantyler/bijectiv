// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterExpressionVisitor.cs" company="Bijectiv">
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
//   Defines the ParameterExpressionVisitor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    /// <summary>
    /// An expression visitor that substitutes an <see cref="Expression"/> for every specific 
    /// <see cref="ParameterExpression"/> instance it finds.
    /// </summary>
    public class ParameterExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// The parameter expression.
        /// </summary>
        private readonly ParameterExpression parameter;

        /// <summary>
        /// The replacement expression.
        /// </summary>
        private readonly Expression replacement;

        /// <summary>
        /// Initialises a new instance of the <see cref="ParameterExpressionVisitor"/> class.
        /// </summary>
        /// <param name="parameter">
        /// The parameter expression.
        /// </param>
        /// <param name="replacement">
        /// The replacement expression to substitute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ParameterExpressionVisitor(
            [NotNull] ParameterExpression parameter,
            [NotNull] Expression replacement)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            if (replacement == null)
            {
                throw new ArgumentNullException("replacement");
            }

            this.parameter = parameter;
            this.replacement = replacement;
        }

        /// <summary>
        /// Visits the children of the <see cref="ParameterExpression"/>.
        /// </summary>
        /// <param name="node">
        /// The expression to visit.
        /// </param>
        /// <returns>
        /// The modified expression, if it or any sub-expression was modified; otherwise, returns the original
        /// expression.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "There is no way that node can be null, so this does not require validation.")]
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.parameter == node ? this.replacement : base.VisitParameter(node);
        }
    }
}