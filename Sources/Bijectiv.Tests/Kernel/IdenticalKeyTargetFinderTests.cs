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

namespace Bijectiv.Tests.Kernel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

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
            // ReSharper disable once AssignNullToNotNullAttribute
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
            // ReSharper disable once AssignNullToNotNullAttribute
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
            // ReSharper disable once AssignNullToNotNullAttribute
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
            var testTarget = new IdenticalKeyTargetFinder(sourceKeySelector, x => x, Stub.Create<IEqualityComparer<object>>());

            // Assert
            Assert.AreEqual(sourceKeySelector, testTarget.SourceKeySelector);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetKeySelectorParameter_IsAssignedToTargetKeySelectorProperty()
        {
            // Arrange
            Func<object, object> targetKeySelector = x => x;

            // Act
            var testTarget = new IdenticalKeyTargetFinder(x => x, targetKeySelector, Stub.Create<IEqualityComparer<object>>());

            // Assert
            Assert.AreEqual(targetKeySelector, testTarget.TargetKeySelector);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ComparerParameter_IsAssignedToComparer()
        {
            // Arrange
            var comparer = Stub.Create<IEqualityComparer<object>>();

            // Act
            var testTarget = new IdenticalKeyTargetFinder(x => x, x => x, comparer);

            // Assert
            Assert.AreEqual(comparer, testTarget.Comparer);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ComparerParameter_IsAssignedToTargetCache()
        {
            // Arrange
            var comparer = Stub.Create<IEqualityComparer<object>>();

            // Act
            var testTarget = new IdenticalKeyTargetFinder(x => x, x => x, comparer);

            // Assert
            Assert.AreEqual(comparer, ((dynamic)testTarget.TargetCache).Comparer);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Initialize_TargetsParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new IdenticalKeyTargetFinder(x => x, x => x, Stub.Create<IEqualityComparer<object>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Initialize(null, Stub.Create<IInjectionContext>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Initialize_ValidParameters_CachesTargetsAgainstSelectedKey()
        {
            // Arrange
            var targets = new[] { new TestClass1(), null, new TestClass1() };
            Func<object, object> targetKeySelector =
                t =>
                {
                    if (t == targets[0])
                    {
                        return 0;
                    }

                    if (t == targets[2])
                    {
                        return 2;
                    }

                    throw new ArgumentException("Unknown target", "t");
                };

            var testTarget = new IdenticalKeyTargetFinder(x => x, targetKeySelector, EqualityComparer<object>.Default);

            // Act
            testTarget.Initialize(targets, Stub.Create<IInjectionContext>());

            // Assert
            Assert.AreEqual(2, testTarget.TargetCache.Count());
            Assert.AreEqual(targets[0], testTarget.TargetCache[0]);
            Assert.AreEqual(targets[2], testTarget.TargetCache[2]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryFind_SourceIsNull_ReturnsFalse()
        {
            // Arrange
            var testTarget = new IdenticalKeyTargetFinder(x => x, x => x, Stub.Create<IEqualityComparer<object>>());
            object target;

            // Act
            var result = testTarget.TryFind(null, out target);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryFind_SourceKeyIsNotPresent_ReturnsFalse()
        {
            // Arrange
            var testTarget = new IdenticalKeyTargetFinder(x => x, x => x, EqualityComparer<object>.Default);
            object target;

            // Act
            var result = testTarget.TryFind(new object(), out target);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryFind_SourceKeyIsPresent_ReturnsTrue()
        {
            // Arrange
            var testTarget = new IdenticalKeyTargetFinder(x => 1, x => x, EqualityComparer<object>.Default);
            var expected = new TestClass1();
            testTarget.TargetCache[1] = expected;
            object target;

            // Act
            var result = testTarget.TryFind(new object(), out target);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expected, target);
        }
    }
}