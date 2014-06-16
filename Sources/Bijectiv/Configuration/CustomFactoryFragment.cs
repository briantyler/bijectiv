// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomFactoryFragment.cs" company="Bijectiv">
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
//   Defines the CustomFactoryFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Specifies that the <see cref="InjectionFragment.Target"/> should be created by a factory method.
    /// </summary>
    public class CustomFactoryFragment : InjectionFragment
    {
        /// <summary>
        /// The factory type template.
        /// </summary>
        private static readonly Type FactoryTypeTemplate = typeof(Func<,>);

        /// <summary>
        /// The parameters type template.
        /// </summary>
        private static readonly Type ParametersTypeTemplate = typeof(CustomFactoryParameters<>);

        /// <summary>
        /// The custom factory delegate.
        /// </summary>
        private readonly object factory;

        /// <summary>
        /// The type of the custom factory delegate.
        /// </summary>
        private readonly Type factoryType;

        /// <summary>
        /// The parameters type.
        /// </summary>
        private readonly Type parametersType;

        /// <summary>
        /// Initialises a new instance of the <see cref="CustomFactoryFragment"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="factory">
        /// The custom factory delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="factory"/> does not have the expected type.
        /// </exception>
        public CustomFactoryFragment([NotNull] Type source, [NotNull] Type target, [NotNull] object factory)
            : base(source, target)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.parametersType = ParametersTypeTemplate.MakeGenericType(source);
            this.factoryType = FactoryTypeTemplate.MakeGenericType(this.parametersType, target);
            if (!this.factoryType.IsInstanceOfType(factory))
            {
                var message = string.Format(
                    "The factory '{0}' is not an instance of '{1}'.", factory.GetType(), this.factoryType);
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
            get { return LegendaryFragments.Factory; }
        }

        /// <summary>
        /// Gets the custom factory delegate.
        /// </summary>
        public object Factory
        {
            get { return this.factory; }
        }

        /// <summary>
        /// Gets the type of the custom factory delegate.
        /// </summary>
        public Type FactoryType
        {
            get { return this.factoryType; }
        }

        /// <summary>
        /// Gets the parameters type.
        /// </summary>
        public Type ParametersType
        {
            get { return this.parametersType; }
        }
    }
}