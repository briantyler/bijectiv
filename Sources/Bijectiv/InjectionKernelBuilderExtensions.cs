// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernelBuilderExtensions.cs" company="Bijectiv">
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
//   Defines the InjectionKernelBuilderExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Extensions to the <see cref="InjectionKernelBuilder"/> class.
    /// </summary>
    public static class InjectionKernelBuilderExtensions
    {
        /// <summary>
        /// Registers a new injection with the builder.
        /// </summary>
        /// <param name="this">
        /// The <see cref="InjectionKernelBuilder"/> to register with.
        /// </param>
        /// <typeparam name="TSource">
        /// The source type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The target type.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IInjectionDefinitionBuilder{TSource,TTarget}"/> that allows further configuration of the 
        /// injection.
        /// </returns>
        public static IInjectionDefinitionBuilder<TSource, TTarget> Register<TSource, TTarget>(
            [NotNull] this InjectionKernelBuilder @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var definition = new InjectionDefinition(typeof(TSource), typeof(TTarget));

            @this.InstanceRegistry.Register(typeof(InjectionDefinition), definition);

            return new InjectionDefinitionBuilder<TSource, TTarget>(definition, @this.InstanceRegistry);
        }

        /// <summary>
        /// Registers a new injection with the builder and inherits all of the appropriate fragments from the 
        /// base injection if it exists.
        /// </summary>
        /// <param name="this">
        /// The <see cref="InjectionKernelBuilder"/> to register with.
        /// </param>
        /// <typeparam name="TSource">
        /// The source type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The target type.
        /// </typeparam>
        /// <typeparam name="TSourceBase">
        /// The base type that source inherits from.
        /// </typeparam>
        /// <typeparam name="TTargetBase">
        /// The base type that target inherits from.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IInjectionDefinitionBuilder{TSource,TTarget}"/> that allows further configuration of the 
        /// injection.
        /// </returns>
        public static IInjectionDefinitionBuilder<TSource, TTarget> 
            RegisterInherited<TSource, TTarget, TSourceBase, TTargetBase>(
            [NotNull] this InjectionKernelBuilder @this)
            where TSource : TSourceBase
            where TTarget : TTargetBase
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var definition = new InjectionDefinition(typeof(TSource), typeof(TTarget))
            {
                new InheritsFragment(typeof(TSource), typeof(TTarget), typeof(TSourceBase), typeof(TTargetBase))
            };

            @this.InstanceRegistry.Register(typeof(InjectionDefinition), definition);

            return new InjectionDefinitionBuilder<TSource, TTarget>(definition, @this.InstanceRegistry);
        }

        /// <summary>
        /// Registers an enumerable monad interface to a collection monad concrete type.
        /// </summary>
        /// <param name="this">
        /// The <see cref="InjectionKernelBuilder"/> to register with.
        /// </param>
        /// <typeparam name="TInterface">
        /// The <see cref="IEnumerable{T}"/> interface.
        /// </typeparam>
        /// <typeparam name="TConcrete">
        /// The <see cref="ICollection{T}"/> concrete type.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when either <typeparamref name="TInterface"/> or <typeparamref name="TConcrete"/> is not a monadic
        /// type.
        /// </exception>
        /// <example>
        /// Use the <see cref="Placeholder"/> type to register a generic type:
        ///     <code>
        /// factory.Register&lt;ISet&lt;Placeholder&gt;, HashSet&lt;Placeholder&gt;&gt;();
        ///     </code>
        /// </example>
        /// <remarks>
        /// It might be possible to constuct parameters that do not behave as expected, but you are very unlikely to 
        /// do this by accident.
        /// </remarks>
        public static void RegisterEnumerable<TInterface, TConcrete>([NotNull] this InjectionKernelBuilder @this)
            where TInterface : IEnumerable<Placeholder>
            where TConcrete : ICollection<Placeholder>, TInterface, new()
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            @this.InstanceRegistry.Register(
                typeof(EnumerableFactoryRegistration), 
                new EnumerableFactoryRegistration(typeof(TInterface), typeof(TConcrete)));
        }

        /// <summary>
        /// Builds a <see cref="IInjectionStore"/> that matches the specification in <paramref name="this"/>, built 
        /// with the default configuration options.
        /// </summary>
        /// <param name="this">
        /// The <see cref="InjectionKernelBuilder"/> from which to build.
        /// </param>
        /// <returns>
        /// A <see cref="IInjectionStore"/> that matches the specification in <paramref name="this"/>, built with the 
        /// default configuration options.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static IInjectionKernel Build([NotNull] this InjectionKernelBuilder @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return @this.Build(
                BuildConfigurator.Instance.StoreFactories.Select(item => item()),
                BuildConfigurator.Instance.InstanceFactories.Select(item => item()));
        }
    }
}