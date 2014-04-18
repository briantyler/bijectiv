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
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Transforms;

    using JetBrains.Annotations;

    /// <summary>
    /// A factory that creates <see cref="DelegateTransform"/> instances.
    /// </summary>
    public class TransformFactory
    {
        /// <summary>
        /// The registry containing all the live <see cref="TransformDefinition"/>s.
        /// </summary>
        private readonly ITransformDefinitionRegistry definitionRegistry;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformFactory"/> class.
        /// </summary>
        /// <param name="definitionRegistry">
        /// The registry containing all the live <see cref="TransformDefinition"/>s.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public TransformFactory([NotNull] ITransformDefinitionRegistry definitionRegistry)
        {
            if (definitionRegistry == null)
            {
                throw new ArgumentNullException("definitionRegistry");
            }

            this.definitionRegistry = definitionRegistry;
        }

        /// <summary>
        /// Creates a <see cref="ITransform"/> from a <see cref="TransformDefinition"/>.
        /// </summary>
        /// <param name="definition">
        /// The definition from which to create a <see cref="ITransform"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ITransform"/> that is represented by <paramref name="definition"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ITransform Create([NotNull] TransformDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            var sourceAsObject = Expression.Parameter(typeof(object), "sourceAsObject");
            var transformContext = Expression.Parameter(typeof(ITransformContext), "transformContext");

            var scaffold = new TransformScaffold(this.definitionRegistry, definition, sourceAsObject, transformContext);

            new InitializeVariablesTask().Execute(scaffold);
            new InitializeFragmentsTask().Execute(scaffold);
            new CreateTargetTask(new ActivateTargetExpressionFactory()).Execute(scaffold);
            new ReturnTargetAsObjectTask().Execute(scaffold);

            var lambda = Expression.Lambda<Func<object, ITransformContext, object>>(
                Expression.Block(typeof(object), scaffold.Variables, scaffold.Expressions),
                sourceAsObject,
                transformContext);

            return new DelegateTransform(definition.Source, definition.Target, lambda.Compile());
        }
    }
}