﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTests.cs" company="Bijectiv">
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
//   Defines the ReflectionExtensionsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1720
namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ReflectionExtensions"/> class.
    /// </summary>
    [TestClass]
    public class ReflectionExtensionsTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanRead_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(MemberInfo).CanRead();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_NormalField_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<FieldTestClass>.Field(_ => _.Field).CanRead();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_ReadonlyField_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<FieldTestClass>.Field(_ => _.ReadonlyField).CanRead();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_NormalProperty_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Property(_ => _.Property).CanRead();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_ReadonlyProperty_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Property(_ => _.ReadonlyProperty).CanRead();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_WriteonlyProperty_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = PropertyTestClass.WriteonlyPropertyPi.CanRead();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanRead_AnyMethod_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = Reflect<TestClass1>.Method(_ => _.GetHashCode()).CanRead();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanWrite_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(MemberInfo).CanWrite();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_NormalField_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<FieldTestClass>.Field(_ => _.Field).CanWrite();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_ReadonlyField_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = Reflect<FieldTestClass>.Field(_ => _.ReadonlyField).CanWrite();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_NormalProperty_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Property(_ => _.Property).CanWrite();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_ReadonlyProperty_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Property(_ => _.ReadonlyProperty).CanWrite();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_WriteonlyProperty_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = PropertyTestClass.WriteonlyPropertyPi.CanWrite();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanWrite_AnyMethod_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = Reflect<TestClass1>.Method(_ => _.GetHashCode()).CanWrite();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetAccessExpression_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(MethodInfo).GetAccessExpression(Stub.Create<Expression>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetAccessExpression_InstanceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Stub.Create<MethodInfo>().GetAccessExpression(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void GetAccessExpression_ThisParameterIsNotPropertyOrField_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Stub.Create<MethodInfo>().GetAccessExpression(Stub.Create<Expression>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetAccessExpression_ThisParameterIsProperty_CreatesPropertyAccess()
        {
            // Arrange
            var instance = new PropertyTestClass { Property = 123 };

            // Act
            var expression = Reflect<PropertyTestClass>
                .Property(_ => _.Property)
                .GetAccessExpression(Expression.Constant(instance));

            // Assert
            Assert.AreEqual(123, Expression.Lambda<Func<int>>(expression).Compile()());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetAccessExpression_ThisParameterIsField_CreatesFieldAccess()
        {
            // Arrange
            var instance = new FieldTestClass { Field = 123 };

            // Act
            var expression = Reflect<FieldTestClass>
                .Field(_ => _.Field)
                .GetAccessExpression(Expression.Constant(instance));

            // Assert
            Assert.AreEqual(123, Expression.Lambda<Func<int>>(expression).Compile()());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetReturnType_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(MethodInfo).GetReturnType();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void GetReturnType_ThisParameterIsUnexpectedType_Throws()
        {
            // Arrange

            // Act
            Stub.Create<MemberInfo>().GetReturnType();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetReturnType_ThisParameterIsField_ReturnsFieldType()
        {
            // Arrange

            // Act
            var result = Reflect<FieldTestClass>.Field(_ => _.Field).GetReturnType();

            // Assert
            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetReturnType_ThisParameterIsProperty_ReturnsPropertyType()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Property(_ => _.Property).GetReturnType();

            // Assert
            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetReturnType_ThisParameterIsMethod_ReturnsMethodReturnType()
        {
            // Arrange

            // Act
            var result = Reflect<PropertyTestClass>.Method(_ => _.GetHashCode()).GetReturnType();

            // Assert
            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetDefault_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(Type).GetDefault();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetDefault_ThisParameterIsValueType_ReturnsDefault()
        {
            // Arrange

            // Act
            var result = typeof(int).GetDefault();

            // Assert
            Assert.AreEqual(default(int), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetDefault_ThisParameterIsNotValueType_ReturnsDefault()
        {
            // Arrange

            // Act
            var result = typeof(TestClass1).GetDefault();

            // Assert
            Assert.AreEqual(default(TestClass1), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetBaseDefinition_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(FieldInfo).GetBaseDefinition();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void GetBaseDefinition_ThisParameterIsEvent_Throws()
        {
            // Arrange
            var testTarget = typeof(EventTestClass).GetEvent("SomethingHappened");

            // Act
            testTarget.GetBaseDefinition();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetBaseDefinition_ThisParameterIsField_ReturnsThisParameter()
        {
            // Arrange
            var testTarget = Reflect<FieldTestClass>.Field(_ => _.Field);

            // Act
            var result = testTarget.GetBaseDefinition();

            // Assert
            Assert.AreEqual(testTarget, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetBaseDefinition_ThisParameterIsMethod_ReturnsMethodGetBaseDefinition()
        {
            // Arrange
            var testTargetMock = new Mock<MethodInfo>(MockBehavior.Strict);
            var baseMethod = Stub.Create<MethodInfo>();
            testTargetMock.Setup(_ => _.GetBaseDefinition()).Returns(baseMethod);

            // Act
            var result = ReflectionExtensions.GetBaseDefinition(testTargetMock.Object);

            // Assert
            testTargetMock.VerifyAll();
            Assert.AreEqual(baseMethod, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetBaseDefinition_ThisParameterIsNonOverridingProperty_ReturnsThisParameter()
        {
            // Arrange
            var testTarget = typeof(MemberInfoHierarchy1).GetProperty("Id");

            // Act
            var result = testTarget.GetBaseDefinition();

            // Assert
            Assert.AreEqual(testTarget, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetBaseDefinition_ThisParameterIsOverridingProperty_ReturnsBaseProperty()
        {
            // Arrange
            var testTarget = typeof(MemberInfoHierarchy4).GetProperty("Id");

            // Act
            var result = testTarget.GetBaseDefinition();

            // Assert
            Assert.AreEqual(typeof(MemberInfoHierarchy3).GetProperty("Id"), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void IsOverride_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(FieldInfo).IsOverride();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void IsOverride_ThisParameterIsEvent_Throws()
        {
            // Arrange
            var testTarget = typeof(EventTestClass).GetEvent("SomethingHappened");

            // Act
            testTarget.IsOverride();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsOverride_ThisParameterIsField_ReturnsFalse()
        {
            // Arrange
            var testTarget = Reflect<FieldTestClass>.Field(_ => _.Field);

            // Act
            var result = testTarget.IsOverride();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsOverride_ThisParameterIsMethod_IsVirtualAndNotNewSlotReturnsTrue()
        {
            // Arrange
            var testTargetMock = new Mock<MethodInfo>(MockBehavior.Strict);
            testTargetMock.SetupGet(_ => _.Attributes).Returns(MethodAttributes.Virtual);

            // Act
            var result = testTargetMock.Object.IsOverride();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsOverride_ThisParameterIsMethod_IsVirtualAndNewSlotReturnsFalse()
        {
            // Arrange
            var testTargetMock = new Mock<MethodInfo>(MockBehavior.Strict);
            testTargetMock.SetupGet(_ => _.Attributes).Returns(MethodAttributes.Virtual & MethodAttributes.NewSlot);

            // Act
            var result = testTargetMock.Object.IsOverride();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsOverride_ThisParameterIsMethod_IsNothingReturnsFalse()
        {
            // Arrange
            var testTargetMock = new Mock<MethodInfo>(MockBehavior.Strict);
            testTargetMock.SetupGet(_ => _.Attributes).Returns(0);

            // Act
            var result = testTargetMock.Object.IsOverride();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsOverride_ThisParameterIsProperty_UsesFirstAccessor()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var methodMock = repository.Create<MethodInfo>(MockBehavior.Strict);
            methodMock.SetupGet(_ => _.Attributes).Returns(0);

            var testTargetMock = repository.Create<PropertyInfo>(MockBehavior.Strict);
            testTargetMock.Setup(_ => _.GetAccessors(true)).Returns(new[] { methodMock.Object });

            // Act
            var result = testTargetMock.Object.IsOverride();

            // Assert
            repository.VerifyAll();
            Assert.IsFalse(result);
        }
    }
}