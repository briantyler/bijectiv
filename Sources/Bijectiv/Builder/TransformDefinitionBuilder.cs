// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformDefinitionBuilder.cs" company="Bijectiv">
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
//   Defines the TransformDefinitionBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// A builder of <see cref="TransformDefinition"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public class TransformDefinitionBuilder<TSource, TTarget> : ITransformDefinitionBuilder<TSource, TTarget>
    {
        /// <summary>
        /// The definition being built.
        /// </summary>
        private readonly TransformDefinition definition;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformDefinitionBuilder{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="definition"/> is incompatible with the builder.
        /// </exception>
        public TransformDefinitionBuilder([NotNull] TransformDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            if (definition.Source != typeof(TSource))
            {
                throw new ArgumentException(
                    string.Format("Source '{0}' must match TSource '{1}'", definition.Source, typeof(TSource)),
                    "definition");
            }

            if (definition.Target != typeof(TTarget))
            {
                throw new ArgumentException(
                    string.Format("Target '{0}' must match TTarget '{1}'", definition.Target, typeof(TTarget)),
                    "definition");
            }

            this.definition = definition;
        }

        /// <summary>
        /// Gets the definition being built.
        /// </summary>
        protected internal TransformDefinition Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Instructs the transform to construct the target via activation (this is the default option if no other
        /// construction method is specified).
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefintionBuilderF<TSource, TTarget> Activate()
        {
            this.Definition.Add(new ActivateFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the transform to construct the target via the default factory.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefintionBuilderF<TSource, TTarget> DefaultFactory()
        {
            this.Definition.Add(new DefaultFactoryFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the transform to construct the target via a custom factory delegate.
        /// </summary>
        /// <param name="factory">
        /// The factory delegate that constructs the target.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefintionBuilderF<TSource, TTarget> CustomFactory(
            Func<CustomFactoryParameters<TSource>, TTarget> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.Definition.Add(new CustomFactoryFragment(typeof(TSource), typeof(TTarget), factory));
            return this;
        }
    }
}