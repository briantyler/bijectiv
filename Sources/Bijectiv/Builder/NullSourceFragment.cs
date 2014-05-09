// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSourceFragment.cs" company="Bijectiv">
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
//   Defines the NullSourceFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an instruction that determines how to treat a <c>NULL</c> source.
    /// </summary>
    public class NullSourceFragment : InjectionFragment
    {
        /// <summary>
        /// The factory type template.
        /// </summary>
        private static readonly Type FactoryTypeTemplate = typeof(Func<,>);

        /// <summary>
        /// The factory that creates the target from a <c>NULL</c> source.
        /// </summary>
        private readonly object factory;

        /// <summary>
        /// The type of the factory delegate.
        /// </summary>
        private readonly Type factoryType;

        /// <summary>
        /// Initialises a new instance of the <see cref="NullSourceFragment"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="factory">
        /// The factory that creates the target from a <c>NULL</c> source.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="factory"/> does not have the expected type.
        /// </exception>
        public NullSourceFragment([NotNull] Type source, [NotNull] Type target, [NotNull] object factory)
            : base(source, target)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.factoryType = FactoryTypeTemplate.MakeGenericType(typeof(IInjectionContext), target);
            if (!this.FactoryType.IsInstanceOfType(factory))
            {
                var message = string.Format(
                    "The factory '{0}' is not an instance of '{1}'.", factory.GetType(), this.FactoryType);
                throw new ArgumentException(message, "factory");
            }

            this.factory = factory;
        }

        /// <summary>
        /// Gets a value indicating whether the fragment is inherited.
        /// </summary>
        public override bool Inherited
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the fragment category.
        /// </summary>
        public override Guid FragmentCategory
        {
            get { return LegendryFragments.NullSource; }
        }

        /// <summary>
        /// Gets the factory that creates the target from a <c>NULL</c> source.
        /// </summary>
        public object Factory
        {
            get { return this.factory; }
        }

        /// <summary>
        /// Gets the type of the factory delegate.
        /// </summary>
        public Type FactoryType
        {
            get { return this.factoryType; }
        }
    }
}