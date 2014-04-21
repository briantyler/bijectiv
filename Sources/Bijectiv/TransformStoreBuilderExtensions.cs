// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformStoreBuilderExtensions.cs" company="Bijectiv">
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
//   Defines the TransformStoreBuilderExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;

    using JetBrains.Annotations;

    /// <summary>
    /// Extensions to the <see cref="TransformStoreBuilder"/> class.
    /// </summary>
    public static class TransformStoreBuilderExtensions
    {
        /// <summary>
        /// Registers a new transform with the store builder.
        /// </summary>
        /// <param name="this">
        /// The <see cref="TransformStoreBuilder"/> to register with.
        /// </param>
        /// <typeparam name="TSource">
        /// The source type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The target type.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ITransformDefinitionBuilder{TSource,TTarget}"/> that allows further configuration of the 
        /// transform.
        /// </returns>
        public static ITransformDefinitionBuilder<TSource, TTarget> Register<TSource, TTarget>(
            [NotNull] this TransformStoreBuilder @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var defintion = new TransformDefinition(typeof(TSource), typeof(TTarget));
            @this.RegisterCallback(registry => registry.Add(defintion));

            return new TransformDefinitionBuilder<TSource, TTarget>(defintion);
        }

        /// <summary>
        /// Registers a new transform with the store builder and inherits all of the appropriate fragments from the 
        /// base transform if it exists.
        /// </summary>
        /// <param name="this">
        /// The <see cref="TransformStoreBuilder"/> to register with.
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
        /// A <see cref="ITransformDefinitionBuilder{TSource,TTarget}"/> that allows further configuration of the 
        /// transform.
        /// </returns>
        public static ITransformDefinitionBuilder<TSource, TTarget> RegisterInherited<TSource, TTarget, TSourceBase, TTargetBase>(
            [NotNull] this TransformStoreBuilder @this)
            where TSource : TSourceBase
            where TTarget : TTargetBase
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var defintion = new TransformDefinition(typeof(TSource), typeof(TTarget))
            {
                new InheritsFragment(typeof(TSource), typeof(TTarget), typeof(TSourceBase), typeof(TTargetBase))
            };

            @this.RegisterCallback(registry => registry.Add(defintion));

            return new TransformDefinitionBuilder<TSource, TTarget>(defintion);
        }

        /// <summary>
        /// Builds a <see cref="ITransformStore"/> that matches the specification in <paramref name="this"/>, built 
        /// with the default configuration options.
        /// </summary>
        /// <param name="this">
        /// The <see cref="TransformStoreBuilder"/> from which to build.
        /// </param>
        /// <returns>
        /// A <see cref="ITransformStore"/> that matches the specification in <paramref name="this"/>, built with the 
        /// default configuration options.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static ITransformStore Build([NotNull] this TransformStoreBuilder @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return @this.Build(BuildConfigurator.Instance.TransformStoreFactories.Select(item => item()));
        }
    }
}