// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FallbackFactoryExpressionFactory.cs" company="Bijectiv">
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
//   Defines the FallbackFactoryExpressionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Builder;

    /// <summary>
    /// An expression factory that creates an expression that creates a target by activation when no other factory 
    /// has created the target.
    /// </summary>
    public class FallbackFactoryExpressionFactory : ISelectiveExpressionFactory
    {
        /// <summary>
        /// Gets a value indicating whether the factory can create an expression from a 
        /// <see cref="TransformScaffold"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <returns>
        /// A value indicating whether the factory can create an expression from a 
        /// <see cref="TransformScaffold"/>.
        /// </returns>
        public bool CanCreateExpression(TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            return scaffold.ProcessedFragments.All(
                candidate => candidate.FragmentCategory != LegendryFragments.Factory);
        }

        /// <summary>
        /// Creates an expression from a <see cref="TransformScaffold"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <returns>
        /// The created <see cref="Expression"/>.
        /// </returns>
        public Expression CreateExpression(TransformScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            return Expression.New(scaffold.Definition.Target);
        }
    }
}