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

    using Bijectiv.Factory;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;

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
            var target = new BuildConfigurator();

            // Assert
            var tasks = target.TransformTasks.Select(item => item()).ToArray();
            Assert.AreEqual(9, tasks.Length);
            var index = 0;
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeFragmentsTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(InitializeVariablesTask));
            Assert.IsInstanceOfType(tasks[index++], typeof(TryGetTargetFromCacheTask));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(ActivateTargetExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(DefaultFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(CustomFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index], typeof(CreateTargetTask));
            Assert.IsInstanceOfType(((CreateTargetTask)tasks[index++]).ExpressionFactory, typeof(FallbackFactoryExpressionFactory));
            Assert.IsInstanceOfType(tasks[index++], typeof(CacheTargetTask));
            Assert.IsInstanceOfType(tasks[index], typeof(ReturnTargetAsObjectTask));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_TransformStoreFactoriesInitialized()
        {
            // Arrange

            // Act
            var target = new BuildConfigurator();

            // Assert
            var factories = target.TransformStoreFactories.Select(item => item()).ToArray();
            Assert.AreEqual(3, factories.Length);
            Assert.IsInstanceOfType(factories[0], typeof(InstanceTransformStoreFactory));
            Assert.IsInstanceOfType(((InstanceTransformStoreFactory)factories[0]).Instance, typeof(IdenticalPrimitiveTransformStore));
            Assert.IsInstanceOfType(factories[1], typeof(InstanceTransformStoreFactory));
            Assert.IsInstanceOfType(((InstanceTransformStoreFactory)factories[1]).Instance, typeof(ConvertibleTransformStore));
            Assert.IsInstanceOfType(factories[2], typeof(DelegateTransformStoreFactory));
        }
    }
}