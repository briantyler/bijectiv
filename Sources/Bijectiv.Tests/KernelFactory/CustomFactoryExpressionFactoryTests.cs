﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomFactoryExpressionFactoryTests.cs" company="Bijectiv">
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
//   Defines the CustomFactoryExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CustomFactoryExpressionFactory"/> class.
    /// </summary>
    [TestClass]
    public class CustomFactoryExpressionFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CustomFactoryExpressionFactory().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanCreateExpression_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.CanCreateExpression(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanCreateExpression_FirstUnprocessedFactoryFragmentIsDefaultFactoryFragment_ReturnsTrue()
        {
            // Arrange
            var testTarget = CreateTestTarget();
            var scaffold = CreateScaffold();

            var factoryFragment1 = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);
            var factoryFragment2 = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment1);
            scaffold.CandidateFragments.Add(CreateFragment());
            scaffold.CandidateFragments.Add(factoryFragment2);

            scaffold.ProcessedFragments.Add(factoryFragment1);

            // Act
            var result = testTarget.CanCreateExpression(scaffold);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanCreateExpression_FirstUnprocessedFactoryFragmentIsNotDefaultFactoryActivateFragment_ReturnsFalse()
        {
            // Arrange
            var testTarget = CreateTestTarget();
            var scaffold = CreateScaffold();

            var factoryFragment1 = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);
            var factoryFragment2 = Stub.Fragment<TestClass1, TestClass2>(false, LegendaryFragments.Factory);

            scaffold.CandidateFragments.Add(factoryFragment1);
            scaffold.CandidateFragments.Add(CreateFragment());
            scaffold.CandidateFragments.Add(factoryFragment2);

            // Act
            var result = testTarget.CanCreateExpression(scaffold);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateExpression_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.CreateExpression(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_ValidParameters_ExpressionCreatesFromStaticMethod()
        {
            // Arrange
            var expected = new TestClass2();
            var injectionContextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            injectionContextMock.Setup(_ => _.Resolve(TestClass2.T)).Returns(expected);

            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = 
                p => p.Source.Id == "Dummy" ? (TestClass2)p.Context.Resolve(TestClass2.T) : new TestClass2();

            var scaffold = CreateScaffold();
            scaffold.Source = Expression.Constant(new TestClass1 { Id = "Dummy" });
            scaffold.CandidateFragments.Add(new CustomFactoryFragment(TestClass1.T, TestClass2.T, factory));

            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.CreateExpression(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IInjectionContext, TestClass2>>(result, (ParameterExpression)scaffold.InjectionContext)
                .Compile();

            Assert.AreEqual(expected, @delegate(injectionContextMock.Object));
            injectionContextMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_ValidParameters_ExpressionCreatesFromInstanceMethod()
        {
            // Arrange
            var wrapper = new Wrapper(new TestClass2());

            var scaffold = CreateScaffold();
            scaffold.Source = Expression.Constant(new TestClass1());
            scaffold.CandidateFragments.Add(
                new CustomFactoryFragment(
                    TestClass1.T, 
                    TestClass2.T, 
                    (Func<CustomFactoryParameters<TestClass1>, TestClass2>)wrapper.Create));

            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.CreateExpression(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IInjectionContext, TestClass2>>(result, (ParameterExpression)scaffold.InjectionContext)
                .Compile();

            Assert.AreEqual(wrapper.Expected, @delegate(Stub.Create<IInjectionContext>()));
        }

        private static CustomFactoryFragment CreateFragment()
        {
            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = p => new TestClass2();
            return new CustomFactoryFragment(TestClass1.T, TestClass2.T, factory);
        }

        private static CustomFactoryExpressionFactory CreateTestTarget()
        {
            return new CustomFactoryExpressionFactory();
        }

        private static InjectionScaffold CreateScaffold()
        {
            return new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(IInjectionContext)));
        }

        private class Wrapper
        {
            private readonly TestClass2 expected;

            public Wrapper(TestClass2 expected)
            {
                this.expected = expected;
            }

            public TestClass2 Expected
            {
                get { return this.expected; }
            }

            public TestClass2 Create(CustomFactoryParameters<TestClass1> parameters)
            {
                return this.Expected;
            }
        }
    }
}