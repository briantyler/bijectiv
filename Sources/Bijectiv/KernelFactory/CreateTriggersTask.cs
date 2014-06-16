// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTriggersTask.cs" company="Bijectiv">
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
//   Defines the CreateTriggersTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// A task that creates all triggers with a given cause.
    /// </summary>
    public class CreateTriggersTask : IInjectionTask
    {
        /// <summary>
        /// The reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </summary>
        private readonly TriggeredBy triggeredBy;

        /// <summary>
        /// Initialises a new instance of the <see cref="CreateTriggersTask"/> class.
        /// </summary>
        /// <param name="triggeredBy">
        /// The reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </param>
        public CreateTriggersTask(TriggeredBy triggeredBy)
        {
            this.triggeredBy = triggeredBy;
        }

        /// <summary>
        /// Gets the reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </summary>
        public TriggeredBy TriggeredBy
        {
            get { return this.triggeredBy; }
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
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            scaffold
                .UnprocessedFragments
                .OfType<InjectionTriggerFragment>()
                .Where(candidate => candidate.TriggeredBy == this.TriggeredBy)
                .ForEach(fragment => this.ProcessFragment(fragment, scaffold));
        }

        /// <summary>
        /// Processes a <see cref="InjectionTriggerFragment"/> fragment.
        /// </summary>
        /// <param name="fragment">
        /// The fragment.
        /// </param>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        protected internal virtual void ProcessFragment(
            [NotNull] InjectionTriggerFragment fragment,
            [NotNull] InjectionScaffold scaffold)
        {
            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            var parameters = scaffold.Variables.First(candidate => candidate.Name == "injectionParameters");
            var trigger = fragment.Trigger;
            var expression = ((Expression<Action>)(
                () => trigger.Pull(Placeholder.Of<IInjectionParameters>("parameters"))))
                .Body;

            scaffold.Expressions.Add(new PlaceholderExpressionVisitor("parameters", parameters).Visit(expression));

            scaffold.ProcessedFragments.Add(fragment);
        }
    }
}