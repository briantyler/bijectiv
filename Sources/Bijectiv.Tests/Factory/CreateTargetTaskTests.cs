// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTargetTaskTests.cs" company="Bijectiv">
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
//   Defines the CreateTargetTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CreateTargetTask"/> class.
    /// </summary>
    [TestClass]
    public class CreateTargetTaskTests
    {
        private static readonly TestClass1 Output = new TestClass1();

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DetailParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new CreateTargetTask(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CreateTargetTask(Stub.Create<ISelectiveExpressionFactory>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DetailParameterIsAssignedToDetailProperty()
        {
            // Arrange
            var expressionFactory = Stub.Create<ISelectiveExpressionFactory>();

            // Act
            var target = new CreateTargetTask(expressionFactory);

            // Assert
            Assert.AreEqual(expressionFactory, target.ExpressionFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new CreateTargetTask(Stub.Create<ISelectiveExpressionFactory>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DetailCanCreateExpressionIsFalse_NoExpressionsCreated()
        {
            // Arrange
            var expressionFactoryMock = new Mock<ISelectiveExpressionFactory>(MockBehavior.Strict);
            expressionFactoryMock
                .Setup(_ => _.CanCreateExpression(It.IsAny<InjectionScaffold>()))
                .Returns(false);

            var scaffold = CreateScaffold();
            var target = new CreateTargetTask(expressionFactoryMock.Object);

            // Act
            target.Execute(scaffold);

            // Assert
            expressionFactoryMock.VerifyAll();
            Assert.IsFalse(scaffold.Expressions.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DetailCanCreateExpressionIsTrue_ExpressionsCreated()
        {
            // Arrange
            var expressionFactoryMock = new Mock<ISelectiveExpressionFactory>(MockBehavior.Strict);
            var scaffold = CreateScaffold();
            var target = new CreateTargetTask(expressionFactoryMock.Object);

            SetupExecuteTest(expressionFactoryMock, scaffold);

            // Act
            target.Execute(scaffold);

            // Assert
            expressionFactoryMock.VerifyAll();
            Assert.AreEqual(2, scaffold.Expressions.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DetailCanCreateExpressionIsTrue_FirstExpressionAssignsTarget()
        {
            // Arrange
            var expressionFactoryMock = new Mock<ISelectiveExpressionFactory>(MockBehavior.Strict);
            var scaffold = CreateScaffold();
            var target = new CreateTargetTask(expressionFactoryMock.Object);

            SetupExecuteTest(expressionFactoryMock, scaffold);

            // Act
            target.Execute(scaffold);

            // Assert
            var variables = new[] { (ParameterExpression)scaffold.Target };
            var @delegate = Expression.Lambda<Func<TestClass1>>(
                Expression.Block(variables, scaffold.Expressions.ElementAt(0), scaffold.Target))
                .Compile();

            Assert.AreEqual(Output, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DetailCanCreateExpressionIsTrue_SecondExpressionAssignsTargetAsObject()
        {
            // Arrange
            var expressionFactoryMock = new Mock<ISelectiveExpressionFactory>(MockBehavior.Strict);
            var scaffold = CreateScaffold();
            var target = new CreateTargetTask(expressionFactoryMock.Object);

            SetupExecuteTest(expressionFactoryMock, scaffold);

            // Act
            target.Execute(scaffold);

            // Assert
            var variables = new[]
            {
                (ParameterExpression)scaffold.Target, 
                (ParameterExpression)scaffold.TargetAsObject
            };
            var @delegate = Expression.Lambda<Func<object>>(
                Expression.Block(variables, scaffold.Expressions.Concat(scaffold.TargetAsObject)))
                .Compile();

            Assert.AreEqual(Output, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DetailCanCreateExpressionIsTrue_FactoryFragmentsProcessed()
        {
            // Arrange
            var expressionFactoryMock = new Mock<ISelectiveExpressionFactory>(MockBehavior.Strict);
            var scaffold = CreateScaffold();
            var target = new CreateTargetTask(expressionFactoryMock.Object);

            var factoryFragment = Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.Factory);
            scaffold.CandidateFragments.Add(factoryFragment);
            scaffold.CandidateFragments.Add(Stub.Fragment<TestClass1, TestClass2>());

            SetupExecuteTest(expressionFactoryMock, scaffold);

            // Act
            target.Execute(scaffold);

            // Assert
            expressionFactoryMock.VerifyAll();
            Assert.AreEqual(1, scaffold.ProcessedFragments.Count());
            Assert.IsTrue(scaffold.ProcessedFragments.Contains(factoryFragment));
        }

        private static void SetupExecuteTest(Mock<ISelectiveExpressionFactory> expressionFactoryMock, InjectionScaffold scaffold)
        {
            expressionFactoryMock
                .Setup(_ => _.CanCreateExpression(It.IsAny<InjectionScaffold>()))
                .Returns(true);

            expressionFactoryMock
                .Setup(_ => _.CreateExpression(It.IsAny<InjectionScaffold>()))
                .Returns(Expression.Constant(Output));

            scaffold.Target = Expression.Parameter(TestClass1.T);
            scaffold.TargetAsObject = Expression.Parameter(typeof(object));
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