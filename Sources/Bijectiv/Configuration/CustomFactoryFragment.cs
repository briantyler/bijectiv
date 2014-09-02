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
    using System.Linq;

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
        private readonly Delegate factory;

        /// <summary>
        /// The parameter type.
        /// </summary>
        private readonly Type parameterType;

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
        public CustomFactoryFragment([NotNull] Type source, [NotNull] Type target, [NotNull] Delegate factory)
            : base(source, target)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            var parameters = factory.Method.GetParameters();
            if (parameters.Count() != 1)
            {
                throw new ArgumentException(
                    string.Format(
                        "Expected 1 parameter, but {0} parameters found: {1}",
                        parameters.Count(),
                        string.Join(", ", parameters.Select(item => item.ParameterType))),
                    "factory");
            }

            this.parameterType = parameters[0].ParameterType;
            var expectedParameterType = ParametersTypeTemplate.MakeGenericType(source);
            if (!expectedParameterType.IsAssignableFrom(this.parameterType))
            {
                throw new ArgumentException(
                    string.Format(
                        "Parameter type '{0}' is not an assignable to '{1}'.", 
                        this.parameterType,
                        expectedParameterType), 
                    "factory");
            }

            if (!target.IsAssignableFrom(factory.Method.ReturnType))
            {
                throw new ArgumentException(
                    string.Format(
                        "Return type '{0}' is not an assignable to '{1}'.",
                        factory.Method.ReturnType,
                        target),
                    "factory");
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
        public Delegate Factory
        {
            get { return this.factory; }
        }

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public Type ParameterType
        {
            get { return this.parameterType; }
        }
    }
}