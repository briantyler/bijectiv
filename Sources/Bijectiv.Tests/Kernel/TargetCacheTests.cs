// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetCacheTests.cs" company="Bijectiv">
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
//   Defines the TargetCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Kernel
{
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TargetCache"/> class.
    /// </summary>
    [TestClass]
    public class TargetCacheTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TargetCache().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CacheIsEmpty()
        {
            // Arrange

            // Act
            var target = new TargetCache();

            // Assert
            Assert.IsFalse(target.Cache.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_SourceTypeParameterNull_Throws()
        {
            // Arrange
            var target = new TargetCache();

            // Act
            target.Add(null, TestClass2.T, new object(), new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_TargetTypeParameterNull_Throws()
        {
            // Arrange
            var target = new TargetCache();

            // Act
            target.Add(TestClass1.T, null, new object(), new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_SourceParameterNull_Throws()
        {
            // Arrange
            var target = new TargetCache();

            // Act
            target.Add(TestClass1.T, TestClass2.T, null, new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_TargetParameterNull_Throws()
        {
            // Arrange
            var target = new TargetCache();

            // Act
            target.Add(TestClass1.T, TestClass2.T, new object(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsToCache()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance = new object();
            var targetInstance = new object();
            object outputTargetInstance;

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, targetInstance);

            // Assert
            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out outputTargetInstance));
            Assert.AreEqual(targetInstance, outputTargetInstance);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_SameSourceInstanceButDifferentTypes_AddsToCache()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance = new object();
            var targetInstance1 = new object();
            var targetInstance2 = new object();
            object outputTargetInstance;

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, targetInstance1);
            target.Add(TestClass2.T, TestClass1.T, sourceInstance, targetInstance2);

            // Assert
            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out outputTargetInstance));
            Assert.AreEqual(targetInstance1, outputTargetInstance);

            Assert.IsTrue(target.TryGet(TestClass2.T, TestClass1.T, sourceInstance, out outputTargetInstance));
            Assert.AreEqual(targetInstance2, outputTargetInstance);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_KeyExists_Throws()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance = new object();

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, new object());
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_SameHashCodeButUnequal_AddedToCache()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance1 = new EquatableClass(1, 1);
            var sourceInstance2 = new EquatableClass(2, 1);
            var targetInstance1 = new object();
            var targetInstance2 = new object();
            
            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance1, targetInstance1);
            target.Add(TestClass1.T, TestClass2.T, sourceInstance2, targetInstance2);

            // Assert
            object outputTargetInstance;
            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance1, out outputTargetInstance));
            Assert.AreEqual(targetInstance1, outputTargetInstance);

            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance2, out outputTargetInstance));
            Assert.AreEqual(targetInstance2, outputTargetInstance);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGet_KeyParametersAreNull_DoesNotThrow()
        {
            // Arrange
            var target = new TargetCache();
            object targetInstance;

            // Act
            target.TryGet(null, null, null, out targetInstance);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGet_KeyDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var target = new TargetCache();
            object targetInstance;

            // Act
            var result = target.TryGet(null, null, null, out targetInstance);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGet_KeyExists_ReturnsTrue()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance = new object();
            object targetInstance;

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, new object());
            var result = target.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out targetInstance);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGet_KeyExists_OutputsTarget()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance = new object();
            var expectedTargetInstance = new object();
            object targetInstance;

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance, expectedTargetInstance);
            target.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out targetInstance);

            // Assert
            Assert.AreEqual(expectedTargetInstance, targetInstance);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_SecondKeyIsEqualButNotReferenceEqual_AddsToCache()
        {
            // Arrange
            var target = new TargetCache();
            var sourceInstance1 = new EquatableClass(1, 1);
            var sourceInstance2 = new EquatableClass(1, 1);

            // Act
            target.Add(TestClass1.T, TestClass2.T, sourceInstance1, new object());
            target.Add(TestClass1.T, TestClass2.T, sourceInstance2, new object());

            object targetInstance;

            // Assert
            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance1, out targetInstance));
            Assert.IsTrue(target.TryGet(TestClass1.T, TestClass2.T, sourceInstance2, out targetInstance));
        }
    }
}