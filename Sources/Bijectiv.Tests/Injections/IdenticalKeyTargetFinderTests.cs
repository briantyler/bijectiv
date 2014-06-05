// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdenticalKeyTargetFinderTests.cs" company="Bijectiv">
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
//   Defines the IdenticalKeyTargetFinderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="IdenticalKeyTargetFinder"/> class.
    /// </summary>
    [TestClass]
    public class IdenticalKeyTargetFinderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceKeySelectorParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new IdenticalKeyTargetFinder(null, x => x, Stub.Create<IEqualityComparer<object>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetKeySelectorParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new IdenticalKeyTargetFinder(x => x, null, Stub.Create<IEqualityComparer<object>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ComparerParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new IdenticalKeyTargetFinder(x => x, x => x, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new IdenticalKeyTargetFinder(x => x, x => x, Stub.Create<IEqualityComparer<object>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceKeySelectorParameter_IsAssignedToSourceKeySelectorProperty()
        {
            // Arrange
            Func<object, object> sourceKeySelector = x => x;

            // Act
            var target = new IdenticalKeyTargetFinder(sourceKeySelector, x => x, Stub.Create<IEqualityComparer<object>>());

            // Assert
            Assert.AreEqual(sourceKeySelector, target.SourceKeySelector);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetKeySelectorParameter_IsAssignedToTargetKeySelectorProperty()
        {
            // Arrange
            Func<object, object> targetKeySelector = x => x;

            // Act
            var target = new IdenticalKeyTargetFinder(x => x, targetKeySelector, Stub.Create<IEqualityComparer<object>>());

            // Assert
            Assert.AreEqual(targetKeySelector, target.TargetKeySelector);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ComparerParameter_IsAssignedToTargetCache()
        {
            // Arrange
            var comparer = Stub.Create<IEqualityComparer<object>>();

            // Act
            var target = new IdenticalKeyTargetFinder(x => x, x => x, comparer);

            // Assert
            Assert.AreEqual(comparer, ((dynamic)target.TargetCache).Comparer);
        }
    }
}