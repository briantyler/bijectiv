// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToArrayInjectionTests.cs" company="Bijectiv">
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
//   Defines the EnumerableToArrayInjectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="EnumerableToArrayInjection"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableToArrayInjectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableToArrayInjection(null, typeof(TestClass1[]), Stub.Create<ICollectionMerger>()).Naught();

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
            new EnumerableToArrayInjection(typeof(IEnumerable), null, Stub.Create<ICollectionMerger>()).Naught();

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
            new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[]), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceParameterIsNotEnumerable_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(
                typeof(int), typeof(TestClass1[]), Stub.Create<ICollectionMerger>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsNotArray_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(Collection<TestClass1>), Stub.Create<ICollectionMerger>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsMultiDimensionalArray_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(TestClass1[,]), Stub.Create<ICollectionMerger>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(TestClass1[]), Stub.Create<ICollectionMerger>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourcePropertyIsEnumerable()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(
                typeof(Stack<int>), typeof(TestClass1[]), Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(TestClass1[]), Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(TestClass1[]), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MergerParameter_IsAssignedToMergerProperty()
        {
            // Arrange
            var merger = Stub.Create<ICollectionMerger>();

            // Act
            var target = new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[]), merger);

            // Assert
            Assert.AreEqual(merger, target.Merger);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterElementType_IsAssignedToTargetElementProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(TestClass1[]), Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(TestClass1), target.TargetElement);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterElementType_IsAmplifiedToEnumerableTargetProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(TestClass1[]), Stub.Create<ICollectionMerger>());

            // Assert
            Assert.AreEqual(typeof(IEnumerable<TestClass1>), target.EnumerableTarget);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ResultFactoryCreatesArray()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(
                typeof(IEnumerable), typeof(int[]), Stub.Create<ICollectionMerger>());

            var result = target.ResultFactory(new List<int> { 1, 2, 3 });

            // Assert
            Assert.IsInstanceOfType(result, typeof(int[]));
            new[] { 1, 2, 3 }.AssertSequenceEqual((int[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNull_ReturnsNull()
        {
            // Arrange
            var target = CreateTarget(typeof(int), Stub.Create<ICollectionMerger>());

            // Act
            var result = target.Transform(null, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget(typeof(int), Stub.Create<ICollectionMerger>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Transform(Enumerable.Empty<object>(), null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceIsGenericEnumerable_ReturnsArray()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var mergerMock = repository.Create<ICollectionMerger>();
            var target = CreateTarget(typeof(int), mergerMock.Object);
            
            var contextMock = repository.Create<IInjectionContext>();
            var registryMock = repository.Create<IInstanceRegistry>();
            var factoryMock = repository.Create<IEnumerableFactory>();

            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);
            registryMock.Setup(_ => _.Resolve<IEnumerableFactory>()).Returns(factoryMock.Object);

            var source = new object[] { };
            var targetEnumerable = new List<int> { 1, 2, 3 };
            factoryMock.Setup(_ => _.Resolve(typeof(IEnumerable<int>))).Returns(targetEnumerable);

            mergerMock.Setup(_ => _.Merge(source, targetEnumerable, contextMock.Object));

            // Act
            var result = target.Transform(source, contextMock.Object, null);

            // Assert
            repository.VerifyAll();
            new[] { 1, 2, 3 }.AssertSequenceEqual((int[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceIsEnumerable_ReturnsArray()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var mergerMock = repository.Create<ICollectionMerger>();
            var target = CreateTarget(typeof(int), mergerMock.Object);

            var contextMock = repository.Create<IInjectionContext>();
            var registryMock = repository.Create<IInstanceRegistry>();
            var factoryMock = repository.Create<IEnumerableFactory>();

            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);
            registryMock.Setup(_ => _.Resolve<IEnumerableFactory>()).Returns(factoryMock.Object);

            var source = Stub.Create<IEnumerable>();
            var targetEnumerable = new List<int> { 1, 2, 3 };
            factoryMock.Setup(_ => _.Resolve(typeof(IEnumerable<int>))).Returns(targetEnumerable);

            mergerMock.Setup(_ => _.Merge(source, targetEnumerable, contextMock.Object));

            // Act
            var result = target.Transform(source, contextMock.Object, null);

            // Assert
            repository.VerifyAll();
            new[] { 1, 2, 3 }.AssertSequenceEqual((int[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_CallsTransform()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[]), Stub.Create<ICollectionMerger>())
                { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context, null)).Returns(target);

            // Act
            targetMock.Object.Merge(source, new int[0], context, null);

            // Assert
            targetMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PostMergeActionIsReplace()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[]), Stub.Create<ICollectionMerger>()) 
                { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context, null)).Returns(target);

            // Act
            var result = targetMock.Object.Merge(source, new int[0], context, null);

            // Assert
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_TargetIsTransformResult()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[]), Stub.Create<ICollectionMerger>())
                { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context, null)).Returns(target);

            // Act
            var result = targetMock.Object.Merge(source, new int[0], context, null);

            // Assert
            Assert.AreEqual(target, result.Target);
        }

        private static EnumerableToArrayInjection CreateTarget(Type targetType, ICollectionMerger merger)
        {
            return new EnumerableToArrayInjection(typeof(IEnumerable), targetType.MakeArrayType(), merger);
        }
    }
}