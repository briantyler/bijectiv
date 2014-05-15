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
    using System.Linq;

    using Bijectiv.Factory;
    using Bijectiv.Stores;
    using Bijectiv.Utilities;

    /// <summary>
    /// Provides default <see cref="IInjectionStore"/> build configuration options.
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
            this.StoreFactories = new List<Func<IInjectionStoreFactory>>();
            this.Tasks = new List<Func<IInjectionTask>>();

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
        /// Gets the default sequence of <see cref="IInjectionStoreFactory"/> instances.
        /// </summary>
        public IList<Func<IInjectionStoreFactory>> StoreFactories { get; private set; }

        /// <summary>
        /// Gets the default sequence of <see cref="IInjectionTask"/> instances.
        /// </summary>
        public IList<Func<IInjectionTask>> Tasks { get; private set; }

        /// <summary>
        /// Resets the configurator to the default configuration.
        /// </summary>
        public void Reset()
        {
            this.ResetStoreFactories();
            this.ResetTasks();
        }

        /// <summary>
        /// Resets <see cref="StoreFactories"/> to its default configuration.
        /// </summary>
        private void ResetStoreFactories()
        {
            this.StoreFactories.Clear();
            this.StoreFactories.AddRange(
                new Func<IInjectionStoreFactory>[]
                {
                    () => new InstanceInjectionStoreFactory(new IdenticalPrimitiveInjectionStore()),
                    () => new InstanceInjectionStoreFactory(new ConvertibleInjectionStore()),
                    () => new DelegateInjectionStoreFactory(new TransformFactory(this.Tasks.Select(item => item()).ToArray()))
                });
        }

        /// <summary>
        /// Resets <see cref="Tasks"/> to its default configuration.
        /// </summary>
        private void ResetTasks()
        {
            this.Tasks.Clear();
            this.Tasks.AddRange(
                new Func<IInjectionTask>[]
                {
                    () => new InitializeFragmentsTask(), 
                    () => new InitializeVariablesTask(), 
                    () => new InitializeMembersTask(new ReflectionGateway()), 
                    () => new NullSourceTask(),
                    () => new TryGetTargetFromCacheTask(),
                    () => new CreateTargetTask(new ActivateTargetExpressionFactory()),
                    () => new CreateTargetTask(new DefaultFactoryExpressionFactory()),
                    () => new CreateTargetTask(new CustomFactoryExpressionFactory()),
                    () => new CreateTargetTask(new FallbackFactoryExpressionFactory()), 
                    () => new CacheTargetTask(),
                    () => new AutoInjectionTask(new AutoInjectionTaskTransformDetail()), 
                    () => new ReturnTargetAsObjectTask()
                });
        }
    }
}