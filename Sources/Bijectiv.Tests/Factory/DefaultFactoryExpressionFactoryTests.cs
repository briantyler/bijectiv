// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultFactoryExpressionFactoryTests.cs" company="Bijectiv">
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
//   Defines the DefaultFactoryExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="DefaultFactoryExpressionFactory"/> class.
    /// </summary>
    [TestClass]
    public class DefaultFactoryExpressionFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DefaultFactoryExpressionFactory().Naught();

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
        public void CanCreateExpression_FirstUnprocessedFactoryFragmentIsDefaultFactoryFragment_ReturnsTrue()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            var factoryFragment1 = Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.Factory);
            var factoryFragment2 = Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment1);
            scaffold.CandidateFragments.Add(new DefaultFactoryFragment(TestClass1.T, TestClass2.T));
            scaffold.CandidateFragments.Add(factoryFragment2);

            scaffold.ProcessedFragments.Add(factoryFragment1);

            // Act
            var result = target.CanCreateExpression(scaffold);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanCreateExpression_FirstUnprocessedFactoryFragmentIsNotDefaultFactoryActivateFragment_ReturnsFalse()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            var factoryFragment1 = Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.Factory);
            var factoryFragment2 = Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment1);
            scaffold.CandidateFragments.Add(new DefaultFactoryFragment(TestClass1.T, TestClass2.T));
            scaffold.CandidateFragments.Add(factoryFragment2);

            // Act
            var result = target.CanCreateExpression(scaffold);

            // Assert
            Assert.IsFalse(result);
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
        public void CreateExpression_ValidParameters_ExpressionCallsResolveOnInjectionContext()
        {
            // Arrange
            var expected = new TestClass2();
            var injectionContextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            injectionContextMock.Setup(_ => _.Resolve(TestClass2.T)).Returns(expected);

            var scaffold = CreateScaffold();
            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold);
            
            // Assert
            var @delegate = Expression.Lambda<Func<IInjectionContext, TestClass2>>(
                result, (ParameterExpression)scaffold.InjectionContext)
                .Compile();

            Assert.AreEqual(expected, @delegate(injectionContextMock.Object));
            injectionContextMock.VerifyAll();
        }

        private static DefaultFactoryExpressionFactory CreateTarget()
        {
            return new DefaultFactoryExpressionFactory();
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