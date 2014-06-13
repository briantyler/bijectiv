﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateInjectionTrigger.cs" company="Bijectiv">
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
//   Defines the DelegateInjectionTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Injections
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="IInjectionTrigger"/> that performs its trigger action via a delegate.
    /// </summary>
    public class DelegateInjectionTrigger : IInjectionTrigger
    {
        /// <summary>
        /// The trigger action.
        /// </summary>
        private readonly Action<IInjectionTriggerParameters> trigger;

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateInjectionTrigger"/> class.
        /// </summary>
        /// <param name="trigger">
        /// The trigger action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public DelegateInjectionTrigger([NotNull] Action<IInjectionTriggerParameters> trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }

            this.trigger = trigger;
        }

        /// <summary>
        /// Gets the trigger action.
        /// </summary>
        public Action<IInjectionTriggerParameters> Trigger
        {
            get { return this.trigger; }
        }

        /// <summary>
        /// Pulls the trigger.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void Pull([NotNull] IInjectionTriggerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            this.Trigger(parameters);
        }
    }
}