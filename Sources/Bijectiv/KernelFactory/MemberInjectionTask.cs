// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInjectionTask.cs" company="Bijectiv">
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
//   Defines the MemberInjectionTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a task that processes <see cref="MemberFragment"/> fragments.
    /// </summary>
    public class MemberInjectionTask : IInjectionTask
    {
        /// <summary>
        /// The sub-tasks that determine how the member injection will be built.
        /// </summary>
        private readonly IEnumerable<IInjectionSubTask<MemberFragment>> subTasks;

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberInjectionTask"/> class.
        /// </summary>
        /// <param name="subTasks">
        /// The sub-tasks that determine how the member injection will be built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public MemberInjectionTask([NotNull] IEnumerable<IInjectionSubTask<MemberFragment>> subTasks)
        {
            if (subTasks == null)
            {
                throw new ArgumentNullException("subTasks");
            }

            this.subTasks = subTasks;
        }

        /// <summary>
        /// Gets the sub-tasks that determine how the member injection will be built.
        /// </summary>
        public IEnumerable<IInjectionSubTask<MemberFragment>> SubTasks
        {
            get { return this.subTasks; }
        }

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

            var fragments = scaffold.UnprocessedFragments.OfType<MemberFragment>().ToArray();

            //// TODO: Ensure that duplicates are ignored.
            fragments
                .Where(candidate => scaffold.UnprocessedTargetMembers.Contains(candidate.Member))
                .ForEach(item => this.ProcessFragment(scaffold, item));

            scaffold.ProcessedFragments.AddRange(fragments);
        }

        public virtual void ProcessFragment([NotNull] InjectionScaffold scaffold, [NotNull] MemberFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            this.SubTasks.ForEach(item => item.Execute(scaffold, fragment));
            scaffold.ProcessedTargetMembers.Add(fragment.Member);
        }
    }
}