// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the CollectionInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CollectionInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class CollectionInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CollectionInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_StoreIsEmpty()
        {
            // Arrange

            // Act
            var target = new CollectionInjectionStore();

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_InjectionParameterIsNull_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var target = new CollectionInjectionStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsInjectionToStore()
        {
            // Arrange
            var injection1 = Stub.Create<IInjection>();
            var injection2 = Stub.Create<IInjection>();
            
            // Act
            var target = new CollectionInjectionStore { injection1, injection2 };

            // Assert
            new[] { injection1, injection2 }.AssertSequenceEqual(target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_DefaultParameters_GetsEnumerator()
        {
            // Arrange
            var injection1 = Stub.Create<IInjection>();
            var target = new CollectionInjectionStore { injection1 };

            // Act
            foreach (var item in (IEnumerable)target)
            {
                // Assert
                Assert.AreEqual(injection1, item);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var target = new CollectionInjectionStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve<IInjection>(null, TestClass2.T);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var target = new CollectionInjectionStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve<IInjection>(TestClass1.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreContainsMatchingInjection_ReturnsInjection()
        {
            // Arrange
            var injectionMock = new Mock<IInjection>();
            injectionMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            injectionMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionInjectionStore { injectionMock.Object };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve<IInjection>(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(injectionMock.Object, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreContainsMatchingTransform_ReturnsTransfrom()
        {
            // Arrange
            var injectionMock = new Mock<IInjection>();
            injectionMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            injectionMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var transformMock = new Mock<ITransform>();
            transformMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            transformMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionInjectionStore
            {
                injectionMock.Object,
                transformMock.Object
            };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve<ITransform>(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(transformMock.Object, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreDoesNotContainMatchingInjection_ReturnsNull()
        {
            // Arrange
            var injectionMock = new Mock<IInjection>();
            injectionMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            injectionMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionInjectionStore { injectionMock.Object };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve<IInjection>(TestClass2.T, TestClass1.T);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreDoesNotContainsMatchingTransform_ReturnsTransfrom()
        {
            // Arrange
            var injectionMock = new Mock<IInjection>();
            injectionMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            injectionMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var transformMock = new Mock<IMerge>();
            transformMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            transformMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionInjectionStore
            {
                injectionMock.Object,
                transformMock.Object
            };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve<ITransform>(TestClass1.T, TestClass2.T);

            // Assert
            Assert.IsNull(result);
        }
    }
}