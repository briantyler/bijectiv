// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSourceMemberShard.cs" company="Bijectiv">
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
//   Defines the ExpressionSourceMemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// A member shard that contains a lambda expression that extracts a value from an instance of type 
    /// <see cref="MemberShard.Source"/> from which the target member will be injected.
    /// </summary>
    public class ExpressionSourceMemberShard : SourceMemberShard
    {
        /// <summary>
        /// The lambda expression.
        /// </summary>
        private readonly LambdaExpression expression;

        /// <summary>
        /// The type of the parameter to the expression.
        /// </summary>
        private readonly Type parameterType;

        /// <summary>
        /// Initialises a new instance of the <see cref="ExpressionSourceMemberShard"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="member">
        /// The member on the target type to which the shard corresponds.
        /// </param>
        /// <param name="expression">
        /// The lambda expression that extracts a value from an instance of type <see cref="MemberShard.Source"/>.
        /// </param>
        /// <param name="inject">
        /// A value indicating whether to inject the source; when false the source will be assigned to the target 
        /// member, not injected.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the lambda expression does not have signature <c>Func&lt;source, *&gt;</c>.
        /// </exception>
        public ExpressionSourceMemberShard(
            [NotNull] Type source,
            [NotNull] Type target, 
            [NotNull] MemberInfo member, 
            [NotNull] LambdaExpression expression,
            bool inject)
            : base(source, target, member, inject)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (expression.Parameters.Count != 1)
            {
                throw new ArgumentException(
                    string.Format(
                        "Lambda expression expected with 1 parameter, but {0} parameters found.",
                        expression.Parameters.Count));
            }

            this.parameterType = expression.Parameters.First().Type;
            if (!this.ParameterType.IsAssignableFrom(source))
            {
                throw new ArgumentException(
                    string.Format(
                        "Lambda expression parameter type '{0}' expected, but was '{1}'.",
                        source,
                        this.ParameterType));
            }

            if (expression.ReturnType == typeof(void))
            {
                throw new ArgumentException("Lambda expression has no return value.");
            }

            this.expression = expression;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public virtual LambdaExpression Expression
        {
            get { return this.expression; }
        }

        /// <summary>
        /// Gets the type of the parameter to the expression.
        /// </summary>
        public Type ParameterType
        {
            get { return this.parameterType; }
        }
    }
}