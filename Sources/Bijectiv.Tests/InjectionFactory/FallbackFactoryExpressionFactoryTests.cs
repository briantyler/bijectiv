// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FallbackFactoryExpressionFactoryTests.cs" company="Bijectiv">
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
//   Defines the FallbackFactoryExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.InjectionFactory
{
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.InjectionFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="FallbackFactoryExpressionFactory"/> class.
    /// </summary>
    [TestClass]
    public class FallbackFactoryExpressionFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new FallbackFactoryExpressionFactory().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanCreateExpression_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CanCreateExpression(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanCreateExpression_ProcessedFragmentsContainsFactoryFragment_ReturnsFalse()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            var factoryFragment = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment);
            scaffold.ProcessedFragments.Add(factoryFragment);

            // Act
            var result = target.CanCreateExpression(scaffold);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanCreateExpression_ProcessedFragmentsDoesNotContainFactoryFragment_ReturnsTrue()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            var factoryFragment = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment);

            // Act
            var result = target.CanCreateExpression(scaffold);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateExpression_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateExpression(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_ValidParameters_ReturnsNewExpression()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            var result = target.CreateExpression(scaffold);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NewExpression));
            Assert.AreEqual(scaffold.Definition.Target, result.Type);
            Assert.IsFalse(((NewExpression)result).Arguments.Any());
        }

        private static FallbackFactoryExpressionFactory CreateTarget()
        {
            return new FallbackFactoryExpressionFactory();
        }

        private static InjectionScaffold CreateScaffold()
        {
            return new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(IInjectionContext)));
        }
    }
}