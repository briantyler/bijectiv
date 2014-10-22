// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToArrayInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the EnumerableToArrayInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class test the <see cref="EnumerableToArrayInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableToArrayInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableToArrayInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(null, typeof(TestClass1[]));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(typeof(IEnumerable), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotEnumerable_ReturnsNull()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<IInjection>(TestClass1.T, typeof(TestClass1[]));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameterIsNotArray_ReturnsNull()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<IInjection>(typeof(IEnumerable), typeof(List<TestClass1>));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameterIsMultiDimensionalArray_ReturnsNull()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<IInjection>(typeof(IEnumerable), typeof(TestClass1[,]));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsInjection()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<IInjection>(typeof(IEnumerable), typeof(TestClass1[]));

            // Assert
            Assert.IsInstanceOfType(result, typeof(EnumerableToArrayInjection));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsTransform()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<ITransform>(typeof(IEnumerable), typeof(TestClass1[]));

            // Assert
            Assert.IsInstanceOfType(result, typeof(EnumerableToArrayInjection));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsMerge()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.Resolve<IMerge>(typeof(IEnumerable), typeof(TestClass1[]));

            // Assert
            Assert.IsInstanceOfType(result, typeof(EnumerableToArrayInjection));
        }

        private static EnumerableToArrayInjectionStore CreateTestTarget()
        {
            return new EnumerableToArrayInjectionStore();
        }
    }
}