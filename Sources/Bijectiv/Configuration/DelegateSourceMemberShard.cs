// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateSourceMemberShard.cs" company="Bijectiv">
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
//   Defines the DelegateSourceMemberShard type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Kernel;

    using JetBrains.Annotations;

    /// <summary>
    /// A member shard that contains a delegate that that takes an instance of 
    /// <see cref="InjectionParameters{TSource,TTarget}"/>, where the source and target types match those of the shard,
    /// and returns an object from which the target member will be injected.
    /// </summary>
    public class DelegateSourceMemberShard : SourceMemberShard
    {
        /// <summary>
        /// The parameters type template.
        /// </summary>
        private static readonly Type ParametersTypeTemplate = typeof(IInjectionParameters<,>);

        /// <summary>
        /// The delegate that returns the member source.
        /// </summary>
        private readonly Delegate @delegate;

        /// <summary>
        /// The type of the parameter to the expression.
        /// </summary>
        private readonly Type parameterType;

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateSourceMemberShard"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="member">
        /// The member.
        /// </param>
        /// <param name="delegate">
        /// The delegate that returns the member source.
        /// </param>
        /// <param name="inject">
        /// A value indicating whether to inject the source; when false the source will be assigned to the target 
        /// member, not injected.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the delgate has the wrong signature.
        /// </exception>
        public DelegateSourceMemberShard(
            [NotNull] Type source, 
            [NotNull] Type target, 
            [NotNull] MemberInfo member, 
            [NotNull] Delegate @delegate, 
            bool inject)
            : base(source, target, member, inject)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            var parameters = @delegate.Method.GetParameters();
            if (parameters.Count() != 1)
            {
                throw new ArgumentException(
                    string.Format(
                    "Delegate with single parameter expected, but {0} parameters found",
                    parameters.Count()));
            }
            
            var parameter = parameters[0];
            this.parameterType = parameter.ParameterType;
            var expectedParameterType = ParametersTypeTemplate.MakeGenericType(source, target);
            if (!expectedParameterType.IsAssignableFrom(this.parameterType))
            {
                throw new ArgumentException(
                    string.Format(
                        "Parameter type '{0}' is not an assignable to '{1}'.",
                        this.parameterType,
                        expectedParameterType),
                    "delegate");
            }

            if (typeof(void) == @delegate.Method.ReturnType)
            {
                throw new ArgumentException("Delegate has no return value", "delegate");
            }
            
            this.@delegate = @delegate;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateSourceMemberShard"/> class.
        /// </summary>
        protected DelegateSourceMemberShard()
        {
        }

        /// <summary>
        /// Gets the delegate that returns the member source.
        /// </summary>
        public Delegate Delegate
        {
            get { return this.@delegate; }
        }

        /// <summary>
        /// Gets the type of the parameter to the delegate.
        /// </summary>
        public Type ParameterType
        {
            get { return this.parameterType; }
        }
    }
}