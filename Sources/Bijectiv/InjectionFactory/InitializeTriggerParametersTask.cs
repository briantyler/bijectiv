// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeTriggerParametersTask.cs" company="Bijectiv">
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
//   Defines the InitializeTriggerParametersTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Injections;
    using Bijectiv.KernelBuilder;

    /// <summary>
    /// A task that initializes a trigger parameters variable if any triggers exist.
    /// </summary>
    public class InitializeTriggerParametersTask : IInjectionTask
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

            var hasTriggers = scaffold.UnprocessedFragments
                .Any(candidate => candidate.FragmentCategory == LegendryFragments.Trigger);

            if (!hasTriggers)
            {
                return;
            }

            var parametersType = typeof(InjectionTriggerParameters<,>)
                .MakeGenericType(scaffold.Definition.Source, scaffold.Definition.Target);

            var createParameters = Expression.New(
                parametersType.GetConstructors().Single(),
                scaffold.Source,
                scaffold.Target,
                scaffold.InjectionContext,
                scaffold.Hint);

            var downCastParameters = Expression.Convert(createParameters, typeof(IInjectionTriggerParameters));
            var parametersVariable = Expression.Variable(typeof(IInjectionTriggerParameters), "triggerParameters");

            scaffold.Variables.Add(parametersVariable);
            scaffold.Expressions.Add(Expression.Assign(parametersVariable, downCastParameters));
        }
    }
}