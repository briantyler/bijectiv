// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformFactory.cs" company="Bijectiv">
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
//   Defines the TransformFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Transforms;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A factory that creates <see cref="DelegateTransform"/> instances.
    /// </summary>
    public class TransformFactory : ITransformFactory
    {
        /// <summary>
        /// The ordered collection of tasks that build the transform delegate.
        /// </summary>
        private readonly IEnumerable<ITransformTask> taskCollection;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformFactory"/> class.
        /// </summary>
        /// <param name="taskCollection">
        /// The ordered collection of tasks that build the transform delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public TransformFactory([NotNull] IEnumerable<ITransformTask> taskCollection)
        {
            if (taskCollection == null)
            {
                throw new ArgumentNullException("taskCollection");
            }

            this.taskCollection = taskCollection;
        }

        /// <summary>
        /// Gets the ordered collection of tasks that build the transform delegate.
        /// </summary>
        public IEnumerable<ITransformTask> TaskCollection
        {
            get { return this.taskCollection; }
        }

        /// <summary>
        /// Creates a <see cref="ITransform"/> from a <see cref="TransformDefinition"/>.
        /// </summary>
        /// <param name="definitionRegistry">
        /// The registry containing all known <see cref="TransformDefinition"/> instances.
        /// </param>
        /// <param name="definition">
        /// The definition from which to create a <see cref="ITransform"/>.
        /// </param>
        /// <returns>
        /// The <see cref="ITransform"/> that is defined by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ITransform Create(ITransformDefinitionRegistry definitionRegistry, TransformDefinition definition)
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
            var transformContext = Expression.Parameter(typeof(ITransformContext), "transformContext");

            var scaffold = new TransformScaffold(
                definitionRegistry, definition, sourceAsObject, transformContext);

            this.TaskCollection.ForEach(item => item.Execute(scaffold));

            var lambda = Expression.Lambda<Func<object, ITransformContext, object>>(
                Expression.Block(typeof(object), scaffold.Variables, scaffold.Expressions),
                sourceAsObject,
                transformContext);

            return new DelegateTransform(definition.Source, definition.Target, lambda.Compile());
        }
    }
}