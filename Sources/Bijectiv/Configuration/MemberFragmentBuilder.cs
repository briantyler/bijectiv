// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberFragmentBuilder.cs" company="Bijectiv">
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
//   Defines the MemberFragmentBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    /// <summary>
    /// A builder of <see cref="MemberFragment"/> instances.
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
    public class MemberFragmentBuilder<TSource, TTarget, TMember> 
        : IMemberFragmentBuilder<TSource, TTarget, TMember>
    {
        /// <summary>
        /// The <see cref="InjectionDefinition"/> builder that spawned this instance.
        /// </summary>
        private readonly IInjectionDefinitionBuilder<TSource, TTarget> builder;

        /// <summary>
        /// The fragment that is being built.
        /// </summary>
        private readonly MemberFragment fragment;

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberFragmentBuilder{TSource,TTarget,TMember}"/> class.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="InjectionDefinition"/> builder that is spawning this instance.
        /// </param>
        /// <param name="fragment">
        /// The fragment that is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public MemberFragmentBuilder(
            [NotNull] IInjectionDefinitionBuilder<TSource, TTarget> builder,
            [NotNull] MemberFragment fragment)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            this.builder = builder;
            this.fragment = fragment;
        }

        /// <summary>
        /// Gets the <see cref="InjectionDefinition"/> builder that spawned this instance.
        /// </summary>
        public IInjectionDefinitionBuilder<TSource, TTarget> Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the fragment that is being built.
        /// </summary>
        public MemberFragment Fragment
        {
            get { return this.fragment; }
        }

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
        public virtual IMemberFragmentBuilder<TSource, TTarget, TMember> Condition(
            Func<IInjectionParameters<TSource, TTarget>, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            this.Fragment.Add(
                new PredicateConditionMemberShard(
                    typeof(TSource), 
                    typeof(TTarget), 
                    this.Fragment.Member,
                    predicate));

            return this;
        }

        /// <summary>
        /// Ignores the member.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> Ignore()
        {
            this.Condition(p => false);
            return this.Builder;
        }

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
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> InjectValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    "value",
                    "To set the member to NULL use AssignValue(null) instead.");
            }

            this.Fragment.Add(
                new ValueSourceMemberShard(
                    typeof(TSource), 
                    typeof(TTarget), 
                    this.Fragment.Member,
                    value, 
                    true));

            return this.Builder;
        }

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
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> InjectSource<TResult>(
            [NotNull] Expression<Func<TSource, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.Fragment.Add(
                new ExpressionSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.Fragment.Member,
                    expression, 
                    true));

            return this.Builder;
        }

        /// <summary>
        /// Injects the result of a delegate, that takes <see cref="IInjectionParameters{TSource,TTarget}"/> as input, into
        /// the member using the run-time type of the delegate result to determine how to inject it 
        /// into the target member.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result of the delegate; should be automatically inferred from the <paramref name="func"/>.
        /// </typeparam>
        /// <param name="func">
        /// The delegate from which to inject.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> InjectDelegate<TResult>(
            [NotNull] Func<IInjectionParameters<TSource, TTarget>, TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            this.Fragment.Add(
                new DelegateSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.Fragment.Member,
                    func, 
                    true));

            return this.Builder;
        }

        /// <summary>
        /// Assigns a value directly to the member.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> AssignValue(TMember value)
        {
            this.Fragment.Add(
                new ValueSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.Fragment.Member,
                    value,
                    false));

            return this.Builder;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AssignSource(
            [NotNull] Expression<Func<TSource, TMember>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this.Fragment.Add(
                new ExpressionSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.Fragment.Member,
                    expression,
                    false));

            return this.Builder;
        }

        /// <summary>
        /// Assigns the result of a delegate, that takes  <see cref="IInjectionParameters{TSource,TTarget}"/> as input,
        /// directly to the member.
        /// </summary>
        /// <param name="func">
        /// The delegate from which to assign.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public virtual IInjectionDefinitionBuilder<TSource, TTarget> AssignDelegate(
            [NotNull] Func<IInjectionParameters<TSource, TTarget>, TMember> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            this.Fragment.Add(
                new DelegateSourceMemberShard(
                    typeof(TSource),
                    typeof(TTarget),
                    this.Fragment.Member,
                    func, 
                    false));

            return this.Builder;
        }
    }
}