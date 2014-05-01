// --------------------------------------------------------------------------------------------------------------------
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
    using System.Reflection;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}