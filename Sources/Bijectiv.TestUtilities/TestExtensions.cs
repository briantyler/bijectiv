// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestExtensions.cs" company="Bijectiv">
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
//   This class contains extension methods that are useful when testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class contains extension methods that are useful when testing.
    /// </summary>
    public static class TestExtensions
    {
        /// <summary>
        /// A method that does exactly nothing.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The type of <paramref name="instance"/>.
        /// </typeparam>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The original instance.
        /// </returns>
        public static TInstance Naught<TInstance>(this TInstance instance)
        {
            return instance;
        }

        /// <summary>
        /// Asserts that sequence <paramref name="expected"/> is equal to <paramref name="actual"/>, failing usefuly
        /// when the assert fails.
        /// </summary>
        /// <param name="expected">
        /// The expected sequence.
        /// </param>
        /// <param name="actual">
        /// The actual sequence.
        /// </param>
        /// <typeparam name="T">
        /// The type of items in a sequence.
        /// </typeparam>
        public static void AssertSequenceEqual<T>(this IEnumerable<T> expected, IEnumerable<T> actual)
        {
            if (ReferenceEquals(expected, actual))
            {
                return;
            }

            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            var expectedArray = expected as T[] ?? expected.ToArray();
            var actualArray = actual as T[] ?? actual.ToArray();

            Assert.AreEqual(
                expectedArray.Count(), 
                actualArray.Count(),
                "Sequence count mismatch: expected {0}, actual {1}", 
                expectedArray.Count(), 
                actualArray.Count());

            for (var index = 0; index < expectedArray.Count(); index++)
            {
                var expectedItem = expectedArray[index];
                var actualItem = actualArray[index];

                Assert.AreEqual(
                    expectedItem, 
                    actualItem, 
                    "Items at position {0} are not equal: expected {1}, actual {2}",
                    index,
                    expectedItem,
                    actualItem);
            }
        }
    }
}