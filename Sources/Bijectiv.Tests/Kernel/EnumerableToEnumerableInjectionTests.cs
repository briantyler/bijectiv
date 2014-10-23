// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToEnumerableInjectionTests.cs" company="Bijectiv">
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
//   Defines the EnumerableToEnumerableInjectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="EnumerableToEnumerableInjection"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableToEnumerableInjectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableToEnumerableInjection(
                typeof(IEnumerable), 
                typeof(IEnumerable<TestClass1>), 
                Stub.Create<ICollectionMerger>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableToEnumerableInjection(
                null,
                typeof(IEnumerable<TestClass1>),
                Stub.Create<ICollectionMerger>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                null,
                Stub.Create<ICollectionMerger>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_MergerParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                null)
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourceParameterIsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var testTarget = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable), testTarget.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var testTarget = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable<TestClass1>), testTarget.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_MergerParameterIsAssignedToMergerProperty()
        {
            // Arrange
            var merger = Stub.Create<ICollectionMerger>();

            // Act
            var testTarget = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                merger);

            // Assert
            Assert.AreEqual(merger, testTarget.Merger);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget(Stub.Create<ICollectionMerger>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Transform(Stub.Create<IEnumerable>(), null, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidParameters_DelegatesToMerge()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = Stub.Create<IEnumerable>();
            var target = new HashSet<TestClass1>();
            var contextMock = repository.Create<IInjectionContext>();
            var expectedResult = new HashSet<TestClass1>();

            var factoryMock = repository.Create<IEnumerableFactory>();
            factoryMock.Setup(_ => _.Resolve(It.IsAny<Type>())).Returns(target);

            var registryMock = repository.Create<IInstanceRegistry>();
            registryMock.Setup(_ => _.Resolve<IEnumerableFactory>()).Returns(factoryMock.Object);

            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);

            var testTargetMock = repository.Create<EnumerableToEnumerableInjection>(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<ICollectionMerger>());
            testTargetMock.CallBase = false;

            testTargetMock
                .Setup(_ => _.Merge(source, target, contextMock.Object, null))
                .Returns(new MergeResult(PostMergeAction.None, expectedResult));

            // Act
            var result = testTargetMock.Object.Transform(source, contextMock.Object, null);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Merge_ContextParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget(Stub.Create<ICollectionMerger>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Merge(Stub.Create<IEnumerable>(), new List<TestClass1>(), null, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsNonGenericEnumerableTargetIsCollection_UsesCorrectMergeOverload()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = Stub.Create<IEnumerable>();
            var target = new HashSet<TestClass1>();
            var context = Stub.Create<IInjectionContext>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock.Setup(_ => _.Merge(source, target, context));

            var testTarget = CreateTestTarget(mergerMock.Object);

            // Act
            var result = testTarget.Merge(source, target, context, null);
            
            // Assert
            repository.VerifyAll();
            Assert.AreEqual(target, result.Target);
            Assert.AreEqual(PostMergeAction.None, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsGenericEnumerableTargetIsCollection_UsesCorrectMergeOverload()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = Stub.Create<IEnumerable<object>>();
            var target = new HashSet<TestClass1>();
            var context = Stub.Create<IInjectionContext>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock.Setup(_ => _.Merge(source, target, context));

            var testTarget = CreateTestTarget(mergerMock.Object);

            // Act
            var result = testTarget.Merge(source, target, context, null);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(target, result.Target);
            Assert.AreEqual(PostMergeAction.None, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsGenericEnumerableTargetIsList_UsesCorrectMergeOverload()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = Stub.Create<IEnumerable<object>>();
            var target = new List<TestClass1>();
            var context = Stub.Create<IInjectionContext>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock.Setup(_ => _.Merge(source, target, context));

            var testTarget = CreateTestTarget(mergerMock.Object);

            // Act
            var result = testTarget.Merge(source, target, context, null);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(target, result.Target);
            Assert.AreEqual(PostMergeAction.None, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_TargetIsNull_ResolvesTargetUsingFactory()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = Stub.Create<IEnumerable<object>>();
            var target = new List<TestClass1>();
            var contextMock = repository.Create<IInjectionContext>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock.Setup(_ => _.Merge(source, target, contextMock.Object));

            var factoryMock = repository.Create<IEnumerableFactory>();
            factoryMock.Setup(_ => _.Resolve(It.IsAny<Type>())).Returns(target);

            var registryMock = repository.Create<IInstanceRegistry>();
            registryMock.Setup(_ => _.Resolve<IEnumerableFactory>()).Returns(factoryMock.Object);
            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);

            var testTarget = CreateTestTarget(mergerMock.Object);

            // Act
            var result = testTarget.Merge(source, null, contextMock.Object, null);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(target, result.Target);
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        private static EnumerableToEnumerableInjection CreateTestTarget(ICollectionMerger merger)
        {
            return new EnumerableToEnumerableInjection(
                typeof(IEnumerable), typeof(IEnumerable<TestClass1>), merger);
        }
    }
}