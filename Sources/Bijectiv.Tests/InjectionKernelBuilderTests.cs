// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernelBuilderTests.cs" company="Bijectiv">
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
//   Defines the InjectionKernelBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests
{
    using System;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InjectionKernelBuilder"/> class.
    /// </summary>
    [TestClass]
    public class InjectionKernelBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InstanceRegistryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionKernelBuilder(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InstanceRegistryParameter_IsAssignedToRegistryProperty()
        {
            // Arrange
            var registry = Stub.Create<IInstanceRegistry>();
            
            // Act
            var target = new InjectionKernelBuilder(registry);

            // Assert
            Assert.AreEqual(registry, target.InstanceRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Build_StoreFactoriesParameterIsNull_Throws()
        {
            // Arrange
            var target = new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            target.Build(null, Enumerable.Empty<IInstanceFactory>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Build_InstanceFactoriesParameterIsNull_Throws()
        {
            // Arrange
            var target = new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            target.Build(Enumerable.Empty<IInjectionStoreFactory>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ReturnedInjectionKernelStoreIsCompositeInjectionStore()
        {
            // Arrange
            var target = new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            var result = target.Build(Enumerable.Empty<IInjectionStoreFactory>(), Enumerable.Empty<IInstanceFactory>());

            // Assert
            Assert.IsInstanceOfType(result.Store, typeof(CompositeInjectionStore));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ConstructsStoreUsingFactories()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var factory1 = repository.Create<IInjectionStoreFactory>();
            var store1 = Stub.Create<IInjectionStore>();
            factory1.Setup(_ => _.Create(It.IsAny<IInstanceRegistry>())).Returns(store1);

            var factory2 = repository.Create<IInjectionStoreFactory>();
            var store2 = Stub.Create<IInjectionStore>();
            factory2.Setup(_ => _.Create(It.IsAny<IInstanceRegistry>())).Returns(store2);

            var target = new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            var result = target.Build(
                new[] { factory1.Object, factory2.Object }, 
                Enumerable.Empty<IInstanceFactory>());

            // Assert
            new[] { store1, store2 }.AssertSequenceEqual((CompositeInjectionStore)result.Store);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ConstructsRegistryUsingFactories()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var factory1 = repository.Create<IInstanceFactory>();
            var instance1 = Stub.Create<ITargetFinderStore>();
            factory1
                .Setup(_ => _.Create(It.IsAny<IInstanceRegistry>()))
                .Returns(new Tuple<Type, object>(typeof(ITargetFinderStore), instance1));

            var factory2 = repository.Create<IInstanceFactory>();
            var instance2 = Stub.Create<IEnumerableFactory>();
            factory2
                .Setup(_ => _.Create(It.IsAny<IInstanceRegistry>()))
                .Returns(new Tuple<Type, object>(typeof(IEnumerableFactory), instance2));

            var target = new InjectionKernelBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            var result = target.Build(
                Enumerable.Empty<IInjectionStoreFactory>(),
                new[] { factory1.Object, factory2.Object });

            // Assert
            Assert.AreEqual(instance1, result.Registry.Resolve<ITargetFinderStore>());
            Assert.AreEqual(instance2, result.Registry.Resolve<IEnumerableFactory>());
        }
    }
}