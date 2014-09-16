// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTriggerFragment.cs" company="Bijectiv">
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
//   Defines the InjectionTriggerFragment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Linq;

    using JetBrains.Annotations;

    /// <summary>
    /// Specifies that a delegate is invoked when <see cref="TriggeredBy"/> occurs.
    /// </summary>
    public class InjectionTriggerFragment : InjectionFragment
    {
        /// <summary>
        /// The parameters type template.
        /// </summary>
        private static readonly Type ParametersTypeTemplate = typeof(IInjectionParameters<,>);

        /// <summary>
        /// The reason that <see cref="trigger"/> is invoked.
        /// </summary>
        private readonly TriggeredBy triggeredBy;

        /// <summary>
        /// The delegate to invoke when when <see cref="InjectionTriggerFragment.TriggeredBy"/> occurs.
        /// </summary>
        private readonly Delegate trigger;

        private readonly Type parameterType;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionTriggerFragment"/> class.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <param name="trigger">
        /// The delegate to invoke when <paramref name="triggeredBy"/> occurs.
        /// </param>
        /// <param name="triggeredBy">
        /// The reason that <paramref name="trigger"/> is invoked.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionTriggerFragment(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] Delegate trigger,
            TriggeredBy triggeredBy)
            : base(source, target)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            var parameters = trigger.Method.GetParameters();
            if (parameters.Count() != 1)
            {
                throw new ArgumentException(
                    string.Format(
                        "Expected 1 parameter, but {0} parameters found: {1}",
                        parameters.Count(),
                        string.Join(", ", parameters.Select(item => item.ParameterType))),
                    "trigger");
            }

            this.parameterType = parameters[0].ParameterType;
            var expectedParameterType = ParametersTypeTemplate.MakeGenericType(source, target);
            if (!expectedParameterType.IsAssignableFrom(this.ParameterType))
            {
                throw new ArgumentException(
                    string.Format(
                        "Parameter type '{0}' is not an assignable to '{1}'.",
                        this.ParameterType,
                        expectedParameterType),
                    "target");
            }

            this.trigger = trigger;
            this.triggeredBy = triggeredBy;
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
            get { return LegendaryFragments.Trigger; }
        }

        /// <summary>
        /// Gets the reason that <see cref="Trigger"/> is invoked.
        /// </summary>
        public TriggeredBy TriggeredBy
        {
            get { return this.triggeredBy; }
        }

        /// <summary>
        /// Gets the delegate to invoke when <see cref="InjectionTriggerFragment.TriggeredBy"/> occurs.
        /// </summary>
        public Delegate Trigger
        {
            get { return this.trigger; }
        }

        public Type ParameterType
        {
            get
            {
                return this.parameterType;
            }
        }
    }
}