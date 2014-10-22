// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PassThroughInjectionTests.cs" company="Bijectiv">
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
//   Defines the PassThroughInjectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Kernel
{
    using System;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="PassThroughInjection"/> class.
    /// </summary>
    [TestClass]
    public class PassThroughInjectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new PassThroughInjection(typeof(object), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new PassThroughInjection(null, typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new PassThroughInjection(typeof(object), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsNotAssignableFromSourceParameter_Throws()
        {
            // Arrange

            // Act
            new PassThroughInjection(typeof(int), typeof(string)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TypesIdentical_InstanceCreated()
        {
            // Arrange

            // Act
            new PassThroughInjection(typeof(DateTime), typeof(DateTime)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameterIsAssignableFromSourceParameter_InstanceCreated()
        {
            // Arrange

            // Act
            new PassThroughInjection(typeof(int), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var testTarget = new PassThroughInjection(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(int), testTarget.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var testTarget = new PassThroughInjection(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(object), testTarget.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNull_ReturnsNull()
        {
            // Arrange
            var testTarget = new PassThroughInjection(typeof(object), typeof(object));

            // Act
            var result = testTarget.Transform(null, null, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceParameterIsNotNull_ReturnsSourceParameter()
        {
            // Arrange
            var testTarget = new PassThroughInjection(typeof(int), typeof(int));

            // Act
            var result = testTarget.Transform(7, null, null);

            // Assert
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PostMergeActionIsReplace()
        {
            // Arrange
            var testTarget = new PassThroughInjection(typeof(int), typeof(int));

            // Act
            var result = testTarget.Merge(7, 1, null, null);

            // Assert
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_TargetIsAssigned()
        {
            // Arrange
            var testTarget = new PassThroughInjection(typeof(int), typeof(int));

            // Act
            var result = testTarget.Merge(7, 1, null, null);

            // Assert
            Assert.AreEqual(7, result.Target);
        }
    }
}