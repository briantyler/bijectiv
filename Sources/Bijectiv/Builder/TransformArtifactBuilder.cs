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
        public TransformArtifactBuilder([NotNull] TransformArtifact artifact)
        {
            if (artifact == null)
            {
                throw new ArgumentNullException("artifact");
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
    }
}