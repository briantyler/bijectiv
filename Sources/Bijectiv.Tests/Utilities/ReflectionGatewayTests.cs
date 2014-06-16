// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionGatewayTests.cs" company="Bijectiv">
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
//   Defines the ReflectionGatewayTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Reflection;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using JetBrains.Annotations;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ReflectionGateway"/> class.
    /// </summary>
    [TestClass]
    public class ReflectionGatewayTests
    {
        private static readonly FieldInfo[] Fields = { FieldTestClass.FieldFi, FieldTestClass.ReadonlyFieldFi };

        private static readonly PropertyInfo[] Properties =
        {
            PropertyTestClass.PropertyPi,
            PropertyTestClass.ReadonlyPropertyPi,
            PropertyTestClass.WriteonlyPropertyPi
        };

        [TestMethod]
        [TestCategory("Unit")]
        public void InstanceBindingFlags_Value_IsInstancePublic()
        {
            // Arrange
            const BindingFlags Expected = BindingFlags.Instance | BindingFlags.Public;

            // Act
            const BindingFlags Actual = ReflectionGateway.Instance;

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NonPublicInstanceBindingFlags_Value_IsInstancePublicNonPublic()
        {
            // Arrange
            const BindingFlags Expected = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            // Act
            const BindingFlags Actual = ReflectionGateway.NonPublicInstance;

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ReflectionGateway().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetFields_TypeParameterIsNull_Throws()
        {
            // Arrange
            var target = new ReflectionGateway();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.GetFields(null, ReflectionOptions.None);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetFields_OptionsParameterNone_UsesInstanceBindingFlags()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetFields(ReflectionGateway.Instance)).Returns(new FieldInfo[0]);

            var target = new ReflectionGateway();

            // Act
            target.GetFields(typeMock.Object, ReflectionOptions.None);

            // Assert
            typeMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetFields_OptionsParameterNone_ReturnsAll()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetFields(It.IsAny<BindingFlags>())).Returns(Fields);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetFields(typeMock.Object, ReflectionOptions.None);

            // Assert
            Fields.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetFields_OptionsIncludeNonPublic_UsesNonPublicInstanceBindingFlags()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock
                .Setup(_ => _.GetFields(ReflectionGateway.NonPublicInstance))
                .Returns(new FieldInfo[0]);

            var target = new ReflectionGateway();

            // Act
            target.GetFields(typeMock.Object, ReflectionOptions.IncludeNonPublic);

            // Assert
            typeMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetFields_OptionsParameterCanRead_ReturnsAll()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetFields(It.IsAny<BindingFlags>())).Returns(Fields);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetFields(typeMock.Object, ReflectionOptions.CanRead);

            // Assert
            Fields.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetFields_OptionsParameterCanWrite_ReturnsWritable()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetFields(It.IsAny<BindingFlags>())).Returns(Fields);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetFields(typeMock.Object, ReflectionOptions.CanWrite);

            // Assert
            new[] { Fields[0] }.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetProperties_TypeParameterIsNull_Throws()
        {
            // Arrange
            var target = new ReflectionGateway();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.GetProperties(null, ReflectionOptions.None);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsNone_UsesInstanceBindingFlags()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetProperties(ReflectionGateway.Instance)).Returns(new PropertyInfo[0]);

            var target = new ReflectionGateway();

            // Act
            target.GetProperties(typeMock.Object, ReflectionOptions.None);

            // Assert
            typeMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsNone_ReturnsAll()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetProperties(ReflectionGateway.Instance)).Returns(Properties);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetProperties(typeMock.Object, ReflectionOptions.None);

            // Assert
            Properties.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsIncludeNonPublic_UsesNonPublicInstanceBindingFlags()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock
                .Setup(_ => _.GetProperties(ReflectionGateway.NonPublicInstance))
                .Returns(new PropertyInfo[0]);

            var target = new ReflectionGateway();

            // Act
            target.GetProperties(typeMock.Object, ReflectionOptions.IncludeNonPublic);

            // Assert
            typeMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsCanRead_ReturnsReadable()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetProperties(It.IsAny<BindingFlags>())).Returns(Properties);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetProperties(typeMock.Object, ReflectionOptions.CanRead);

            // Assert
            new[] { Properties[0], Properties[1] }.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsCanWrite_ReturnsWritable()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetProperties(It.IsAny<BindingFlags>())).Returns(Properties);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetProperties(typeMock.Object, ReflectionOptions.CanWrite);

            // Assert
            new[] { Properties[0], Properties[2] }.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetProperties_OptionsCanReadCanWrite_ReturnsReadableAndWritable()
        {
            // Arrange
            var typeMock = new Mock<Type>(MockBehavior.Strict);
            typeMock.Setup(_ => _.GetProperties(It.IsAny<BindingFlags>())).Returns(Properties);

            var target = new ReflectionGateway();

            // Act
            var result = target.GetProperties(typeMock.Object, ReflectionOptions.CanRead | ReflectionOptions.CanWrite);

            // Assert
            new[] { Properties[0] }.AssertSequenceEqual(result);
        }
    }
}