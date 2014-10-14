// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITargetMemberInjector.cs" company="Bijectiv">
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
//   Defines the ITargetMemberInjector type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a service that augments the injection that is currently being built with an expression 
    /// that injects a source expression into a target member.
    /// </summary>
    public interface ITargetMemberInjector
    {
        /// <summary>
        /// Adds an expression to the scaffold that transforms the <paramref name="sourceExpression"/> into the 
        /// referenced <paramref name="member"/> of the <paramref name="scaffold"/> target.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="member">
        /// The member in the target that is the target of the transform.
        /// </param>
        /// <param name="sourceExpression">
        /// The source expression that will be transformed.
        /// </param>
        void AddTransformExpressionToScaffold(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberInfo member,
            [NotNull] Expression sourceExpression);

        /// <summary>
        /// Adds an expression to the scaffold that merges the <paramref name="sourceExpression"/> into the 
        /// referenced <paramref name="member"/> of the <paramref name="scaffold"/> target.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold.
        /// </param>
        /// <param name="member">
        /// The member in the target that is the target of the merge.
        /// </param>
        /// <param name="sourceExpression">
        /// The source expression that will be merged.
        /// </param>
        void AddMergeExpressionToScaffold(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberInfo member,
            [NotNull] Expression sourceExpression);
    }
}