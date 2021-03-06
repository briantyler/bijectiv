﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildConfiguratorTests.cs" company="Bijectiv">
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
//   Defines the BuildConfiguratorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests
{
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="BuildConfigurator"/> class.
    /// </summary>
    [TestClass]
    public class BuildConfiguratorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange
            
            // Act
            new BuildConfigurator().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_TransformTasksInitialized()
        {
            // Arrange

            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var tasks = testTarget.TransformTasks.Select(item => item()).ToArray();
            var index = 0;
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeFragmentsTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeTransformVariablesTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeMembersTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(NullSourceTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(TryGetTargetFromCacheTask));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(
                ((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(ActivateTargetExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(
                ((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(DefaultFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(
                ((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(CustomFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(
                ((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(FallbackFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index++], typeof(CacheTargetTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(AddToInjectionTrailTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeInjectionParametersTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(MemberInjectionTask));
            Assert.IsInstanceOfType(tasks[index], typeof(AutoInjectionTask));
            Assert.AreEqual(((AutoInjectionTask)tasks[index++]).Detail.IsMerge, false);
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTriggersTask));
            Assert.AreEqual(TriggeredBy.InjectionEnded, ((CreateTriggersTask)tasks[index++]).TriggeredBy);
            Assert.IsInstanceOfType(tasks[index++], typeof(ReturnTargetAsObjectTask));

            Assert.AreEqual(index, tasks.Length);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_MemberTransformTasksInitialized()
        {
            // Arrange
            
            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var tasks = testTarget.MemberTransformTasks.Select(item => item()).ToArray();
            var index = 0;
            Assert.IsInstanceOfType(tasks[index++], typeof(MemberConditionSubtask));
            //// TODO Validate factories
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<ValueSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<ExpressionSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<DelegateSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<ValueSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<ExpressionSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<DelegateSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateLabelSubtask));
            Assert.AreEqual(LegendaryLabels.End, ((CreateLabelSubtask)tasks[index++]).Category);
            Assert.AreEqual(index, tasks.Length);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_MergeTasksInitialized()
        {
            // Arrange

            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var tasks = testTarget.MergeTasks.Select(item => item()).ToArray();
            var index = 0;
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeFragmentsTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(FallbackToTransformOnNullTargetTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeMergeVariablesTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeInjectionParametersTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeMembersTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(AddToInjectionTrailTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(MemberInjectionTask));
            Assert.IsInstanceOfType(tasks[index], typeof(AutoInjectionTask));
            Assert.AreEqual(((AutoInjectionTask)tasks[index++]).Detail.IsMerge, true);
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTriggersTask));
            Assert.AreEqual(TriggeredBy.InjectionEnded, ((CreateTriggersTask)tasks[index++]).TriggeredBy);
            Assert.IsInstanceOfType(tasks[index++], typeof(ReturnMergeResultTask));

            Assert.AreEqual(index, tasks.Length);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_MemberMergeTasksInitialized()
        {
            // Arrange

            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var tasks = testTarget.MemberMergeTasks.Select(item => item()).ToArray();
            var index = 0;
            Assert.IsInstanceOfType(tasks[index++], typeof(MemberConditionSubtask));
            //// TODO Validate factories
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<ValueSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<ExpressionSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberInjectionSubtask<DelegateSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<ValueSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<ExpressionSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index++], typeof(SourceMemberAssignSubtask<DelegateSourceMemberShard>));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateLabelSubtask));
            Assert.AreEqual(LegendaryLabels.End, ((CreateLabelSubtask)tasks[index++]).Category);
            Assert.AreEqual(index, tasks.Length);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_StoreFactoriesInitialized()
        {
            // Arrange

            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var index = 0;
            var factories = testTarget.StoreFactories.Select(item => item()).ToArray();

            Assert.IsInstanceOfType(factories[index], typeof(InstanceInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((InstanceInjectionStoreFactory)factories[index++]).Instance, 
                typeof(IdenticalPrimitiveInjectionStore));
            Assert.IsInstanceOfType(factories[index], typeof(InstanceInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((InstanceInjectionStoreFactory)factories[index++]).Instance, 
                typeof(ConvertibleInjectionStore));
            Assert.IsInstanceOfType(factories[index], typeof(InstanceInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((InstanceInjectionStoreFactory)factories[index++]).Instance,
                typeof(EnumerableToArrayInjectionStore));
            Assert.IsInstanceOfType(factories[index], typeof(InstanceInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((InstanceInjectionStoreFactory)factories[index++]).Instance,
                typeof(EnumerableToEnumerableInjectionStore));
            Assert.IsInstanceOfType(factories[index], typeof(DelegateInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((DelegateInjectionStoreFactory)factories[index++]).InjectionFactory,
                typeof(TransformFactory));
            Assert.IsInstanceOfType(factories[index], typeof(DelegateInjectionStoreFactory));
            Assert.IsInstanceOfType(
                ((DelegateInjectionStoreFactory)factories[index++]).InjectionFactory,
                typeof(MergeFactory));
            
            Assert.AreEqual(index, factories.Length);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceFactoriesInitialized()
        {
            // Arrange

            // Act
            var testTarget = new BuildConfigurator();

            // Assert
            var index = 0;
            var factories = testTarget.InstanceFactories.Select(item => item()).ToArray();

            Assert.IsInstanceOfType(factories[index], typeof(RegisteringInstanceFactory<EnumerableRegistration>));
            Assert.AreEqual(
               typeof(IEnumerableFactory),
               ((RegisteringInstanceFactory<EnumerableRegistration>)factories[index]).InstanceType);
            Assert.IsInstanceOfType(
               ((RegisteringInstanceFactory<EnumerableRegistration>)factories[index++]).InstanceFactory(),
               typeof(EnumerableFactory));

            Assert.IsInstanceOfType(factories[index], typeof(RegisteringInstanceFactory<TargetFinderRegistration>));
            Assert.AreEqual(
               typeof(ITargetFinderStore),
               ((RegisteringInstanceFactory<TargetFinderRegistration>)factories[index]).InstanceType);
            Assert.IsInstanceOfType(
               ((RegisteringInstanceFactory<TargetFinderRegistration>)factories[index++]).InstanceFactory(),
               typeof(TargetFinderStore));

            Assert.AreEqual(index, factories.Length);
        }
    }
}