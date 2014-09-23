// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISourceExpressionFactory.cs" company="Bijectiv">
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
//   Defines the ISourceExpressionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a factory that produces an expression from a <see cref="SourceMemberShard"/>.
    /// </summary>
    /// <typeparam name="TShard">
    /// The type of shard from which to create the expression.
    /// </typeparam>
    public interface ISourceExpressionFactory<in TShard> 
        where TShard : SourceMemberShard
    {
        /// <summary>
        /// Creates an expression that provides a source object from a shard of type <typeparamref name="TShard"/>.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the injection is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment to which the shard belongs.
        /// </param>
        /// <param name="shard">
        /// The shard from which to produce the expression.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/> that provides the source object.
        /// </returns>
        Expression Create(
            [NotNull] InjectionScaffold scaffold, 
            [NotNull] MemberFragment fragment, 
            [NotNull] TShard shard);
    }
}