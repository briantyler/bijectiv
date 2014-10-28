// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExactInjectionResolutionStrategyTests.cs" company="Bijectiv">
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
//   Defines the ExactInjectionResolutionStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ExactInjectionResolutionStrategy"/> class.
    /// </summary>
    [TestClass]
    public class ExactInjectionResolutionStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ExactInjectionResolutionStrategy().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose(null, TestClass1.T, new IInjection[0]);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose(TestClass1.T, null, new IInjection[0]);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_CandidatesParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose<IInjection>(TestClass1.T, TestClass2.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesIsEmpty_ChoosesNull()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();

            // Act
            var result = testTarget.Choose(TestClass1.T, TestClass2.T, Ix());

            // Assert
            Assert.IsNull(result, Print(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsNoMatch_ChoosesNull()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();

            // Act
            var result = testTarget.Choose(
                TestClass1.T, 
                TestClass2.T,
                Ix(I<TestClass1, object>(), I<object, TestClass2>(), I<object, object>()));

            // Assert
            Assert.IsNull(result, Print(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsSingleExactMatch_ChoosesExact()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();
            var injection = I<TestClass1, TestClass2>();

            // Act
            var result = testTarget.Choose(TestClass1.T, TestClass2.T, Ix(injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMultipleExactMatches_ChoosesLastExact()
        {
            // Arrange
            var testTarget = new ExactInjectionResolutionStrategy();
            var injection = I<TestClass1, TestClass2>();

            // Act
            var result = testTarget.Choose(
                TestClass1.T,
                TestClass2.T, 
                Ix(I<TestClass1, TestClass2>(), I<TestClass1, TestClass2>(), injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        private static IInjection I<TSource, TTarget>()
        {
            var injectionMock = new Mock<IInjection>(MockBehavior.Strict);

            injectionMock.SetupGet(_ => _.Source).Returns(typeof(TSource));
            injectionMock.SetupGet(_ => _.Target).Returns(typeof(TTarget));

            return injectionMock.Object;
        }

        private static IEnumerable<IInjection> Ix(params IInjection[] injections)
        {
            return injections;
        }

        private static string Print(params IInjection[] injections)
        {
            return string.Join(
                ", ",
                injections
                    .Where(candidate => candidate != null)
                    .Select(item => string.Format("{0} --> {1}", item.Source, item.Target)));
        }
    }
}