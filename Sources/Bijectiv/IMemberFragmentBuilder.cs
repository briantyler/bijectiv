// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemberFragmentBuilder.cs" company="Bijectiv">
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
//   Defines the IMemberFragmentBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a builder of <see cref="MemberFragment"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    /// <typeparam name="TMember">
    /// The type of the target member referenced by the fragment.
    /// </typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes",
        Justification = "By design.")]
    public interface IMemberFragmentBuilder<TSource, TTarget, TMember>
    {
        /// <summary>
        /// Registers a condition that must be met for the member to be injected.
        /// </summary>
        /// <param name="predicate">
        /// The predicate that defines the condition.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IMemberFragmentBuilder<TSource, TTarget, TMember> Condition(
            [NotNull] Func<IInjectionParameters<TSource, TTarget>, bool> predicate);

        /// <summary>
        /// Ignores the member.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> Ignore();

        /// <summary>
        /// Injects a value into the member using the run-time type of <paramref name="value"/> to determine how to
        /// inject it into the target member.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> InjectValue([NotNull] object value);

        /// <summary>
        /// Injects the result of an <paramref name="expression"/>, that takes the source object as input, into
        /// the member using the run-time type of the <paramref name="expression"/> result to determine how to inject 
        /// it into the target member.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of the expression; should be automatically inferred from the 
        /// <paramref name="expression"/>.
        /// </typeparam>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> InjectSource<TResult>(
            Expression<Func<TSource, TResult>> expression);

        /// <summary>
        /// Injects the result of a <paramref name="delegate"/>, that takes 
        /// <see cref="IInjectionParameters{TSource,TTarget}"/> as input, into
        /// the member using the run-time type of the <paramref name="delegate"/> result to determine how to inject it 
        /// into the target member.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of the delegate; should be automatically inferred from the 
        /// <paramref name="delegate"/>.
        /// </typeparam>
        /// <param name="delegate">
        /// The delegate.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> InjectDelegate<TResult>(
            Func<IInjectionParameters<TSource, TTarget>, TResult> @delegate);

        /// <summary>
        /// Assigns a value directly to the member.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AssignValue(TMember value);

        /// <summary>
        /// Assigns the result of an <paramref name="expression"/>, that takes the source as input, directly to the
        /// member.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AssignSource(
            Expression<Func<TSource, TMember>> expression);

        /// <summary>
        /// Assigns the result of a <paramref name="delegate"/>, that takes 
        /// <see cref="IInjectionParameters{TSource,TTarget}"/> as input, directly to the member.
        /// </summary>
        /// <param name="delegate">
        /// The delegate.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AssignDelegate(
            Func<IInjectionParameters<TSource, TTarget>, TMember> @delegate);
    }
}