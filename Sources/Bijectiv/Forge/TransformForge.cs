// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformForge.cs" company="Bijectiv">
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
//   Defines the TransformForge type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Transforms;

    using JetBrains.Annotations;

    /// <summary>
    /// A forge that creates <see cref="DelegateTransform"/> instances.
    /// </summary>
    public class TransformForge
    {
        /// <summary>
        /// The artifact from which to create a <see cref="DelegateTransform"/>.
        /// </summary>
        private readonly TransformArtifact artifact;

        /// <summary>
        /// The registry containing all the live <see cref="TransformArtifact"/>s.
        /// </summary>
        private readonly ITransformArtifactRegistry artifactRegistry;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformForge"/> class.
        /// </summary>
        /// <param name="artifact">
        /// The artifact from which to create a <see cref="DelegateTransform"/>.
        /// </param>
        /// <param name="artifactRegistry">
        /// The registry containing all the live <see cref="TransformArtifact"/>s.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public TransformForge(
            [NotNull] TransformArtifact artifact,
            [NotNull] ITransformArtifactRegistry artifactRegistry)
        {
            if (artifact == null)
            {
                throw new ArgumentNullException("artifact");
            }

            if (artifactRegistry == null)
            {
                throw new ArgumentNullException("artifactRegistry");
            }

            this.artifact = artifact;
            this.artifactRegistry = artifactRegistry;
        }

        public ITransform Manufacture()
        {
            var source = Expression.Parameter(typeof(object));
            var transformContext = Expression.Parameter(typeof(ITransformContext));

            var ore = new TransformOre(this.artifactRegistry, this.artifact, source, transformContext);

            new InitializeHammer().Strike(ore);
            new UninheritableFragmentFilterHammer().Strike(ore);
            new CandidateHammer().Strike(ore);
            new ActivateHammer().Strike(ore);
            new ReturnHammer().Strike(ore);

            var @delegate = Expression.Lambda<Func<object, ITransformContext, object>>(
                Expression.Block(ore.Expressions),
                source,
                transformContext).Compile();

            return new DelegateTransform(this.artifact.Source, this.artifact.Target, @delegate);
        }
    }
}