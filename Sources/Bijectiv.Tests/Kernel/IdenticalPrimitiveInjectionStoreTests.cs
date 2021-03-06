﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdenticalPrimitiveInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the IdenticalPrimitiveInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Kernel
{
    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="IdenticalPrimitiveInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class IdenticalPrimitiveInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new IdenticalPrimitiveInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            // Act
            testTarget.Resolve<IInjection>(null, typeof(int));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            // Act
            testTarget.Resolve<IInjection>(typeof(int), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotEqualToTargetParameter_ReturnsNull()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            // Act
            var result = testTarget.Resolve<IInjection>(typeof(int), typeof(uint));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotPrimitive_ReturnsNull()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            // Act
            var result = testTarget.Resolve<IInjection>(typeof(object), typeof(object));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsPrimitive_ReturnsPassthroughTransform()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            foreach (var type in TypeClasses.PrimitiveTypes)
            {
                // Act
                var result = testTarget.Resolve<IInjection>(type, type);

                // Assert
                Assert.IsInstanceOfType(result, typeof(PassThroughInjection));
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TransformTypeParameter_ReturnsPassthroughTransform()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            foreach (var type in TypeClasses.PrimitiveTypes)
            {
                // Act
                var result = testTarget.Resolve<ITransform>(type, type);

                // Assert
                Assert.IsInstanceOfType(result, typeof(PassThroughInjection));
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_MergeTypeParameter_ReturnsPassthroughTransform()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            foreach (var type in TypeClasses.PrimitiveTypes)
            {
                // Act
                var result = testTarget.Resolve<ITransform>(type, type);

                // Assert
                Assert.IsInstanceOfType(result, typeof(PassThroughInjection));
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameter_IsAssignedToPassthoughSourceProperty()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            foreach (var type in TypeClasses.PrimitiveTypes)
            {
                // Act
                var result = (PassThroughInjection)testTarget.Resolve<IInjection>(type, type);

                // Assert
                Assert.AreEqual(type, result.Source);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameter_IsAssignedToPassthoughTargetProperty()
        {
            // Arrange
            var testTarget = new IdenticalPrimitiveInjectionStore();

            foreach (var type in TypeClasses.PrimitiveTypes)
            {
                // Act
                var result = (PassThroughInjection)testTarget.Resolve<IInjection>(type, type);

                // Assert
                Assert.AreEqual(type, result.Target);
            }
        }
    }
}