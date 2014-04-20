// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformStoreBuilderTests.cs" company="Bijectiv">
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
//   Defines the TransformStoreBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.Stores;
    using Bijectiv.Tests.TestTools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TransformStoreBuilder"/> class.
    /// </summary>
    [TestClass]
    public class TransformStoreBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformStoreBuilder(Stub.Create<ITransformDefinitionRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_RegistryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformStoreBuilder(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_RegistryParameter_IsAssignedToRegistryProperty()
        {
            // Arrange
            var registry = Stub.Create<ITransformDefinitionRegistry>();
            
            // Act
            var target = new TransformStoreBuilder(registry);

            // Assert
            Assert.AreEqual(registry, target.Registry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void RegisterCallback_CallbackParameterIsNull_Throws()
        {
            // Arrange
            var registry = Stub.Create<ITransformDefinitionRegistry>();
            var target = new TransformStoreBuilder(registry);

            // Act
            target.RegisterCallback(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterCallback_CallbackParameter_IsCalledOnRegistryProperty()
        {
            // Arrange
            var registry = Stub.Create<ITransformDefinitionRegistry>();
            var target = new TransformStoreBuilder(registry);
            ITransformDefinitionRegistry calledRegistry = null;

            // Act
            target.RegisterCallback(_ => calledRegistry = _);

            // Assert
            Assert.AreEqual(registry, calledRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Build_FactoriesParameterIsNull_Throws()
        {
            // Arrange
            var target = new TransformStoreBuilder(Stub.Create<ITransformDefinitionRegistry>());

            // Act
            target.Build(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ReturnsCompositeTransformStore()
        {
            // Arrange
            var target = new TransformStoreBuilder(Stub.Create<ITransformDefinitionRegistry>());

            // Act
            var result = target.Build(Enumerable.Empty<ITransformStoreFactory>());

            // Assert
            Assert.IsInstanceOfType(result, typeof(CompositeTransformStore));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ConstructsStoreUsingFactories()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var factory1 = repository.Create<ITransformStoreFactory>();
            var store1 = Stub.Create<ITransformStore>();
            factory1.Setup(_ => _.Create(It.IsAny<ITransformDefinitionRegistry>())).Returns(store1);

            var factory2 = repository.Create<ITransformStoreFactory>();
            var store2 = Stub.Create<ITransformStore>();
            factory2.Setup(_ => _.Create(It.IsAny<ITransformDefinitionRegistry>())).Returns(store2);

            var target = new TransformStoreBuilder(Stub.Create<ITransformDefinitionRegistry>());

            // Act
            var result = (CompositeTransformStore)target.Build(new[] { factory1.Object, factory2.Object });

            // Assert
            new[] { store1, store2 }.AssertSequenceEqual(result);
        }
    }
}