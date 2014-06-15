// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetFinderRegistration.cs" company="Bijectiv">
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
//   Defines the TargetFinderRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelBuilder
{
    using System;

    using Bijectiv.Utilities;

    /// <summary>
    /// A <see cref="ITargetFinder"/> registration.
    /// </summary>
    public class TargetFinderRegistration
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TargetFinderRegistration"/> class.
        /// </summary>
        /// <param name="sourceElement">
        /// The source element type supported by the created <see cref="ITargetFinder"/>.
        /// </param>
        /// <param name="targetElement">
        /// The target element type supported by the created <see cref="ITargetFinder"/>.
        /// </param>
        /// <param name="targetFinderFactory">
        /// The factory that creates a <see cref="ITargetFinder"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public TargetFinderRegistration(
            [NotNull] Type sourceElement, 
            [NotNull] Type targetElement,
            [NotNull] Func<ITargetFinder> targetFinderFactory)
        {
            if (sourceElement == null)
            {
                throw new ArgumentNullException("sourceElement");
            }

            if (targetElement == null)
            {
                throw new ArgumentNullException("targetElement");
            }

            if (targetFinderFactory == null)
            {
                throw new ArgumentNullException("targetFinderFactory");
            }

            this.SourceElement = sourceElement;
            this.TargetElement = targetElement;
            this.TargetFinderFactory = targetFinderFactory;
        }

        /// <summary>
        /// Gets the source element type supported by the created <see cref="ITargetFinder"/>.
        /// </summary>
        public Type SourceElement { get; private set; }

        /// <summary>
        /// Gets the target element type supported by the created <see cref="ITargetFinder"/>.
        /// </summary>
        public Type TargetElement { get; private set; }

        /// <summary>
        /// Gets the factory that creates a <see cref="ITargetFinder"/>.
        /// </summary>
        public Func<ITargetFinder> TargetFinderFactory { get; private set;  }
    }
}