﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeTransformVariablesTask.cs" company="Bijectiv">
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
//   Defines the InitializeTransformVariablesTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The task that initializes the scaffold variables for a <see cref="ITransform"/>.
    /// </summary>
    public class InitializeTransformVariablesTask : IInjectionTask
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

            scaffold.Variables.Add(Expression.Variable(scaffold.Definition.Source, "source"));
            scaffold.Source = scaffold.Variables.Last();

            scaffold.Variables.Add(Expression.Variable(scaffold.Definition.Target, "target"));
            scaffold.Target = scaffold.Variables.Last();

            scaffold.Variables.Add(Expression.Variable(typeof(object), "targetAsObject"));
            scaffold.TargetAsObject = scaffold.Variables.Last();

            var sourceToType = Expression.Convert(scaffold.SourceAsObject, scaffold.Definition.Source);
            var assignSource = Expression.Assign(scaffold.Source, sourceToType);

            scaffold.Expressions.Add(assignSource);
        }
    }
}