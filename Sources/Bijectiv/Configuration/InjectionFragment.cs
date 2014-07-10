// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionFragment.cs" company="Bijectiv">
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
//   Defines the InjectionFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// The base class for injection fragments.
    /// </summary>
    public abstract class InjectionFragment
    {
        /// <summary>
        /// The source type.
        /// </summary>
        private readonly Type source;

        /// <summary>
        /// The target type.
        /// </summary>
        private readonly Type target;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionFragment"/> class.
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
        protected InjectionFragment([NotNull] Type source, [NotNull] Type target)
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
        /// Initialises a new instance of the <see cref="InjectionFragment"/> class.
        /// </summary>
        protected InjectionFragment()
        {
        }

        /// <summary>
        /// Gets the source type.
        /// </summary>
        public virtual Type Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public virtual Type Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// Gets a value indicating whether the fragment is inherited.
        /// </summary>
        public abstract bool Inherited { get;  }

        /// <summary>
        /// Gets the fragment category.
        /// </summary>
        public abstract Guid FragmentCategory { get; }
    }
}