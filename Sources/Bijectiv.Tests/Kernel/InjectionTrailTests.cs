﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTrailTests.cs" company="Bijectiv">
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
//   Defines the InjectionTrailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InjectionTrail"/> class.
    /// </summary>
    [TestClass]
    public class InjectionTrailTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionTrail().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_ItemParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionTrail { null }.Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameter_IsAddedToTrail()
        {
            // Arrange
            var item = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), new object());
            
            // Act
            var testTarget = new InjectionTrail { item };

            // Assert
            Assert.IsTrue(testTarget.Contains(item));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterTarget_IsAddedToTargets()
        {
            // Arrange
            var target = new object();
            var item = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), target);

            // Act
            var testTarget = new InjectionTrail { item };

            // Assert
            Assert.IsTrue(testTarget.Targets.Contains(target));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterTargetIsNull_TargetIsNotAddedToTargets()
        {
            // Arrange
            var item = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), null);

            // Act
            var testTarget = new InjectionTrail { item };

            // Assert
            Assert.IsFalse(testTarget.Targets.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterTargetIsNotInTargets_ReturnsTrue()
        {
            // Arrange
            var item = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), new object());
            var testTarget = new InjectionTrail();

            // Act
            var result = testTarget.Add(item);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterTargetIsInTargets_ReturnsFalse()
        {
            // Arrange
            var target = new object();
            var item1 = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), target);
            var item2 = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), target);
            var testTarget = new InjectionTrail { item1 };

            // Act
            var result = testTarget.Add(item2);

            // Assert
            Assert.IsTrue(testTarget.Contains(item2));
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ContainsTarget_TargetParameterIsNotContainedInTrail_ReturnsFalse()
        {
            // Arrange
            var testTarget = new InjectionTrail();

            // Act
            var result = testTarget.ContainsTarget(new object());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ContainsTarget_TargetParameterIsContainedInTrail_ReturnsTrue()
        {
            // Arrange
            var target = new object();
            var item = new InjectionTrailItem(Stub.Create<IInjection>(), new object(), target);
            var testTarget = new InjectionTrail { item };

            // Act
            var result = testTarget.ContainsTarget(target);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_WeakVersion_GetsEnumerator()
        {
            // Arrange
            var items = new[]
            {
                new InjectionTrailItem(Stub.Create<IInjection>(), new object(), new object()),
                new InjectionTrailItem(Stub.Create<IInjection>(), new object(), new object()),
                new InjectionTrailItem(Stub.Create<IInjection>(), new object(), new object())
            };
            var testTarget = new InjectionTrail();
            items.ForEach(item => testTarget.Add(item));
            var result = new List<object>();

            // Act
            // Forces the non-generice GetEnumerator() to be invokec.
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var obj in (IEnumerable)testTarget)
            {
                result.Add(obj);
            }

            // Assert
            items.AssertSequenceEqual(result);
        }
    }
}