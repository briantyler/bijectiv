// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleTransformTests.cs" company="Bijectiv">
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
//   Defines the ConvertibleTransformTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Injections
{
    using System;
    using System.Globalization;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ConvertibleTransform"/> class.
    /// </summary>
    [TestClass]
    public class ConvertibleTransformTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new ConvertibleTransform(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterIsConvertibleTarget_InstanceCreated()
        {
            // Arrange

            // Act
            foreach (var type in TypeClasses.ConvertibleTargetTypes)
            {
                new ConvertibleTransform(type).Naught();
            }

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsNotConvertibleType_Throws()
        {
            // Arrange

            // Act
            new ConvertibleTransform(typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourcePropertyIsIconvertible()
        {
            // Arrange

            // Act
            var target = new ConvertibleTransform(typeof(int));

            // Assert
            Assert.AreEqual(typeof(IConvertible), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new ConvertibleTransform(typeof(int));

            // Assert
            Assert.AreEqual(typeof(int), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNullTargetTypeIsNotClass_ReturnsDefault()
        {
            // Arrange
            var target = new ConvertibleTransform(typeof(bool));

            // Act
            var result = target.Transform(null, Stub.Create<IInjectionContext>());

            // Assert
            Assert.AreEqual(default(bool), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNullTargetTypeIsClass_ReturnsDefault()
        {
            // Arrange
            var target = new ConvertibleTransform(typeof(string));

            // Act
            var result = target.Transform(null, Stub.Create<IInjectionContext>());

            // Assert
            Assert.AreEqual(default(string), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var target = new ConvertibleTransform(typeof(bool));

            // Act
            target.Transform(true, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidSourceParameter_ReturnsConvertedTarget()
        {
            // Arrange
            var target = new ConvertibleTransform(typeof(bool));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Transform("TRUE", contextMock.Object);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidSourceParameter_ConvertsUsingInjectionContextCulture()
        {
            // Arrange
            var target = new ConvertibleTransform(typeof(DateTime));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(new CultureInfo("en-US"));

            // Act
            var result = target.Transform("04/06/2014", contextMock.Object);

            // Assert
            Assert.AreEqual(new DateTime(2014, 04, 06), result);
        }
    }
}