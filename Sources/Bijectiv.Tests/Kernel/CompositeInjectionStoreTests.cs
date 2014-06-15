// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the CompositeInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable UseObjectOrCollectionInitializer
namespace Bijectiv.Tests.Kernel
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class CompositeInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CompositeInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CollectionEmpty()
        {
            // Arrange

            // Act
            var target = new CompositeInjectionStore();

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CollectionConstructor_CorrectNumberOfItemsAdded()
        {
            // Arrange
            var store1 = Stub.Create<IInjectionStore>();
            var store2 = Stub.Create<IInjectionStore>();
            var store3 = Stub.Create<IInjectionStore>();

            // Act
            var target = new CompositeInjectionStore { store1, store2, store3 };

            // Assert
            Assert.AreEqual(3, target.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CollectionConstructor_ItemsAddedInOrder()
        {
            // Arrange
            var store1 = Stub.Create<IInjectionStore>();
            var store2 = Stub.Create<IInjectionStore>();
            var store3 = Stub.Create<IInjectionStore>();

            // Act
            var target = new CompositeInjectionStore { store1, store2, store3 };

            // Assert
            Assert.AreEqual(store1, target.ElementAt(0));
            Assert.AreEqual(store2, target.ElementAt(1));
            Assert.AreEqual(store3, target.ElementAt(2));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_StoreParameterIsNull_Throws()
        {
            // Arrange
            var target = new CompositeInjectionStore();

            // Act
            target.Add(null);
            
            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_CorrectNumberOfItemsAdded()
        {
            // Arrange
            var store1 = Stub.Create<IInjectionStore>();
            var store2 = Stub.Create<IInjectionStore>();
            var store3 = Stub.Create<IInjectionStore>();

            var target = new CompositeInjectionStore();

            // Act
            target.Add(store1);
            target.Add(store2);
            target.Add(store3);
            
            // Assert
            Assert.AreEqual(3, target.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_ItemsAddedInOrder()
        {
            // Arrange
            var store1 = Stub.Create<IInjectionStore>();
            var store2 = Stub.Create<IInjectionStore>();
            var store3 = Stub.Create<IInjectionStore>();

            var target = new CompositeInjectionStore();

            // Act
            target.Add(store1);
            target.Add(store2);
            target.Add(store3);
            
            // Assert
            Assert.AreEqual(store1, target.ElementAt(0));
            Assert.AreEqual(store2, target.ElementAt(1));
            Assert.AreEqual(store3, target.ElementAt(2));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_NonGenericVersion_EnumeratesCollection()
        {
            // Arrange
            var store1 = Stub.Create<IInjectionStore>();
            var store2 = Stub.Create<IInjectionStore>();
            var store3 = Stub.Create<IInjectionStore>();

            // Act
            var target = new CompositeInjectionStore { store1, store2, store3 };

            var index = 0;
            foreach (var store in (IEnumerable)target)
            {
                // Assert
                Assert.AreEqual(target.ElementAt(index++), store);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_NoStores_ReturnsNull()
        {
            // Arrange
            var target = new CompositeInjectionStore();

            // Act
            var result = target.Resolve<IInjection>(typeof(int), typeof(int));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SingleStoreContainingMatchingInjection_ReturnsInjection()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var storeMock = repository.Create<IInjectionStore>();
            var expected = Stub.Create<IInjection>();

            storeMock.Setup(_ => _.Resolve<IInjection>(typeof(int), typeof(int))).Returns(expected);

            var target = new CompositeInjectionStore { storeMock.Object };

            // Act
            var actual = target.Resolve<IInjection>(typeof(int), typeof(int));

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SingleStoreDoesNotContainingMatchingInjection_ReturnsNull()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var storeMock = repository.Create<IInjectionStore>();

            storeMock.Setup(_ => _.Resolve<IInjection>(It.IsAny<Type>(), It.IsAny<Type>())).Returns(default(IInjection));

            var target = new CompositeInjectionStore { storeMock.Object };

            // Act
            var result = target.Resolve<IInjection>(typeof(int), typeof(int));

            // Assert
            repository.VerifyAll();
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_MultipleStores_ResolvesFromEachInOrder()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var storeMock1 = repository.Create<IInjectionStore>();
            var storeMock2 = repository.Create<IInjectionStore>();
            var storeMock3 = repository.Create<IInjectionStore>();
            var expected = Stub.Create<IInjection>();

            var sequence = new MockSequence();
            storeMock1
                .InSequence(sequence)
                .Setup(_ => _.Resolve<IInjection>(It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns(default(IInjection));

            storeMock2
                .InSequence(sequence)
                .Setup(_ => _.Resolve<IInjection>(typeof(int), typeof(int)))
                .Returns(expected);

            var target = new CompositeInjectionStore { storeMock1.Object, storeMock2.Object, storeMock3.Object };

            // Act
            var actual = target.Resolve<IInjection>(typeof(int), typeof(int));

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expected, actual);
        }
    }
}