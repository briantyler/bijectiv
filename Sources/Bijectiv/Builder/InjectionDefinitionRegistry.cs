// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionDefinitionRegistry.cs" company="Bijectiv">
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
//   Defines the InjectionDefinitionRegistry type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="InjectionDefinition"/> registry.
    /// </summary>
    public class InjectionDefinitionRegistry : IInjectionDefinitionRegistry
    {
        /// <summary>
        /// The defintions.
        /// </summary>
        private readonly List<InjectionDefinition> defintions = new List<InjectionDefinition>();

        /// <summary>
        /// Returns an enumerator that iterates through the registry.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the registry.
        /// </returns>
        public IEnumerator<InjectionDefinition> GetEnumerator()
        {
            return this.defintions.GetEnumerator();
        }

        /// <summary>
        /// Adds a definition to the registry.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        public void Add([NotNull] InjectionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            this.defintions.Add(definition);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the registry.
        /// </summary>
        /// <returns>
        /// An enumerator that iterates through the registry.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.defintions).GetEnumerator();
        }
    }
}