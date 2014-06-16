// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaceholderExpressionVisitor.cs" company="Bijectiv">
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
//   Defines the PlaceholderExpressionVisitor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JetBrains.Annotations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    /// <summary>
    /// An expression visitor that substitutes an <see cref="Expression"/> for every
    /// <see cref="Placeholder.Of{T}(string)"/> it finds with a given name.
    /// </summary>
    public class PlaceholderExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// The name of the placeholder to replace.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The replacement expression.
        /// </summary>
        private readonly Expression replacement;

        /// <summary>
        /// Initialises a new instance of the <see cref="PlaceholderExpressionVisitor"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the placeholder.
        /// </param>
        /// <param name="replacement">
        /// The replacement expression to substitute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public PlaceholderExpressionVisitor([NotNull] string name, [NotNull] Expression replacement)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (replacement == null)
            {
                throw new ArgumentNullException("replacement");
            }

            this.name = name;
            this.replacement = replacement;
        }

        /// <summary>
        /// Visits the children of the <see cref="MethodCallExpression"/>.
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
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType != typeof(Placeholder))
            {
                return base.VisitMethodCall(node);
            }

            if (node.Arguments.Count != 1)
            {
                return base.VisitMethodCall(node);
            }

            var argumentName = node.Arguments[0] as ConstantExpression;
            if (argumentName != null)
            {
                return !this.name.Equals(argumentName.Value) ? base.VisitMethodCall(node) : this.replacement;
            }

            var member = node.Arguments[0] as MemberExpression;
            if (member != null)
            {
                var value = Expression.Lambda<Func<string>>(member.Member.GetAccessExpression(member.Expression)).Compile()();
                return !this.name.Equals(value) ? base.VisitMethodCall(node) : this.replacement;
            }

            throw new InvalidOperationException(
                string.Format("Unable to replace '{0}' parameter in '{1}'", this.name, node.Arguments[0]));
        }
    }
}