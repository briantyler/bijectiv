﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivateTargetExpressionFactory.cs" company="Bijectiv">
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
//   Defines the ActivateTargetExpressionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// An expression factory that creates an expression that creates a target by activation.
    /// </summary>
    public class ActivateTargetExpressionFactory : ISelectiveExpressionFactory
    {
        /// <summary>
        /// Gets a value indicating whether the factory can create an expression from a 
        /// <see cref="InjectionScaffold"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <returns>
        /// A value indicating whether the factory can create an expression from a 
        /// <see cref="InjectionScaffold"/>.
        /// </returns>
        public bool CanCreateExpression([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            return scaffold
                .UnprocessedFragments
                .FirstOrDefault(candidate => candidate.FragmentCategory == LegendaryFragments.Factory)
                is ActivateFragment;
        }

        /// <summary>
        /// Creates an expression from a <see cref="InjectionScaffold"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <returns>
        /// The created <see cref="Expression"/>.
        /// </returns>
        public Expression CreateExpression([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            return Expression.New(scaffold.Definition.Target);
        }
    }
}