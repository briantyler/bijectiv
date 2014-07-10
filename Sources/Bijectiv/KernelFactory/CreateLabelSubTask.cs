// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateLabelSubtask.cs" company="Bijectiv">
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
//   Defines the CreateLabelSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    /// <summary>
    /// A subtask that creates a label for a given category in the scope of a fragment.
    /// </summary>
    public class CreateLabelSubtask : IInjectionSubtask<InjectionFragment> 
    {
        /// <summary>
        /// The label category.
        /// </summary>
        private readonly Guid category;

        /// <summary>
        /// Initialises a new instance of the <see cref="CreateLabelSubtask"/> class.
        /// </summary>
        /// <param name="category">
        /// The label category.
        /// </param>
        public CreateLabelSubtask(Guid category)
        {
            this.category = category;
        }

        /// <summary>
        /// Gets the label category.
        /// </summary>
        public Guid Category
        {
            get { return this.category; }
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute(InjectionScaffold scaffold, InjectionFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(fragment, this.Category)));
        }
    }
}