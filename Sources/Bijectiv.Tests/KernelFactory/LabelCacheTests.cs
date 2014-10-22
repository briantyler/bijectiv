// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LabelCacheTests.cs" company="Bijectiv">
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
//   Defines the LabelCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;

    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="LabelCache"/> class.
    /// </summary>
    [TestClass]
    public class LabelCacheTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new LabelCache().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetLabel_ScopeParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new LabelCache();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.GetLabel(null, Guid.Empty);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLabel_ValidParameters_LabelIsCreated()
        {
            // Arrange
            var testTarget = new LabelCache();

            // Act
            var label = testTarget.GetLabel(new object(), new Guid("7BD5F011-E7F4-4EF5-9D97-B5A2C0480F4B"));

            // Assert
            Assert.IsNotNull(label);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLabel_LabelWasGotForIdenticalScopeAndCategory_OriginalLabelIsReturned()
        {
            // Arrange
            var testTarget = new LabelCache();
            var scope = new object();
            var category = new Guid("7BD5F011-E7F4-4EF5-9D97-B5A2C0480F4B");
            
            var expected = testTarget.GetLabel(scope, category);

            // Act
            var label = testTarget.GetLabel(scope, category);

            // Assert
            Assert.AreEqual(expected, label);
        }
    }
}