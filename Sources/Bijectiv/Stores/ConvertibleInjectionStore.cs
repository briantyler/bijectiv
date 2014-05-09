// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleInjectionStore.cs" company="Bijectiv">
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
//   Defines the ConvertibleInjectionStore type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.Injections;

    using JetBrains.Annotations;

    /// <summary>
    /// A store that contains <see cref="ITransform"/> instances that transform between types that implement 
    /// <see cref="IConvertible"/>.
    /// </summary>
    public class ConvertibleInjectionStore : IInjectionStore
    {
        /// <summary>
        /// Resolves a <see cref="ITransform"/> that transforms instances of type <paramref name="source"/> into
        /// instances of type <paramref name="target"/> if one exists, or; returns NULL otherwise.
        /// </summary>
        /// <param name="source">
        /// The source type.
        /// </param>
        /// <param name="target">
        /// The target type.
        /// </param>
        /// <returns>
        /// A <see cref="ITransform"/> that transforms instances of type <paramref name="source"/> into
        /// instances of type <paramref name="target"/> if one exists, or; NULL otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any parameter is null.
        /// </exception>
        public ITransform Resolve([NotNull] Type source, [NotNull] Type target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (typeof(IConvertible).IsAssignableFrom(source) && Type.GetTypeCode(target) != TypeCode.Object)
            {
                return new ConvertibleTransform(target);
            }

            return null;
        }

        public TInjection Resolve<TInjection>(Type source, Type target) where TInjection : IInjection
        {
            throw new NotImplementedException();
        }
    }
}