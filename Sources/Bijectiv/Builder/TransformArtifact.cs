// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformArtifact.cs" company="Bijectiv">
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
//   Defines the TransformArtifact type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// An artifact that completely describes a transform.
    /// </summary>
    public class TransformArtifact : IEnumerable<TransformFragment>
    {
        /// <summary>
        /// The fragments that comprise the artifact.
        /// </summary>
        private readonly List<TransformFragment> fragments = new List<TransformFragment>();

        /// <summary>
        /// The source type.
        /// </summary>
        private readonly Type source;

        /// <summary>
        /// The target type.
        /// </summary>
        private readonly Type target;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformArtifact"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public TransformArtifact([NotNull] Type source, [NotNull] Type target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            this.source = source;
            this.target = target;
        }

        /// <summary>
        /// Gets the source type.
        /// </summary>
        public Type Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public Type Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// Adds the fragment to the artifact.
        /// </summary>
        /// <param name="fragment">
        /// The fragment.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Add([NotNull] TransformFragment fragment)
        {
            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (fragment.Source != this.Source)
            {
                throw new ArgumentException(string.Format(
                    "The fragment source '{0}' does not match the artifact source '{1}'", 
                    fragment.Source,
                    this.Source));
            }

            if (fragment.Target != this.Target)
            {
                throw new ArgumentException(string.Format(
                    "The fragment target '{0}' does not match the artifact target '{1}'",
                    fragment.Target,
                    this.Target));
            }

            this.fragments.Add(fragment);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the fragments in the artifact.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the fragments in the collection.
        /// </returns>
        public IEnumerator<TransformFragment> GetEnumerator()
        {
            return this.fragments.GetEnumerator();
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the fragments in the artifact.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the fragments in the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.fragments.GetEnumerator();
        }
    }
}