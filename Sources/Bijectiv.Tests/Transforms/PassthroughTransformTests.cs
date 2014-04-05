// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PassthroughTransformTests.cs" company="Bijectiv">
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
//   Defines the PassthroughTransformTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Transforms
{
    using System;

    using Bijectiv.Tests.Tools;
    using Bijectiv.Transforms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="PassthroughTransform"/> class.
    /// </summary>
    [TestClass]
    public class PassthroughTransformTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new PassthroughTransform(typeof(object), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new PassthroughTransform(null, typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new PassthroughTransform(typeof(object), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateInstance_TargetParameterIsNotAssignableFromSourceParameter_Throws()
        {
            // Arrange

            // Act
            new PassthroughTransform(typeof(int), typeof(string)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TypesIdentical_InstanceCreated()
        {
            // Arrange

            // Act
            new PassthroughTransform(typeof(DateTime), typeof(DateTime)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterIsAssignableFromSourceParameter_InstanceCreated()
        {
            // Arrange

            // Act
            new PassthroughTransform(typeof(int), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = new PassthroughTransform(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(int), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new PassthroughTransform(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(object), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNull_ReturnsNull()
        {
            // Arrange
            var target = new PassthroughTransform(typeof(object), typeof(object));

            // Act
            var result = target.Transform(null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNotNull_ReturnsSourceParameter()
        {
            // Arrange
            var target = new PassthroughTransform(typeof(int), typeof(int));

            // Act
            var result = target.Transform(7, null);

            // Assert
            Assert.AreEqual(7, result);
        }
    }
}