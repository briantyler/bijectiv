// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateInjectionStoreFactoryTests.cs" company="Bijectiv">
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
//   Defines the DelegateInjectionStoreFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using Bijectiv.InjectionFactory;
    using Bijectiv.KernelBuilder;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="DelegateInjectionStoreFactory"/> class.
    /// </summary>
    [TestClass]
    public class DelegateInjectionStoreFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InjectionFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateInjectionStoreFactory(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateInjectionStoreFactory(Stub.Create<IInjectionFactory<IInjection>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectionFactoryParameter_IsAssignedToInjectionFactoryProperty()
        {
            // Arrange
            var injectionFactory = Stub.Create<IInjectionFactory<IInjection>>();

            // Act
            var target = new DelegateInjectionStoreFactory(injectionFactory);

            // Assert
            Assert.AreEqual(injectionFactory, target.InjectionFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_RegistryParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateInjectionStoreFactory(Stub.Create<IInjectionFactory<IInjection>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsCollectionInjectionStore()
        {
            // Arrange
            var target = new DelegateInjectionStoreFactory(Stub.Create<IInjectionFactory<IInjection>>());
            var registry = new InstanceRegistry();

            // Act
            var result = target.Create(registry);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CollectionInjectionStore));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_BuildsStoreFromRegistryUsingFactory()
        {
            // Arrange
            var definition1 = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var definition2 = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            registryMock.Setup(_ => _.ResolveAll<InjectionDefinition>()).Returns(new[] { definition1, definition2 });

            var injection1 = Stub.Create<IInjection>();
            var injection2 = Stub.Create<IInjection>();
            var factoryMock = new Mock<IInjectionFactory<IInjection>>(MockBehavior.Strict);

            // Note: `factoryMock.Setup(_ => _.Create(registry, definition1))` does not work, which is a moq problem.
            factoryMock
                .Setup(_ => _.Create(registryMock.Object, It.Is<InjectionDefinition>(d => d == definition1)))
                .Returns(injection1);

            factoryMock
                .Setup(_ => _.Create(registryMock.Object, It.Is<InjectionDefinition>(d => d == definition2)))
                .Returns(injection2);

            var target = new DelegateInjectionStoreFactory(factoryMock.Object);

            // Act
            var result = (CollectionInjectionStore)target.Create(registryMock.Object);

            // Assert
            factoryMock.VerifyAll();
            new[] { injection1, injection2 }.AssertSequenceEqual(result);
        }
    }
}