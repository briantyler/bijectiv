// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectTests.cs" company="Bijectiv">
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
//   Defines the ReflectTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="Reflect{T}"/> class.
    /// </summary>
    [TestClass]
    public class ReflectTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Constructor_ExpressionParameterNull_Throws()
        {
            // Arrange

            // Act
            Reflect<MultiConstructorTestClass>.Constructor(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Constructor_ExpressionParameterIsNotConstructor_Throws()
        {
            // Arrange
            
            // Act
            Reflect<MultiConstructorTestClass>.Constructor(() => new MultiConstructorTestClass().Naught());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_NoParameters_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>.Constructor(() => new MultiConstructorTestClass());

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.IsFalse(ctor.GetParameters().Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_OneParameter_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>
                .Constructor(() => new MultiConstructorTestClass(Placeholder.Of<TestClass2>()));

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.AreEqual(typeof(TestClass2), ctor.GetParameters().Single().ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_TwoParameters_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>
                .Constructor(() => new MultiConstructorTestClass(Placeholder.Of<TestClass1>(), Placeholder.Of<TestClass2>()));

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.AreEqual(2, ctor.GetParameters().Length);
            Assert.AreEqual(typeof(TestClass1), ctor.GetParameters().ElementAt(0).ParameterType);
            Assert.AreEqual(typeof(TestClass2), ctor.GetParameters().ElementAt(1).ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MethodActionVariant_ExpressionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            Reflect<MultiConstructorTestClass>.Method(default(Expression<Action<MultiConstructorTestClass>>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void MethodActionVariant_ExpressionParameterIsNotMethod_Throws()
        {
            // Arrange

            // Act
            Reflect<MultiConstructorTestClass>.Method((Expression<Action<MultiConstructorTestClass>>)(_ => new MultiConstructorTestClass()));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MethodActionVariant_ExpressionParameterIsMethod_ReturnsMethod()
        {
            // Arrange

            // Act
            var method = Reflect<Stack<TestClass1>>.Method(_ => _.Push(Placeholder.Of<TestClass1>()));

            // Assert
            Assert.IsInstanceOfType(method, typeof(MethodInfo));
            Assert.AreEqual("Push", method.Name);
            Assert.AreEqual(typeof(TestClass1), method.GetParameters().Single().ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MethodFuncVariant_ExpressionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            Reflect<MultiConstructorTestClass>.Method(default(Expression<Func<MultiConstructorTestClass, int>>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void MethodFuncVariant_ExpressionParameterIsNotMethod_Throws()
        {
            // Arrange

            // Act
            Reflect<MultiConstructorTestClass>.Method(_ => 1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MethodFuncVariant_ExpressionParameterIsMethod_ReturnsMethod()
        {
            // Arrange

            // Act
            var method = Reflect<Stack<TestClass1>>.Method(_ => _.Pop());

            // Assert
            Assert.IsInstanceOfType(method, typeof(MethodInfo));
            Assert.AreEqual("Pop", method.Name);
            Assert.AreEqual(typeof(TestClass1), method.ReturnType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Property_ExpressionParameterNull_Throws()
        {
            // Arrange

            // Act
            Reflect<PropertyTestClass>.Property(default(Expression<Func<PropertyTestClass, int>>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Property_ExpressionParameterIsNotProperty_Throws()
        {
            // Arrange

            // Act
            Reflect<PropertyTestClass>.Property(_ => 1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Property_ExpressionParameterIsField_Throws()
        {
            // Arrange

            // Act
            Reflect<FieldTestClass>.Property(_ => _.Field);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Property_ExpressionParameterIsProperty_ReturnsProperty()
        {
            // Arrange

            // Act
            var property = Reflect<PropertyTestClass>.Property(_ => _.Property);

            // Assert
            Assert.IsInstanceOfType(property, typeof(PropertyInfo));
            Assert.AreEqual("Property", property.Name);
            Assert.AreEqual(typeof(int), property.PropertyType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Field_ExpressionParameterNull_Throws()
        {
            // Arrange

            // Act
            Reflect<FieldTestClass>.Field(default(Expression<Func<FieldTestClass, int>>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Field_ExpressionParameterIsNotField_Throws()
        {
            // Arrange

            // Act
            Reflect<FieldTestClass>.Field(_ => 1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Field_ExpressionParameterIsProperty_Throws()
        {
            // Arrange

            // Act
            Reflect<PropertyTestClass>.Field(_ => _.Property);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Field_ExpressionParameterIsField_ReturnsField()
        {
            // Arrange

            // Act
            var field = Reflect<FieldTestClass>.Field(_ => _.Field);

            // Assert
            Assert.IsInstanceOfType(field, typeof(FieldInfo));
            Assert.AreEqual("Field", field.Name);
            Assert.AreEqual(typeof(int), field.FieldType);
        }
    }
}