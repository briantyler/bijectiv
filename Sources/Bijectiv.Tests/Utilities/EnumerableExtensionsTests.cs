// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionsTests.cs" company="Bijectiv">
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
//   Defines the EnumerableExtensionsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1720
namespace Bijectiv.Tests.Utilities
{
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Tests.TestTools;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="EnumerableExtensions"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Concat_ThisParameterIsEmpty_Throws()
        {
            // Arrange

            // Act
            default(IEnumerable<int>).Concat();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Concat_SingleArgumentParameter_ArgumentAppendedToResult()
        {
            // Arrange

            // Act
            var result = Enumerable.Range(0, 3).Concat(3);

            // Assert
            Assert.IsTrue(Enumerable.Range(0, 4).SequenceEqual(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Concat_MultipleArgumentParameter_ArgumentsAppendedToResult()
        {
            // Arrange

            // Act
            var result = Enumerable.Range(0, 3).Concat(3, 4, 5);

            // Assert
            Assert.IsTrue(Enumerable.Range(0, 6).SequenceEqual(result));
        }
    }
}