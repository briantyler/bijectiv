// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableToEnumerableInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the EnumerableToEnumerableInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Injections;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="EnumerableToEnumerableInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableToEnumerableInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableToEnumerableInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var target = new EnumerableToEnumerableInjectionStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve<IInjection>(null, typeof(IEnumerable<TestClass1>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var target = new EnumerableToEnumerableInjectionStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve<IInjection>(typeof(IEnumerable), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotEnumerable_ReturnsNull()
        {
            // Arrange
            var target = new EnumerableToEnumerableInjectionStore();

            // Act
            var result = target.Resolve<IInjection>(TestClass1.T, typeof(IEnumerable));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameterIsNotEnumerable_ReturnsNull()
        {
            // Arrange
            var target = new EnumerableToEnumerableInjectionStore();

            // Act
            var result = target.Resolve<IInjection>(typeof(IEnumerable), TestClass1.T);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsEnumerableToEnumerableInjection()
        {
            // Arrange
            var target = new EnumerableToEnumerableInjectionStore();

            // Act
            var result = target.Resolve<IInjection>(typeof(IEnumerable), typeof(IEnumerable));

            // Assert
            Assert.IsInstanceOfType(result, typeof(EnumerableToEnumerableInjection));
            Assert.AreEqual(typeof(IEnumerable), result.Source);
            Assert.AreEqual(typeof(IEnumerable), result.Target);
        }
    }
}