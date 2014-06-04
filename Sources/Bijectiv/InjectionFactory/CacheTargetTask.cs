// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTargetTask.cs" company="Bijectiv">
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
//   Defines the CacheTargetTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A task that caches a target instance.
    /// </summary>
    public class CacheTargetTask : IInjectionTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="ITransform"/> is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }
            
            var expression = (Expression)(Expression<Action>)(
                () => Placeholder.Of<IInjectionContext>("context").TargetCache.Add(
                    scaffold.Definition.Source,
                    scaffold.Definition.Target,
                    Placeholder.Of<object>("source"),
                    Placeholder.Of<object>("target")));

            expression = new PlaceholderExpressionVisitor("context", scaffold.InjectionContext).Visit(expression);
            expression = new PlaceholderExpressionVisitor("source", scaffold.SourceAsObject).Visit(expression);
            expression = new PlaceholderExpressionVisitor("target", scaffold.TargetAsObject).Visit(expression);

            scaffold.Expressions.Add(((LambdaExpression)expression).Body);
        }
    }
}