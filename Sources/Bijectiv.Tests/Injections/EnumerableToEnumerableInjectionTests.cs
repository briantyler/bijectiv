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

namespace Bijectiv.Tests.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Injections;
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
                Stub.Create<IEnumerableFactory>(),
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
                Stub.Create<IEnumerableFactory>(),
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
                Stub.Create<IEnumerableFactory>(),
                Stub.Create<ICollectionMerger>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
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
                Stub.Create<IEnumerableFactory>(),
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
            var target = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<IEnumerableFactory>(),
                Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<IEnumerableFactory>(),
                Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable<TestClass1>), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_FactoryParameterIsAssignedToFactoryProperty()
        {
            // Arrange
            var factory = Stub.Create<IEnumerableFactory>();

            // Act
            var target = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                factory,
                Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(factory, target.Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_MergerParameterIsAssignedToMergerProperty()
        {
            // Arrange
            var merger = Stub.Create<ICollectionMerger>();

            // Act
            var target = new EnumerableToEnumerableInjection(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                Stub.Create<IEnumerableFactory>(),
                merger);

            // Assert
            Assert.AreEqual(merger, target.Merger);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget(Stub.Create<IEnumerableFactory>(), Stub.Create<ICollectionMerger>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Transform(Stub.Create<IEnumerable>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidParameters_DelegatesToMerge()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var sourceInstance = Stub.Create<IEnumerable>();
            var targetInstance = new HashSet<TestClass1>();
            var context = Stub.Create<IInjectionContext>();
            var expectedResult = new HashSet<TestClass1>();

            var factoryMock = repository.Create<IEnumerableFactory>();
            factoryMock.Setup(_ => _.Resolve(It.IsAny<Type>())).Returns(targetInstance);

            var target = repository.Create<EnumerableToEnumerableInjection>(
                typeof(IEnumerable),
                typeof(IEnumerable<TestClass1>),
                factoryMock.Object,
                Stub.Create<ICollectionMerger>());
            target.CallBase = false;

            target
                .Setup(_ => _.Merge(sourceInstance, targetInstance, context))
                .Returns(new MergeResult(PostMergeAction.None, expectedResult));

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Object.Transform(sourceInstance, context);

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
            var target = CreateTarget(Stub.Create<IEnumerableFactory>(), Stub.Create<ICollectionMerger>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Merge(Stub.Create<IEnumerable>(), new List<TestClass1>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsNonGenericEnumerableTargetIsCollection_Expectation()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var sourceInstance = Stub.Create<IEnumerable>();
            var targetInstance = new HashSet<TestClass1>();
            var context = Stub.Create<IInjectionContext>();
            var expectedResult = Stub.Create<IMergeResult>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock
                .Setup(_ => _.Merge(sourceInstance, targetInstance, context))
                .Returns(expectedResult);

            var target = CreateTarget(Stub.Create<IEnumerableFactory>(), mergerMock.Object);

            // Act
            var result = target.Merge(sourceInstance, targetInstance, context);
            
            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsGenericEnumerableTargetIsCollection_Expectation()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var sourceInstance = Stub.Create<IEnumerable<object>>();
            var targetInstance = new HashSet<TestClass1>();
            var context = Stub.Create<IInjectionContext>();
            var expectedResult = Stub.Create<IMergeResult>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock
                .Setup(_ => _.Merge(sourceInstance, targetInstance, context))
                .Returns(expectedResult);

            var target = CreateTarget(Stub.Create<IEnumerableFactory>(), mergerMock.Object);

            // Act
            var result = target.Merge(sourceInstance, targetInstance, context);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_SourceIsGenericEnumerableTargetIsList_Expectation()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var sourceInstance = Stub.Create<IEnumerable<object>>();
            var targetInstance = new List<TestClass1>();
            var context = Stub.Create<IInjectionContext>();
            var expectedResult = Stub.Create<IMergeResult>();

            var mergerMock = repository.Create<ICollectionMerger>();
            mergerMock
                .Setup(_ => _.Merge(sourceInstance, targetInstance, context))
                .Returns(expectedResult);

            var target = CreateTarget(Stub.Create<IEnumerableFactory>(), mergerMock.Object);

            // Act
            var result = target.Merge(sourceInstance, targetInstance, context);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expectedResult, result);
        }

        private static EnumerableToEnumerableInjection CreateTarget(
            IEnumerableFactory factory,
            ICollectionMerger merger)
        {
            return new EnumerableToEnumerableInjection(
                typeof(IEnumerable), typeof(IEnumerable<TestClass1>), factory, merger);
        }
    }
}