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

namespace Bijectiv.KernelBuilder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Specifies that a <see cref="IInjectionTrigger"/> is pulled when <see cref="TriggeredBy"/> occurs.
    /// </summary>
    public class InjectionTriggerFragment : InjectionFragment
    {
        /// <summary>
        /// The reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </summary>
        private readonly TriggeredBy triggeredBy;

        /// <summary>
        /// The trigger to pull when <see cref="InjectionTriggerFragment.TriggeredBy"/> occurs.
        /// </summary>
        private readonly IInjectionTrigger trigger;

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
        /// The trigger to pull when <paramref name="triggeredBy"/> occurs.
        /// </param>
        /// <param name="triggeredBy">
        /// The reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionTriggerFragment(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] IInjectionTrigger trigger,
            TriggeredBy triggeredBy)
            : base(source, target)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.triggeredBy = triggeredBy;
            this.trigger = trigger;
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
            get { return LegendryFragments.Trigger; }
        }

        /// <summary>
        /// Gets the reason that <see cref="IInjectionTrigger"/> is pulled.
        /// </summary>
        public TriggeredBy TriggeredBy
        {
            get { return this.triggeredBy; }
        }

        /// <summary>
        /// Gets the trigger to pull when <see cref="InjectionTriggerFragment.TriggeredBy"/> occurs.
        /// </summary>
        public IInjectionTrigger Trigger
        {
            get { return this.trigger; }
        }
    }
}