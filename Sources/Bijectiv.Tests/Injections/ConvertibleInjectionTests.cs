// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleInjectionTests.cs" company="Bijectiv">
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
//   Defines the ConvertibleInjectionTests type.
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
    /// This class tests the <see cref="ConvertibleInjection"/> class.
    /// </summary>
    [TestClass]
    public class ConvertibleInjectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new ConvertibleInjection(null).Naught();

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
                new ConvertibleInjection(type).Naught();
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
            new ConvertibleInjection(typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourcePropertyIsIconvertible()
        {
            // Arrange

            // Act
            var target = new ConvertibleInjection(typeof(int));

            // Assert
            Assert.AreEqual(typeof(IConvertible), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new ConvertibleInjection(typeof(int));

            // Assert
            Assert.AreEqual(typeof(int), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNullTargetTypeIsNotClass_ReturnsDefault()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(bool));

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
            var target = new ConvertibleInjection(typeof(string));

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
            var target = new ConvertibleInjection(typeof(bool));

            // Act
            target.Transform(true, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidSourceParameter_ReturnsConvertedTarget()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(bool));
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
            var target = new ConvertibleInjection(typeof(DateTime));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(new CultureInfo("en-US"));

            // Act
            var result = target.Transform("04/06/2014", contextMock.Object);

            // Assert
            Assert.AreEqual(new DateTime(2014, 04, 06), result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_StringScientificNotationToFloat_Converts()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(float));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Transform("2.3e4", contextMock.Object);

            // Assert
            Assert.AreEqual(2.3e4f, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_StringScientificNotationToDouble_Converts()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(double));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Transform("2.3e4", contextMock.Object);

            // Assert
            Assert.AreEqual(2.3e4, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_StringScientificNotationToDecimal_Converts()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(decimal));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Transform("2.3e4", contextMock.Object);

            // Assert
            Assert.AreEqual(2.3e4m, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PostMergeActionIsReplace()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(bool));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Merge("TRUE", false, contextMock.Object);

            // Assert
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_TargetIsAssigned()
        {
            // Arrange
            var target = new ConvertibleInjection(typeof(bool));
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = target.Merge("TRUE", false, contextMock.Object);

            // Assert
            Assert.AreEqual(true, result.Target);
        }
    }
}