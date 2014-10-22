// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeInjectionParametersTaskTests.cs" company="Bijectiv">
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
//   Defines the InitializeInjectionParametersTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InitializeInjectionParametersTask"/> class.
    /// </summary>
    [TestClass]
    public class InitializeInjectionParametersTaskTests
    {
        private readonly TestClass1 sourceInstance = new TestClass1();

        private readonly TestClass2 targetInstance = new TestClass2();

        private readonly IInjectionContext context = Stub.Create<IInjectionContext>();

        private readonly object hint = new object();

        private readonly List<Expression> expressions = new List<Expression>();

        private ParameterExpression parameter;

        private Mock<InjectionScaffold> scaffoldMock;

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InitializeInjectionParametersTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_InitializesInjectionParametersVariable()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            this.scaffoldMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_CreatesInjectionParametersVariable()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);
            
            // Assert
            Assert.IsNotNull(this.RetrieveParameters());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsParametersSource()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(this.sourceInstance, this.RetrieveParameters().Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsParametersTarget()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(this.targetInstance, this.RetrieveParameters().Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsParametersContext()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(this.context, this.RetrieveParameters().Context);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsParametersHint()
        {
            // Arrange
            var scaffold = this.CreateScaffold();
            var testTarget = new InitializeInjectionParametersTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(this.hint, this.RetrieveParameters().Hint);
        }

        private InjectionScaffold CreateScaffold()
        {
            this.scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            this.scaffoldMock.SetupGet(_ => _.Definition).Returns(new InjectionDefinition(TestClass1.T, TestClass2.T));

            this.scaffoldMock.SetupGet(_ => _.Source).Returns(Expression.Constant(this.sourceInstance));
            this.scaffoldMock.SetupGet(_ => _.Target).Returns(Expression.Constant(this.targetInstance));
            this.scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(this.context));
            this.scaffoldMock.SetupGet(_ => _.Hint).Returns(Expression.Constant(this.hint));

            this.parameter = Expression.Parameter(typeof(InjectionParameters<TestClass1, TestClass2>));
            this.scaffoldMock
                .Setup(_ => _.GetVariable("injectionParameters", typeof(InjectionParameters<TestClass1, TestClass2>)))
                .Returns(this.parameter);
            this.scaffoldMock.SetupGet(_ => _.Expressions).Returns(this.expressions);

            return this.scaffoldMock.Object;
        }

        private IInjectionParameters<TestClass1, TestClass2> RetrieveParameters()
        {
            this.expressions.Add(this.parameter);
            return Expression
                .Lambda<Func<IInjectionParameters<TestClass1, TestClass2>>>(
                    Expression.Block(new[] { this.parameter }, this.expressions))
                .Compile()();
        }
    }
}