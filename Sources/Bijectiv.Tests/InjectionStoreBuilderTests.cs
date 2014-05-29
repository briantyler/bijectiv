// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionStoreBuilderTests.cs" company="Bijectiv">
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
//   Defines the InjectionStoreBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests
{
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InjectionStoreBuilder"/> class.
    /// </summary>
    [TestClass]
    public class InjectionStoreBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionStoreBuilder(Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InstanceRegistryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionStoreBuilder(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InstanceRegistryParameter_IsAssignedToRegistryProperty()
        {
            // Arrange
            var registry = Stub.Create<IInstanceRegistry>();
            
            // Act
            var target = new InjectionStoreBuilder(registry);

            // Assert
            Assert.AreEqual(registry, target.InstanceRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Build_FactoriesParameterIsNull_Throws()
        {
            // Arrange
            var target = new InjectionStoreBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            target.Build(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_ReturnsCompositeInjectionStore()
        {
            // Arrange
            var target = new InjectionStoreBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            var result = target.Build(Enumerable.Empty<IInjectionStoreFactory>());

            // Assert
            Assert.IsInstanceOfType(result, typeof(CompositeInjectionStore));
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

            var target = new InjectionStoreBuilder(Stub.Create<IInstanceRegistry>());

            // Act
            var result = (CompositeInjectionStore)target.Build(new[] { factory1.Object, factory2.Object });

            // Assert
            new[] { store1, store2 }.AssertSequenceEqual(result);
        }
    }
}