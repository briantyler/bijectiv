// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionDefinitionBuilder.cs" company="Bijectiv">
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
//   Defines the InjectionDefinitionBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelBuilder
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Injections;

    using JetBrains.Annotations;

    /// <summary>
    /// A builder of <see cref="InjectionDefinition"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public class InjectionDefinitionBuilder<TSource, TTarget> : IInjectionDefinitionBuilder<TSource, TTarget>
    {
        /// <summary>
        /// The definition being built.
        /// </summary>
        private readonly InjectionDefinition definition;

        /// <summary>
        /// The instance registry to which <see cref="definition"/> belongs.
        /// </summary>
        private readonly IInstanceRegistry registry;

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionDefinitionBuilder{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="definition">
        /// The definition being built.
        /// </param>
        /// <param name="registry">
        /// The instance registry to which <see cref="definition"/> belongs.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="definition"/> is incompatible with the builder.
        /// </exception>
        public InjectionDefinitionBuilder(
            [NotNull] InjectionDefinition definition, 
            [NotNull] IInstanceRegistry registry)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (definition.Source != typeof(TSource))
            {
                throw new ArgumentException(
                    string.Format("Source '{0}' must match TSource '{1}'", definition.Source, typeof(TSource)),
                    "definition");
            }

            if (definition.Target != typeof(TTarget))
            {
                throw new ArgumentException(
                    string.Format("Target '{0}' must match TTarget '{1}'", definition.Target, typeof(TTarget)),
                    "definition");
            }

            this.definition = definition;
            this.registry = registry;
        }

        /// <summary>
        /// Gets the definition being built.
        /// </summary>
        protected internal InjectionDefinition Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Gets the instance registry to which <see cref="definition"/> belongs.
        /// </summary>
        protected internal IInstanceRegistry Registry
        {
            get { return this.registry; }
        }

        /// <summary>
        /// Instructs the injection to construct the target via activation.
        /// </summary>
        /// <remarks>
        /// This is the default option if no other construction method is specified.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public IInjectionDefinitionBuilder<TSource, TTarget> Activate()
        {
            this.Definition.Add(new ActivateFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the injection to construct the target via the default factory.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public IInjectionDefinitionBuilder<TSource, TTarget> DefaultFactory()
        {
            this.Definition.Add(new DefaultFactoryFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the injection to construct the target via a custom factory delegate.
        /// </summary>
        /// <param name="factory">
        /// The factory delegate that constructs the target.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public IInjectionDefinitionBuilder<TSource, TTarget> CustomFactory(
            Func<CustomFactoryParameters<TSource>, TTarget> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.Definition.Add(new CustomFactoryFragment(typeof(TSource), typeof(TTarget), factory));
            return this;
        }

        /// <summary>
        /// Instructs the injection to not automatically inject any source member to a target member: only
        /// explicit member injections will be used.
        /// </summary>
        /// <remarks>
        /// This may be useful in the case of an inherited injection when the auto rules defined in the base
        /// injection non longer make sense and need to be cancelled, or when the source and target have very
        /// different structures.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoNone()
        {
            var fragment = new AutoInjectionFragment(
                typeof(TSource), typeof(TTarget), new NullAutoInjectionStrategy());

            this.Definition.Add(fragment);

            return this;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoExact()
        {
            return this.AutoRegex(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$",
                AutoInjectionOptions.None);
        }

        /// <summary>
        /// Instructs the injection to automatically inject a source member to a target member whenever
        /// a source member's name prefixed by <paramref name="prefix"/> is equal to a target member's name.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>automatically injection
        /// <example>
        /// The following is a match:
        ///     Prefix = 'Foo'
        ///     Source = 'Bar'
        ///     Target = 'FooBar'.
        /// </example>
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoPrefixSource(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            return this.AutoRegex(
                "^" + prefix + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$", 
                AutoInjectionOptions.MatchTarget);
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoPrefixTarget(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            return this.AutoRegex(
                "^" + prefix + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$", 
                AutoInjectionOptions.None);
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoSuffixSource(string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            return this.AutoRegex(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + suffix + "$", 
                AutoInjectionOptions.MatchTarget);
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoSuffixTarget(string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            return this.AutoRegex(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + suffix + "$", 
                AutoInjectionOptions.None);
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> AutoRegex(string regex, AutoInjectionOptions options)
        {
            if (regex == null)
            {
                throw new ArgumentNullException("regex");
            }

            var strategy = new NameRegexAutoInjectionStrategy(regex, options);
            var fragment = new AutoInjectionFragment(typeof(TSource), typeof(TTarget), strategy);
            this.Definition.Add(fragment);

            return this;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> NullSourceThrow(
            Func<IInjectionContext, Exception> exceptionFactory)
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException("exceptionFactory");
            }

            Func<IInjectionContext, TTarget> factory = context => { throw exceptionFactory(context); };
            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Instructs the injection to set the target to its default value (<see langword="default" />) if the 
        /// source instance is <c>null</c>.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        public IInjectionDefinitionBuilder<TSource, TTarget> NullSourceDefault()
        {
            Func<IInjectionContext, TTarget> factory = context => default(TTarget);
            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> NullSourceCustom(
            Func<IInjectionContext, TTarget> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> MergeOnKey(
            Func<TSource, object> sourceKeySelector,
            Func<TTarget, object> targetKeySelector)
        {
            if (sourceKeySelector == null)
            {
                throw new ArgumentNullException("sourceKeySelector");
            }

            if (targetKeySelector == null)
            {
                throw new ArgumentNullException("targetKeySelector");
            }

            Func<ITargetFinder> factory = () =>
                new IdenticalKeyTargetFinder(
                    s => sourceKeySelector((TSource)s),
                    t => targetKeySelector((TTarget)t),
                    EqualityComparer<object>.Default);

            this.Registry.Register(
                typeof(TargetFinderRegistration),
                new TargetFinderRegistration(typeof(TSource), typeof(TTarget), factory));

            return this;
        }

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
        public IInjectionDefinitionBuilder<TSource, TTarget> OnInjectionEnded(
            Action<IInjectionTriggerParameters<TSource, TTarget>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var trigger = new DelegateInjectionTrigger(p => action((IInjectionTriggerParameters<TSource, TTarget>)p));
            var fragment = new InjectionTriggerFragment(
                typeof(TSource), typeof(TTarget), trigger, InjectionTriggerCause.InjectionEnded);
            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Registers an action with the injection that will be executed immediately before the injection ends, 
        /// whenever the injection target is being injected as a member of a collection.
        /// </summary>
        /// <param name="action">
        /// The action to execute where the <see cref="int"/> parameter is the index of 
        /// <see cref="IInjectionTriggerParameters{TSource,TTarget}.Target"/> in its parent collection.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the injection.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public IInjectionDefinitionBuilder<TSource, TTarget> OnCollectionItem(
            Action<int, IInjectionTriggerParameters<TSource, TTarget>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Action<IInjectionTriggerParameters<TSource, TTarget>> indexedAction = p =>
                {
                    var hint = p.Hint as EnumerableInjectionHint;
                    if (hint == null)
                    {
                        return;
                    }

                    action(hint.Index, p);
                };

            return this.OnInjectionEnded(indexedAction);
        } 
    }
}