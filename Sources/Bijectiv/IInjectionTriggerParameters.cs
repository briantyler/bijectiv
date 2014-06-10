// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionTriggerParameters.cs" company="Bijectiv">
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
//   Defines the IInjectionTriggerParameters type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    /// <summary>
    /// The parameters to a <see cref="IInjectionTrigger"/>.
    /// </summary>
    public interface IInjectionTriggerParameters
    {
        /// <summary>
        /// Gets the source instance as an object.
        /// </summary>
        object SourceAsObject { get; }

        /// <summary>
        /// Gets the target instance as an object.
        /// </summary>
        object TargetAsObject { get; }

        /// <summary>
        /// Gets the context in which the trigger will take place.
        /// </summary>
        IInjectionContext Context { get; }

        /// <summary>
        /// Gets the hint that was passed to the <see cref="IInjection"/>.
        /// </summary>
        object Hint { get; }
    }

    /// <summary>
    /// The strongly typed parameters to a <see cref="IInjectionTrigger"/>.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public interface IInjectionTriggerParameters<out TSource, out TTarget> : IInjectionTriggerParameters
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        TSource Source { get; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        TTarget Target { get; }
    }
}