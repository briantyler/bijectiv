// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullTargetFinderTests.cs" company="Bijectiv">
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
//   Defines the NullTargetFinderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using System.Collections;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="NullTargetFinder"/> class.
    /// </summary>
    [TestClass]
    public class NullTargetFinderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NullTargetFinder().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Initialize_TargetsParameterIsNull_DoesNothing()
        {
            // Arrange
            var target = new NullTargetFinder();

            // Act
            target.Initialize(null, Stub.Create<IInjectionContext>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Initialize_ContextParameterIsNull_DoesNothing()
        {
            // Arrange
            var target = new NullTargetFinder();

            // Act
            target.Initialize(Stub.Create<IEnumerable>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Initialize_ValidParameters_DoesNothing()
        {
            // Arrange
            var target = new NullTargetFinder();

            // Act
            target.Initialize(Stub.Create<IEnumerable>(), Stub.Create<IInjectionContext>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryFind_SourceParameterIsNull_ReturnsFalse()
        {
            // Arrange
            var target = new NullTargetFinder();

            // Act
            object targetElement;
            var result = target.TryFind(null, out targetElement);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryFind_SourceParameterIsAnything_ReturnsFalse()
        {
            // Arrange
            var target = new NullTargetFinder();

            // Act
            object targetElement;
            var result = target.TryFind(Stub.Create<object>(), out targetElement);

            // Assert
            Assert.IsFalse(result);
        }
    }
}