// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformArtifactBuilder.cs" company="Bijectiv">
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
//   Defines the TransformArtifactBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// A builder of <see cref="TransformArtifact"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public class TransformArtifactBuilder<TSource, TTarget> : ITransformArtifactBuilder<TSource, TTarget>
    {
        /// <summary>
        /// The artifact being built.
        /// </summary>
        private readonly TransformArtifact artifact;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformArtifactBuilder{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="artifact">
        /// The artifact.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="artifact"/> is incompatible with the builder.
        /// </exception>
        public TransformArtifactBuilder([NotNull] TransformArtifact artifact)
        {
            if (artifact == null)
            {
                throw new ArgumentNullException("artifact");
            }

            if (artifact.Source != typeof(TSource))
            {
                throw new ArgumentException(
                    string.Format("Source '{0}' must match TSource '{1}'", artifact.Source, typeof(TSource)),
                    "artifact");
            }

            if (artifact.Target != typeof(TTarget))
            {
                throw new ArgumentException(
                    string.Format("Target '{0}' must match TTarget '{1}'", artifact.Target, typeof(TTarget)),
                    "artifact");
            }

            this.artifact = artifact;
        }

        /// <summary>
        /// Gets the artifact being built.
        /// </summary>
        protected internal TransformArtifact Artifact
        {
            get { return this.artifact; }
        }

        /// <summary>
        /// Creates an inheritance relationship for the transform: this transform will inherit all of the fragments
        /// from the base transform if it exists.
        /// </summary>
        /// <typeparam name="TSourceBase">
        /// The base type that source inherits from.
        /// </typeparam>
        /// <typeparam name="TTargetBase">
        /// The base type that target inherits from.
        /// </typeparam>
        /// <returns>
        /// The an object that allows further configuration of the <see cref="TransformArtifact"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="TSourceBase"/> is not assignable from <typeparamref name="TSource"/>.
        /// Thrown when <typeparamref name="TTargetBase"/> is not assignable from <typeparamref name="TTarget"/>.
        /// </exception>
        /// <remarks>
        /// There is no way to enforce this relationship at compile time without reversing the relationship; i.e.
        /// calling this method "Derived", but that is not how it should be defined.
        /// </remarks>
        public ITransformArtifactBuilder<TSource, TTarget> Inherits<TSourceBase, TTargetBase>()
        {
            this.Artifact.Add(
                new InheritsFragment(typeof(TSource), typeof(TTarget), typeof(TSourceBase), typeof(TTargetBase)));

            return this;
        }
    }
}