﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectionDefinitionBuilder.cs" company="Bijectiv">
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
//   Defines the IInjectionDefinitionBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// Represents a builder of <see cref="InjectionDefinition"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IInjectionDefinitionBuilder<TSource, TTarget>
    {
        /// <summary>
        /// Instructs the injection to construct the target via activation.
        /// </summary>
        /// <remarks>
        /// This is the default option if no other construction method is specified.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> Activate();

        /// <summary>
        /// Instructs the injection to construct the target via the default factory.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> DefaultFactory();

        /// <summary>
        /// Instructs the injection to construct the target via a custom factory delegate.
        /// </summary>
        /// <param name="factory">
        /// The factory delegate that constructs the target.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> CustomFactory(
            Func<CustomFactoryParameters<TSource>, TTarget> factory);

        /// <summary>
        /// Instructs the injection to not automatically inject any source member to a target member: only
        /// explicit member transforms will be used.
        /// </summary>
        /// <remarks>
        /// This may be useful in the case of an inherited injection when the auto rules defined in the base
        /// injection non longer make sense and need to be cancelled, or when the source and target have very
        /// different structures.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoNone();

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// they have exactly the same, case sensitive, name.
        /// </summary>
        /// <remarks>
        /// This is the default option if no other auto method is specified.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoExact();

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// a source member's name prefixed by <paramref name="prefix"/> is equal to a target member's name.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Prefix = 'Foo'
        ///     Source = 'Bar'
        ///     Target = 'FooBar'.
        /// </example>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoPrefixSource([NotNull] string prefix);

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// a target member's name prefixed by <paramref name="prefix"/> is equal to a source member's name.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Prefix = 'Foo'
        ///     Source = 'FooBar'
        ///     Target = 'Bar'.
        /// </example>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoPrefixTarget([NotNull] string prefix);

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// a source member's name suffixed by <paramref name="suffix"/> is equal to a target member's name.
        /// </summary>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Suffix = 'Bar'
        ///     Source = 'Foo'
        ///     Target = 'FooBar'.
        /// </example>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoSuffixSource([NotNull] string suffix);

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// a target member's name suffixed by <paramref name="suffix"/> is equal to a source member's name.
        /// </summary>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Suffix = 'Bar'
        ///     Source = 'FooBar'
        ///     Target = 'Foo'.
        /// </example>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoSuffixTarget([NotNull] string suffix);

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// there is a regex match between the two names.
        /// </summary>
        /// <param name="regex">
        /// The regex with which to match.
        /// Note that the magic constant  <see cref="NameRegexAutoInjectionStrategy.NameTemplateParameter"/> gets 
        /// substituted for the name of the source member when <see cref="AutoInjectionOptions.MatchTarget"/> is set and 
        /// substituted for the target member's name otherwise.
        /// </param>
        /// <param name="options">
        /// The auto injection options.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> AutoRegex([NotNull] string regex, AutoInjectionOptions options);

        /// <summary>
        /// Instructs the injection to throw an exception if the source instance is <c>null</c>.
        /// </summary>
        /// <param name="exceptionFactory">
        /// The exception factory.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> NullSourceThrow(
            [NotNull] Func<IInjectionContext, Exception> exceptionFactory);

        /// <summary>
        /// Instructs the injection to set the target to its default value (<see langword="default" />) if the 
        /// source instance is <c>null</c>.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        IInjectionDefinitionBuilder<TSource, TTarget> NullSourceDefault();

        /// <summary>
        /// Instructs the injection to create the target from a custom factory if the source instance is <c>null</c>.
        /// </summary>
        /// <param name="factory">
        /// The custom factory.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> NullSourceCustom(
            [NotNull] Func<IInjectionContext, TTarget> factory);

        /// <summary>
        /// Instructs a merge from a collection of <typeparamref name="TSource"/> to a collection of 
        /// <typeparamref name="TTarget"/> to match source with target on the key returned by 
        /// <paramref name="sourceKeySelector"/> and <paramref name="targetKeySelector"/> respectively.
        /// </summary>
        /// <param name="sourceKeySelector">
        /// The source key locator.
        /// </param>
        /// <param name="targetKeySelector">
        /// The target key locator.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> MergeOnKey(
            [NotNull] Func<TSource, object> sourceKeySelector,
            [NotNull] Func<TTarget, object> targetKeySelector);

        /// <summary>
        /// Registers an action with the <see cref="IInjection"/>.
        /// </summary>
        /// <param name="trigger">
        /// The trigger.
        /// </param>
        /// <param name="triggeredBy">
        /// The reason that the <paramref name="trigger"/> is invoked.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> RegisterTrigger(
            [NotNull] Action<IInjectionParameters<TSource, TTarget>> trigger, 
            TriggeredBy triggeredBy);

        /// <summary>
        /// Registers an action with the injection that will be executed immediately before the injection ends.
        /// </summary>
        /// <param name="action">
        /// The action to execute.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> OnInjectionEnded(
            [NotNull] Action<IInjectionParameters<TSource, TTarget>> action);

        /// <summary>
        /// Registers an action with the injection that will be executed immediately before the injection ends, 
        /// whenever the injection target is being injected as a member of a collection.
        /// </summary>
        /// <param name="action">
        /// The action to execute where the <see cref="int"/> parameter is the index of 
        /// <see cref="IInjectionParameters{TSource,TTarget}.Target"/> in its parent collection.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IInjectionDefinitionBuilder<TSource, TTarget> OnCollectionItem(
            [NotNull] Action<int, IInjectionParameters<TSource, TTarget>> action);

        /// <summary>
        /// Registers an injection for a target member.
        /// </summary>
        /// <param name="member">
        /// The target member for which to register an injection.
        /// </param>
        /// <typeparam name="TMember">
        /// The type of the target member.
        /// </typeparam>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        IMemberFragmentBuilder<TSource, TTarget, TMember> InjectMember<TMember>(
            [NotNull] Expression<Func<TTarget, TMember>> member);
    }
}