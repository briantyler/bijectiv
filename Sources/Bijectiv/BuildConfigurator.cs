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

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
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
            this.InstanceFactories = new List<Func<IInstanceFactory>>();
            this.TransformTasks = new List<Func<IInjectionTask>>();
            this.MergeTasks = new List<Func<IInjectionTask>>();

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
        /// Gets the default sequence of <see cref="IInstanceFactory"/> instances.
        /// </summary>
        public IList<Func<IInstanceFactory>> InstanceFactories { get; private set; }

        /// <summary>
        /// Gets the default sequence of <see cref="IInjectionTask"/> instances used for constructing 
        /// <see cref="ITransform"/> instances.
        /// </summary>
        public IList<Func<IInjectionTask>> TransformTasks { get; private set; }

        /// <summary>
        /// Gets the default sequence of <see cref="IInjectionTask"/> instances used for constructing 
        /// <see cref="IMerge"/> instances.
        /// </summary>
        public IList<Func<IInjectionTask>> MergeTasks { get; private set; }

        /// <summary>
        /// Resets the configurator to the default configuration.
        /// </summary>
        public void Reset()
        {
            this.ResetStoreFactories();
            this.ResetInstanceFactories();
            this.ResetTransformTasks();
            this.ResetMergeTasks();
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
                    () => new InstanceInjectionStoreFactory(new EnumerableToArrayInjectionStore()),
                    () => new InstanceInjectionStoreFactory(new EnumerableToEnumerableInjectionStore()),
                    () => new DelegateInjectionStoreFactory(
                        new TransformFactory(this.TransformTasks.Select(item => item()).ToArray())),
                    () => new DelegateInjectionStoreFactory(
                        new MergeFactory(this.MergeTasks.Select(item => item()).ToArray()))
                });
        }

        /// <summary>
        /// Resets <see cref="InstanceFactories"/> to its default configuration.
        /// </summary>
        private void ResetInstanceFactories()
        {
            this.InstanceFactories.Clear();
            this.InstanceFactories.AddRange(
                new Func<IInstanceFactory>[]
                {
                    () => new RegisteringInstanceFactory<EnumerableRegistration>(
                        typeof(IEnumerableFactory), () => new EnumerableFactory()),
                    () => new RegisteringInstanceFactory<TargetFinderRegistration>(
                        typeof(ITargetFinderStore), () => new TargetFinderStore())
                });
        }

        /// <summary>
        /// Resets <see cref="TransformTasks"/> to its default configuration.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity",
            Justification = "The annonymous methods make this theoretically bad, but in practice it's fine.")]
        private void ResetTransformTasks()
        {
            this.TransformTasks.Clear();
            this.TransformTasks.AddRange(
                new Func<IInjectionTask>[]
                {
                    () => new InitializeFragmentsTask(), 
                    () => new InitializeTransformVariablesTask(), 
                    () => new InitializeMembersTask(new ReflectionGateway()), 
                    () => new NullSourceTask(),
                    () => new TryGetTargetFromCacheTask(),
                    () => new CreateTargetTask(new ActivateTargetExpressionFactory()),
                    () => new CreateTargetTask(new DefaultFactoryExpressionFactory()),
                    () => new CreateTargetTask(new CustomFactoryExpressionFactory()),
                    () => new CreateTargetTask(new FallbackFactoryExpressionFactory()), 
                    () => new CacheTargetTask(),
                    () => new InitializeTriggerParametersTask(),
                    () => new AutoInjectionTask(new AutoInjectionTaskTransformDetail()),
                    () => new CreateTriggersTask(TriggeredBy.InjectionEnded), 
                    () => new ReturnTargetAsObjectTask()
                });
        }

        /// <summary>
        /// Resets <see cref="TransformTasks"/> to its default configuration.
        /// </summary>
        private void ResetMergeTasks()
        {
            this.MergeTasks.Clear();
            this.MergeTasks.AddRange(
                new Func<IInjectionTask>[]
                {
                    () => new InitializeFragmentsTask(), 
                    () => new InitializeMergeVariablesTask(),
                    () => new InitializeTriggerParametersTask(),
                    () => new InitializeMembersTask(new ReflectionGateway()), 
                    () => new AutoInjectionTask(new AutoInjectionTaskMergeDetail()),
                    () => new CreateTriggersTask(TriggeredBy.InjectionEnded), 
                    () => new ReturnMergeResultTask()
                });
        }
    }
}