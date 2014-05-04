// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildConfigurator.cs" company="Bijectiv">
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
//   Defines the BuildConfigurator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bijectiv.Factory;
    using Bijectiv.Stores;
    using Bijectiv.Utilities;

    /// <summary>
    /// Provides default <see cref="ITransformStore"/> build configuration options.
    /// </summary>
    public sealed class BuildConfigurator
    {
        /// <summary>
        /// The singleton instance implementation.
        /// </summary>
        private static readonly BuildConfigurator InstanceImpl = new BuildConfigurator();

        /// <summary>
        /// Initialises a new instance of the <see cref="BuildConfigurator"/> class.
        /// </summary>
        internal BuildConfigurator()
        {
            this.TransformStoreFactories = new List<Func<ITransformStoreFactory>>();
            this.TransformTasks = new List<Func<ITransformTask>>();

            this.Reset();
        }

        /// <summary>
        /// Gets the configurator instance.
        /// </summary>
        public static BuildConfigurator Instance
        {
            get { return InstanceImpl; }
        }

        /// <summary>
        /// Gets the default sequence of <see cref="ITransformStoreFactory"/> instances.
        /// </summary>
        public IList<Func<ITransformStoreFactory>> TransformStoreFactories { get; private set; }

        /// <summary>
        /// Gets the default sequence of <see cref="ITransformTask"/> instances.
        /// </summary>
        public IList<Func<ITransformTask>> TransformTasks { get; private set; }

        /// <summary>
        /// Resets the configurator to the default configuration.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "This method is not hard to maintain.")]
        public void Reset()
        {
            this.TransformStoreFactories.Clear();
            this.TransformStoreFactories.AddRange(
                new Func<ITransformStoreFactory>[] 
                {
                    () => new InstanceTransformStoreFactory(new IdenticalPrimitiveTransformStore()),
                    () => new InstanceTransformStoreFactory(new ConvertibleTransformStore()),
                    () => new DelegateTransformStoreFactory(
                        new TransformFactory(this.TransformTasks.Select(item => item()).ToArray()))
                });

            this.TransformTasks.Clear();
            this.TransformTasks.AddRange(
                new Func<ITransformTask>[] 
                {
                    () => new InitializeFragmentsTask(),
                    () => new InitializeVariablesTask(),
                    () => new NullSourceTask(),
                    () => new TryGetTargetFromCacheTask(),
                    () => new CreateTargetTask(new ActivateTargetExpressionFactory()),
                    () => new CreateTargetTask(new DefaultFactoryExpressionFactory()),
                    () => new CreateTargetTask(new CustomFactoryExpressionFactory()),
                    () => new CreateTargetTask(new FallbackFactoryExpressionFactory()),
                    () => new CacheTargetTask(), 
                    () => new ReturnTargetAsObjectTask()
                });
        }
    }
}