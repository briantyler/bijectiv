// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTransformTask.cs" company="Bijectiv">
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
//   Defines the AutoTransformTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a task that processes <see cref="AutoTransformFragment"/> fragments.
    /// </summary>
    public class AutoTransformTask : IInjectionTask
    {
        /// <summary>
        /// The task detail.
        /// </summary>
        private readonly AutoTransformTaskDetail detail;

        /// <summary>
        /// Initialises a new instance of the <see cref="AutoTransformTask"/> class.
        /// </summary>
        /// <param name="detail">
        /// The task detail.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public AutoTransformTask([NotNull] AutoTransformTaskDetail detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            this.detail = detail;
        }

        /// <summary>
        /// Gets the task detail.
        /// </summary>
        public AutoTransformTaskDetail Detail
        {
            get { return this.detail; }
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="ITransform"/> is being built.
        /// </param>
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var fragments = scaffold.UnprocessedFragments.OfType<AutoTransformFragment>().ToArray();

            this.Detail
                .CreateSourceTargetPairs(scaffold, fragments.Select(item => item.Strategy))
                .ForEach(pair => this.Detail.ProcessPair(scaffold, pair));

            scaffold.ProcessedFragments.AddRange(fragments);
        }
    }
}