// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionTransformStoreTests.cs" company="Bijectiv">
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
//   Defines the CollectionTransformStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using System.Collections;
    using System.Linq;

    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CollectionTransformStore"/> class.
    /// </summary>
    [TestClass]
    public class CollectionTransformStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CollectionTransformStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_StoreIsEmpty()
        {
            // Arrange

            // Act
            var target = new CollectionTransformStore();

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_TransformParameterIsNull_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var target = new CollectionTransformStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsTransformToStore()
        {
            // Arrange
            var transform1 = Stub.Create<ITransform>();
            var transform2 = Stub.Create<ITransform>();
            
            // Act
            var target = new CollectionTransformStore { transform1, transform2 };

            // Assert
            new[] { transform1, transform2 }.AssertSequenceEqual(target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_DefaultParameters_GetsEnumerator()
        {
            // Arrange
            var transform1 = Stub.Create<ITransform>();
            var target = new CollectionTransformStore { transform1 };

            // Act
            foreach (var item in (IEnumerable)target)
            {
                // Assert
                Assert.AreEqual(transform1, item);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var target = new CollectionTransformStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve(null, TestClass2.T);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var target = new CollectionTransformStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve(TestClass1.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreContainsMatchingTransform_ReturnsTransform()
        {
            // Arrange
            var transformMock = new Mock<ITransform>();
            transformMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            transformMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionTransformStore { transformMock.Object };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(transformMock.Object, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_StoreDoesNotContainMatchingTransform_ReturnsNull()
        {
            // Arrange
            var transformMock = new Mock<ITransform>();
            transformMock.SetupGet(_ => _.Source).Returns(TestClass1.T);
            transformMock.SetupGet(_ => _.Target).Returns(TestClass2.T);

            var target = new CollectionTransformStore { transformMock.Object };

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Resolve(TestClass2.T, TestClass1.T);

            // Assert
            Assert.IsNull(result);
        }
    }
}