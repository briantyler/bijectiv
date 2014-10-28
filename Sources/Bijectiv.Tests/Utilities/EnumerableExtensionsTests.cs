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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bijectiv.TestUtilities;
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
            // ReSharper disable once AssignNullToNotNullAttribute
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

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ForEach_ThisParameterIsEmpty_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(IEnumerable<int>).ForEach(item => item.Naught());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ForEach_ActionParameterIsEmpty_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new Collection<int>().ForEach(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ForEach_ValidParameters_InvokesActionOnEachItem()
        {
            // Arrange
            var output = new List<int>();

            // Act
            Enumerable.Range(0, 3).ForEach(output.Add);

            // Assert
            Enumerable.Range(0, 3).AssertSequenceEqual(output);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Collect_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            EnumerableExtensions.Collect(null, (int x) => x);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Collect_KeySelectorParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new object[0].Collect<object, int>(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Collect_ComparerParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new object[0].Collect(x => x, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Collect_NoComparerParameter_CollectsWithDefaultComparer()
        {
            // Arrange
            var source = new[] { 1, 2, 1 };

            // Act
            var result = source.Collect(candidate => candidate).ToArray();

            // Assert
            Assert.AreEqual(source.Length, result.Length);
            Assert.AreEqual(source.Count(candidate => candidate == 1), result.Count(candidate => candidate.Key == 1));
            Assert.AreEqual(source.Count(candidate => candidate == 2), result.Count(candidate => candidate.Key == 2));
            result
                .Where(candidate => candidate.Key == 1)
                .ForEach(item => item.AssertSequenceEqual(source.Where(candidate => candidate == 1)));
            result
                .Where(candidate => candidate.Key == 2)
                .ForEach(item => item.AssertSequenceEqual(source.Where(candidate => candidate == 2)));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Collect_ComparerParameterSupplied_CollectsWithComparer()
        {
            // Arrange
            var source = new[] { 1, 2, 1 };

            // Act
            var result = source.Collect(candidate => candidate, new SocialistComparer()).ToArray();

            // Assert
            Assert.AreEqual(source.Length, result.Length);
            Assert.AreEqual(source.Count(candidate => candidate == 1), result.Count(candidate => candidate.Key == 1));
            Assert.AreEqual(source.Count(candidate => candidate == 2), result.Count(candidate => candidate.Key == 2));
            result.ForEach(item => item.AssertSequenceEqual(source));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Equivalent_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            EnumerableExtensions.Equivalent(null, (int x) => x);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Equivalent_KeySelectorParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new object[0].Equivalent<object, int>(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Equivalent_ComparerParameterParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new object[0].Equivalent(x => x, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equivalent_NoComparerParameter_DeterminesEquivalenceWithDefaultComparerPositive()
        {
            // Arrange

            // Act
            var result = new[] { 1, 1, 1 }.Equivalent(x => x);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equivalent_NoComparerParameter_DeterminesEquivalenceWithDefaultComparerFalse()
        {
            // Arrange

            // Act
            var result = new[] { 1, 2, 1 }.Equivalent(x => x);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equivalent_ComparerParameterSupplied_DeterminesEquivalenceWithComparerPositive()
        {
            // Arrange

            // Act
            var result = new[] { 1, 2, 1 }.Equivalent(x => x, new SocialistComparer());

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Equivalent_ComparerParameterSupplied_DeterminesEquivalenceWithComparerFalse()
        {
            // Arrange

            // Act
            var result = new[] { 1, 1, 1 }.Equivalent(x => x, new EgotistComparer());

            // Assert
            Assert.IsFalse(result);
        }

        private class SocialistComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }

            public int GetHashCode(int obj)
            {
                return 0;
            }
        }

        private class EgotistComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return false;
            }

            public int GetHashCode(int obj)
            {
                return new Random().Next();
            }
        }
    }
}