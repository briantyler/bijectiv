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
            new EnumerableToArrayInjection(null, typeof(TestClass1[])).Naught();

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
            new EnumerableToArrayInjection(typeof(IEnumerable), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceParameterIsNotEnumerable_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(typeof(int), typeof(TestClass1[])).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsNotArray_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(typeof(IEnumerable), typeof(Collection<TestClass1>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsMultiDimensionalArray_Throws()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[,])).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[])).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourcePropertyIsEnumerable()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(typeof(Stack<int>), typeof(TestClass1[]));

            // Assert
            Assert.AreEqual(typeof(IEnumerable), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[]));

            // Assert
            Assert.AreEqual(typeof(TestClass1[]), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterElementType_IsAssignedToTargetElementProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(typeof(IEnumerable), typeof(TestClass1[]));

            // Assert
            Assert.AreEqual(typeof(TestClass1), target.TargetElement);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ResultFactoryCreatesArray()
        {
            // Arrange

            // Act
            var target = new EnumerableToArrayInjection(typeof(IEnumerable), typeof(int[]));
            var result = target.ResultFactory(new List<object> { 1, 2, 3 });

            // Assert
            Assert.IsInstanceOfType(result, typeof(int[]));
            new[] { 1, 2, 3 }.AssertSequenceEqual((int[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNull_ReturnsNull()
        {
            // Arrange
            var target = CreateTarget(typeof(int));

            // Act
            var result = target.Transform(null, Stub.Create<IInjectionContext>());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget(typeof(int));

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = target.Transform(Enumerable.Empty<object>(), null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_TargetIsValueType_ReturnsArray()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget(typeof(int));
            
            var transformMock = repository.Create<ITransform>(MockBehavior.Strict);
            var storeMock = repository.Create<IInjectionStore>(MockBehavior.Strict);
            var contextMock = repository.Create<IInjectionContext>(MockBehavior.Strict);

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(It.IsAny<Type>(), typeof(int))).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform("1", contextMock.Object)).Returns(1);
            transformMock.Setup(_ => _.Transform(true, contextMock.Object)).Returns(2);
            transformMock.Setup(_ => _.Transform(typeof(double), contextMock.Object)).Returns(3);

            // Act
            var result = target.Transform(new object[] { "1", true, null, typeof(double) }, contextMock.Object);

            // Assert
            repository.VerifyAll();
            new[] { 1, 2, 0, 3 }.AssertSequenceEqual((int[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_TargetIsReferenceType_ReturnsArray()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget(typeof(string));

            var transformMock = repository.Create<ITransform>(MockBehavior.Strict);
            var storeMock = repository.Create<IInjectionStore>(MockBehavior.Strict);
            var contextMock = repository.Create<IInjectionContext>(MockBehavior.Strict);

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(It.IsAny<Type>(), typeof(string))).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(1, contextMock.Object)).Returns("1");
            transformMock.Setup(_ => _.Transform(true, contextMock.Object)).Returns("2");
            transformMock.Setup(_ => _.Transform(typeof(double), contextMock.Object)).Returns("3");

            // Act
            var result = target.Transform(new object[] { 1, true, null, typeof(double) }, contextMock.Object);

            // Assert
            repository.VerifyAll();
            new[] { "1", "2", null, "3" }.AssertSequenceEqual((string[])result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_CallsTransform()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[])) { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context)).Returns(target);

            // Act
            targetMock.Object.Merge(source, new int[0], context);

            // Assert
            targetMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PostMergeActionIsReplace()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[])) { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context)).Returns(target);

            // Act
            var result = targetMock.Object.Merge(source, new int[0], context);

            // Assert
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_TargetIsTransformResult()
        {
            // Arrange
            var targetMock = new Mock<EnumerableToArrayInjection>(
                MockBehavior.Strict, typeof(IEnumerable), typeof(int[])) { CallBase = false };

            var source = Stub.Create<IEnumerable>();
            var context = Stub.Create<IInjectionContext>();
            var target = new int[0];

            targetMock.Setup(_ => _.Transform(source, context)).Returns(target);

            // Act
            var result = targetMock.Object.Merge(source, new int[0], context);

            // Assert
            Assert.AreEqual(target, result.Target);
        }

        private static EnumerableToArrayInjection CreateTarget(Type targetType)
        {
            return new EnumerableToArrayInjection(typeof(IEnumerable), targetType.MakeArrayType());
        }
    }
}