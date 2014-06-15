// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMergeResultTask.cs" company="Bijectiv">
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
//   Defines the ReturnMergeResultTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Kernel;
    using Bijectiv.Utilities;

    /// <summary>
    /// A task that returns a <see cref="IMergeResult"/>.
    /// </summary>
    public class ReturnMergeResultTask : IInjectionTask
    {
        /// <summary>
        /// The <see cref="MergeResult"/> constructor.
        /// </summary>
        private static readonly ConstructorInfo Constructor = Reflect<MergeResult>.Constructor(
            () => new MergeResult(Placeholder.Of<PostMergeAction>(), Placeholder.Of<object>()));

        /// <summary>
        /// The return type.
        /// </summary>
        private static readonly Type ReturnType = typeof(IMergeResult);

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var postMergeAction = (Expression)scaffold.Variables
                .SingleOrDefault(
                    candidate => candidate.Name.Equals("PostMergeAction", StringComparison.OrdinalIgnoreCase))
                ?? Expression.Constant(PostMergeAction.None);

            var result = Expression.New(Constructor, postMergeAction, scaffold.TargetAsObject);
            var returnTarget = Expression.Label(ReturnType);

            scaffold.Expressions.Add(Expression.Label(scaffold.ReturnLabel));
            scaffold.Expressions.Add(Expression.Return(returnTarget, result, ReturnType));
            scaffold.Expressions.Add(Expression.Label(returnTarget, Expression.Constant(null, ReturnType)));
        }
    }
}