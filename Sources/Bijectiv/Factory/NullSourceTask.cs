// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSourceTask.cs" company="Bijectiv">
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
//   Defines the NullSourceTask type.
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
    /// Represents a task that processes <see cref="NullSourceFragment"/> fragments.
    /// </summary>
    public class NullSourceTask : IInjectionTask
    {
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

            var fragment = scaffold.UnprocessedFragments.OfType<NullSourceFragment>().FirstOrDefault();

            Expression createTarget;
            if (fragment == null)
            {
                if (scaffold.ProcessedFragments.Any(
                        candidate => candidate.FragmentCategory == LegendryFragments.NullSource))
                {
                    return;
                }

                createTarget = Expression.Default(scaffold.Definition.Target);
            }
            else
            {
                createTarget = Expression.Call(
                    Expression.Constant(fragment.Factory),
                    fragment.FactoryType.GetMethod("Invoke"),
                    new[] { scaffold.InjectionContext });
            }

            var assignTarget = Expression.Assign(scaffold.Target, createTarget);
            var targetToObject = Expression.Convert(scaffold.Target, typeof(object));
            var assignTargetAsObject = Expression.Assign(scaffold.TargetAsObject, targetToObject);

            var ifIsNull = Expression.IfThen(
                Expression.Equal(scaffold.SourceAsObject, Expression.Constant(null)),
                Expression.Block(assignTarget, assignTargetAsObject, Expression.Goto(scaffold.ReturnLabel)));

            scaffold.Expressions.Add(ifIsNull);

            var nullSourceFragments = scaffold.CandidateFragments
                .Where(candidate => candidate.FragmentCategory == LegendryFragments.NullSource);
            scaffold.ProcessedFragments.AddRange(nullSourceFragments);
        }
    }
}