// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformContextTests.cs" company="Bijectiv">
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
//   Defines the TransformContextTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests
{
    using System.Globalization;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformContext"/> class.
    /// </summary>
    [TestClass]
    public class TransformContextTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformContext().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CulturePropertyIsInvariant()
        {
            // Arrange

            // Act
            var target = new TransformContext();

            // Assert
            Assert.AreEqual(CultureInfo.InvariantCulture, target.Culture);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void CreateInstance_DefaultParameters_ResolveDelegateThrows()
        {
            // Arrange

            // Act
            var target = new TransformContext();

            // Assert
            target.ResolveDelegate(typeof(object));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_CultureParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TransformContext(null, t => new object()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ResolveDelegateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TransformContext(CultureInfo.InvariantCulture, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TransformContext(CultureInfo.InvariantCulture, t => new object()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CultureParameter_IsAssignedToCultureProperty()
        {
            // Arrange
            var culture = CultureInfo.CreateSpecificCulture("en-GB");

            // Act
            var target = new TransformContext(culture, t => new object());

            // Assert
            Assert.AreEqual(culture, target.Culture);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TypeParameterIsNull_Throws()
        {
            // Arrange
            var target = new TransformContext(CultureInfo.InvariantCulture, t => new object());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_InvokesResolveDelegate()
        {
            // Arrange
            var expected = new object();
            var target = new TransformContext(CultureInfo.InvariantCulture, t => t == TestClass1.T ? expected : null);

            // Act
            var result = target.Resolve(TestClass1.T);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}