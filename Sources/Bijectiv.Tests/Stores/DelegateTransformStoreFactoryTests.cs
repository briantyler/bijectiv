// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateTransformStoreFactoryTests.cs" company="Bijectiv">
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
//   Defines the DelegateTransformStoreFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="DelegateTransformStoreFactory"/> class.
    /// </summary>
    [TestClass]
    public class DelegateTransformStoreFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TransformFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateTransformStoreFactory(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateTransformStoreFactory(Stub.Create<ITransformFactory>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TransformFactoryParameter_IsAssignedToTransformFactoryProperty()
        {
            // Arrange
            var transformFactory = Stub.Create<ITransformFactory>();

            // Act
            var target = new DelegateTransformStoreFactory(transformFactory);

            // Assert
            Assert.AreEqual(transformFactory, target.TransformFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_RegistryParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateTransformStoreFactory(Stub.Create<ITransformFactory>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsCollectionTransformStore()
        {
            // Arrange
            var target = new DelegateTransformStoreFactory(Stub.Create<ITransformFactory>());
            var registry = new TransformDefinitionRegistry();

            // Act
            var result = target.Create(registry);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CollectionTransformStore));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_BuildsStoreFromRegistryUsingFactory()
        {
            // Arrange
            var definition1 = new TransformDefinition(TestClass1.T, TestClass2.T);
            var definition2 = new TransformDefinition(TestClass1.T, TestClass2.T);
            var registry = new TransformDefinitionRegistry { definition1, definition2 };

            var transform1 = Stub.Create<ITransform>();
            var transform2 = Stub.Create<ITransform>();
            var factoryMock = new Mock<ITransformFactory>(MockBehavior.Strict);

            // Note: `factoryMock.Setup(_ => _.Create(registry, definition1))` does not work, which is a moq problem.
            factoryMock
                .Setup(_ => _.Create(registry, It.Is<TransformDefinition>(d => d == definition1)))
                .Returns(transform1);

            factoryMock
                .Setup(_ => _.Create(registry, It.Is<TransformDefinition>(d => d == definition2)))
                .Returns(transform2);

            var target = new DelegateTransformStoreFactory(factoryMock.Object);

            // Act
            var result = (CollectionTransformStore)target.Create(registry);

            // Assert
            factoryMock.VerifyAll();
            new[] { transform1, transform2 }.AssertSequenceEqual(result);
        }
    }
}