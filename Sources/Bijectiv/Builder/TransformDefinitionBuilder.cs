// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformDefinitionBuilder.cs" company="Bijectiv">
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
//   Defines the TransformDefinitionBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Builder
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// A builder of <see cref="TransformDefinition"/> instances.
    /// </summary>
    /// <typeparam name="TSource">
    /// The source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// The target type.
    /// </typeparam>
    public class TransformDefinitionBuilder<TSource, TTarget> : ITransformDefinitionBuilder<TSource, TTarget>
    {
        /// <summary>
        /// The definition being built.
        /// </summary>
        private readonly TransformDefinition definition;

        /// <summary>
        /// Initialises a new instance of the <see cref="TransformDefinitionBuilder{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="definition">
        /// The definition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="definition"/> is incompatible with the builder.
        /// </exception>
        public TransformDefinitionBuilder([NotNull] TransformDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
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
        }

        /// <summary>
        /// Gets the definition being built.
        /// </summary>
        protected internal TransformDefinition Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Instructs the transform to construct the target via activation.
        /// </summary>
        /// <remarks>
        /// This is the default option if no other construction method is specified.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> Activate()
        {
            this.Definition.Add(new ActivateFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the transform to construct the target via the default factory.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> DefaultFactory()
        {
            this.Definition.Add(new DefaultFactoryFragment(typeof(TSource), typeof(TTarget)));
            return this;
        }

        /// <summary>
        /// Instructs the transform to construct the target via a custom factory delegate.
        /// </summary>
        /// <param name="factory">
        /// The factory delegate that constructs the target.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> CustomFactory(
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
        /// Instructs the transform to not automatically transform any source member to a target member: only
        /// explicit member transforms will be used.
        /// </summary>
        /// <remarks>
        /// This may be useful in the case of an inherited transform when the auto rules defined in the base
        /// transform non longer make sense and need to be cancelled, or when the source and target have very
        /// different structures.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoNone()
        {
            var fragment = new AutoTransformFragment(
                typeof(TSource), typeof(TTarget), new NullAutoTransformStrategy());

            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// they have exactly the same, case sensitive, name.
        /// </summary>
        /// <remarks>
        /// This is the default option if no other auto method is specified.
        /// </remarks>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoExact()
        {
            return this.AutoRegex(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + "$",
                AutoTransformOptions.None);
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// a source member's name prefixed by <paramref name="prefix"/> is equal to a target member's name.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Prefix = 'Foo'
        ///     Source = 'Bar'
        ///     Target = 'FooBar'.
        /// </example>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoPrefixSource(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            return this.AutoRegex(
                "^" + prefix + NameRegexAutoTransformStrategy.NameTemplateParameter + "$", 
                AutoTransformOptions.MatchTarget);
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// a target member's name prefixed by <paramref name="prefix"/> is equal to a source member's name.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Prefix = 'Foo'
        ///     Source = 'FooBar'
        ///     Target = 'Bar'.
        /// </example>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoPrefixTarget(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            return this.AutoRegex(
                "^" + prefix + NameRegexAutoTransformStrategy.NameTemplateParameter + "$", 
                AutoTransformOptions.None);
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// a source member's name suffixed by <paramref name="suffix"/> is equal to a target member's name.
        /// </summary>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Suffix = 'Bar'
        ///     Source = 'Foo'
        ///     Target = 'FooBar'.
        /// </example>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoSuffixSource(string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            return this.AutoRegex(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + suffix + "$", 
                AutoTransformOptions.MatchTarget);
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// a target member's name suffixed by <paramref name="suffix"/> is equal to a source member's name.
        /// </summary>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <example>
        /// The following is a match:
        ///     Suffix = 'Bar'
        ///     Source = 'FooBar'
        ///     Target = 'Foo'.
        /// </example>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoSuffixTarget(string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            return this.AutoRegex(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + suffix + "$", 
                AutoTransformOptions.None);
        }

        /// <summary>
        /// Instructs the transform to automatically transform a source member to a target member whenever
        /// there is a regex match between the two names.
        /// </summary>
        /// <param name="regex">
        /// The regex with which to match.
        /// Note that the magic constant  <see cref="NameRegexAutoTransformStrategy.NameTemplateParameter"/> gets 
        /// substituted for the name of the source member when <see cref="AutoTransformOptions.MatchTarget"/> is set and 
        /// substituted for the target member's name otherwise.
        /// </param>
        /// <param name="options">
        /// The auto transform options.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> AutoRegex(string regex, AutoTransformOptions options)
        {
            if (regex == null)
            {
                throw new ArgumentNullException("regex");
            }

            var strategy = new NameRegexAutoTransformStrategy(regex, options);
            var fragment = new AutoTransformFragment(typeof(TSource), typeof(TTarget), strategy);
            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Instructs the transform to throw an exception if the source instance is <c>NULL</c>.
        /// </summary>
        /// <param name="exceptionFactory">
        /// The exception factory.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ITransformDefinitionBuilder<TSource, TTarget> NullSourceThrow(
            Func<ITransformContext, Exception> exceptionFactory)
        {
            if (exceptionFactory == null)
            {
                throw new ArgumentNullException("exceptionFactory");
            }

            Func<ITransformContext, TTarget> factory = context => { throw exceptionFactory(context); };
            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Instructs the transform to set the target to its default value (<see langword="default" />) if the 
        /// source instance is <c>NULL</c>.
        /// </summary>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        public ITransformDefinitionBuilder<TSource, TTarget> NullSourceDefault()
        {
            Func<ITransformContext, TTarget> factory = context => default(TTarget);
            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }

        /// <summary>
        /// Instructs the transform to create the target from a custom factory if the source instance is <c>NULL</c>.
        /// </summary>
        /// <param name="factory">
        /// The custom factory.
        /// </param>
        /// <returns>
        /// An object that allows further configuration of the transform.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public ITransformDefinitionBuilder<TSource, TTarget> NullSourceCustom(
            Func<ITransformContext, TTarget> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            var fragment = new NullSourceFragment(typeof(TSource), typeof(TTarget), factory);
            this.Definition.Add(fragment);

            return this;
        }
    }
}