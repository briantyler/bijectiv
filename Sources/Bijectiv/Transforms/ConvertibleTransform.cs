// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleTransform.cs" company="Bijectiv">
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
//   Defines the ConvertibleTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Transforms
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a transform from an instance of type <see cref="IConvertible"/> to one of the primitive types.
    /// </summary>
    public class ConvertibleTransform : ITransform
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ConvertibleTransform"/> class.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ConvertibleTransform([NotNull] Type target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (Type.GetTypeCode(target) == TypeCode.Object)
            {
                var message = string.Format("Target type '{0}' must not have Type Code 'Object'", target);
                throw new ArgumentException(message, "target");
            }

            this.Target = target;
        }

        /// <summary>
        /// Gets the source type supported by the transform.
        /// </summary>
        public Type Source 
        { 
            get { return typeof(IConvertible); }
        }

        /// <summary>
        /// Gets the target type created by the transform.
        /// </summary>
        public Type Target { get; private set; }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <see cref="ITransform.Target"/> via
        /// <see cref="Convert.ChangeType"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <returns>
        /// The newly minted target instance.
        /// </returns>
        public object Transform(object source, TransformContext context)
        {
            return Convert.ChangeType(source, this.Target, context.Culture);
        }
    }
}