// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateSourceExpressionFactory.cs" company="Bijectiv">
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
//   Defines the DelegateSourceExpressionFactory type.
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
    /// A factory that produces an expression from a <see cref="DelegateSourceMemberShard"/> that invokes the delegate
    /// from the shard.
    /// </summary>
    public class DelegateSourceExpressionFactory : ISourceExpressionFactory<DelegateSourceMemberShard>
    {
        /// <summary>
        /// Creates an expression that provides a source object from a <see cref="DelegateSourceMemberShard"/>.
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
        public Expression Create(InjectionScaffold scaffold, MemberFragment fragment, DelegateSourceMemberShard shard)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            var parameters = scaffold.Variables
                .Single(item => item.Name.Equals("injectionParameters", StringComparison.OrdinalIgnoreCase));

            return shard.Delegate.Method.IsStatic
                       ? Expression.Call(shard.Delegate.Method, Expression.Convert(parameters, shard.ParameterType))
                       : Expression.Call(
                           Expression.Constant(shard.Delegate.Target),
                           shard.Delegate.Method,
                           Expression.Convert(parameters, shard.ParameterType));
        }
    }
}