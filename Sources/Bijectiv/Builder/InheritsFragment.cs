// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritsFragment.cs" company="Bijectiv">
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
//   Defines the InheritsFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an inheritance relationship between transforms.
    /// </summary>
    public class InheritsFragment : TransformFragment
    {
        /// <summary>
        /// The source base type.
        /// </summary>
        private readonly Type sourceBase;

        /// <summary>
        /// The target base type.
        /// </summary>
        private readonly Type targetBase;

        /// <summary>
        /// Initialises a new instance of the <see cref="InheritsFragment"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="sourceBase">
        /// The source base type.
        /// </param>
        /// <param name="targetBase">
        /// The target base type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InheritsFragment(
            [NotNull] Type source, [NotNull] Type target, [NotNull] Type sourceBase, [NotNull] Type targetBase)
            : base(source, target)
        {
            if (sourceBase == null)
            {
                throw new ArgumentNullException("sourceBase");
            }

            if (targetBase == null)
            {
                throw new ArgumentNullException("targetBase");
            }

            if (source == sourceBase && target == targetBase)
            {
                throw new ArgumentException("A transform cannot inherit from itself.");
            }

            if (!sourceBase.IsAssignableFrom(source))
            {
                throw new ArgumentException(
                    string.Format("Source base type '{0}' is not assignable source type '{1}'.", sourceBase, source),
                    "sourceBase");
            }

            if (!targetBase.IsAssignableFrom(target))
            {
                throw new ArgumentException(
                    string.Format("Target base type '{0}' is not assignable target type '{1}'.", targetBase, target),
                    "targetBase");
            }

            this.sourceBase = sourceBase;
            this.targetBase = targetBase;
        }

        /// <summary>
        /// Gets a value indicating whether the fragment is inherited.
        /// </summary>
        public override bool Inherited
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the fragment category.
        /// </summary>
        public override Guid FragmentCategory
        {
            get { return LegendryFragments.Inherits; }
        }

        /// <summary>
        /// Gets the source base.
        /// </summary>
        public Type SourceBase
        {
            get { return this.sourceBase; }
        }

        /// <summary>
        /// Gets the target base.
        /// </summary>
        public Type TargetBase
        {
            get { return this.targetBase; }
        }
    }
}