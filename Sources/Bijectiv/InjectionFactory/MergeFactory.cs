// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeFactory.cs" company="Bijectiv">
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
//   Defines the MergeFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.InjectionFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Injections;
    using Bijectiv.KernelBuilder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A factory that creates <see cref="IMerge"/> instances.
    /// </summary>
    public class MergeFactory : IInjectionFactory<IMerge>
    {
         /// <summary>
        /// The ordered collection of tasks that build the transform delegate.
        /// </summary>
        private readonly IEnumerable<IInjectionTask> tasks;

        /// <summary>
        /// Initialises a new instance of the <see cref="MergeFactory"/> class.
        /// </summary>
        /// <param name="tasks">
        /// The ordered collection of tasks that build the <see cref="IMerge"/> delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public MergeFactory([NotNull] IEnumerable<IInjectionTask> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException("tasks");
            }

            this.tasks = tasks;
        }

        /// <summary>
        /// Gets the ordered collection of tasks that build the transform delegate.
        /// </summary>
        public IEnumerable<IInjectionTask> Tasks
        {
            get { return this.tasks; }
        }

        /// <summary>
        /// Creates a <see cref="IMerge"/> from a <see cref="InjectionDefinition"/>.
        /// </summary>
        /// <param name="definitionRegistry">
        /// The registry containing all known instances.
        /// </param>
        /// <param name="definition">
        /// The definition from which to create a <see cref="IMerge"/>.
        /// </param>
        /// <returns>
        /// The <see cref="IMerge"/> that is defined by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public IMerge Create(IInstanceRegistry definitionRegistry, InjectionDefinition definition)
        {
           if (definitionRegistry == null)
            {
                throw new ArgumentNullException("definitionRegistry");
            }

            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            var sourceAsObject = Expression.Parameter(typeof(object), "sourceAsObject");
            var targetAsObject = Expression.Parameter(typeof(object), "targetAsObject");
            var injectionContext = Expression.Parameter(typeof(IInjectionContext), "InjectionContext");
            var hint = Expression.Parameter(typeof(object), "hint");

            var scaffold = new InjectionScaffold(
                definitionRegistry, definition, sourceAsObject, injectionContext)
                {
                    TargetAsObject = targetAsObject,
                    Hint = hint
                };

            this.Tasks.ForEach(item => item.Execute(scaffold));

            var lambda = Expression.Lambda<DMerge>(
                Expression.Block(typeof(IMergeResult), scaffold.Variables, scaffold.Expressions),
                sourceAsObject,
                targetAsObject,
                injectionContext,
                hint);

            return new DelegateMerge(definition.Source, definition.Target, lambda.Compile());
        }
    }
}