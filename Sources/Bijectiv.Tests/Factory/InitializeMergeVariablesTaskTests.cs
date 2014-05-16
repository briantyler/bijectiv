// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeMergeVariablesTaskTests.cs" company="Bijectiv">
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
//   Defines the InitializeMergeVariablesTaskTests type.
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InitializeMergeVariablesTask"/> class.
    /// </summary>
    [TestClass]
    public class InitializeMergeVariablesTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InitializeTransformVariablesTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_ExpectedNumberOfVariablesAddedToScaffold()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            Assert.AreEqual(2, scaffold.Variables.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AddsSourceVariableToScaffoldVariablesProperty()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            var variable = scaffold.Variables.SingleOrDefault(candidate => candidate.Name == "source");
            Assert.IsNotNull(variable);
            Assert.IsInstanceOfType(variable, typeof(ParameterExpression));
            Assert.AreEqual(TestClass1.T, variable.Type);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsSourceVariableToScaffoldSourceProperty()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            var variable = scaffold.Variables.SingleOrDefault(candidate => candidate.Name == "source");
            Assert.AreEqual(variable, scaffold.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AddsTargetVariableToScaffoldVariablesProperty()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            var variable = scaffold.Variables.SingleOrDefault(candidate => candidate.Name == "target");
            Assert.IsNotNull(variable);
            Assert.IsInstanceOfType(variable, typeof(ParameterExpression));
            Assert.AreEqual(TestClass2.T, variable.Type);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AssignsTargetVariableToScaffoldTargetProperty()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            var variable = scaffold.Variables.SingleOrDefault(candidate => candidate.Name == "target");
            Assert.AreEqual(variable, scaffold.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_ExpectedNumberOfExpressionsAddedToScaffold()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();

            // Act
            target.Execute(scaffold);

            // Assert
            Assert.AreEqual(2, scaffold.Expressions.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AddedExpressionAssignsSourceAsObjectToSource()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();
            object sourceInstance = new TestClass1();

            // Act
            target.Execute(scaffold);

            // Assert
            var body = Expression.Block(
                scaffold.Variables, 
                scaffold.Expressions.Take(1).Concat(new[] { scaffold.Source }));
            var result = Expression.Lambda<Func<object, TestClass1>>(
                    body,
                    (ParameterExpression)scaffold.SourceAsObject)
                .Compile()(sourceInstance);

            Assert.AreEqual(sourceInstance, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AddedExpressionAssignsTargetAsObjectToTarget()
        {
            // Arrange
            var target = CreateTarget();
            var scaffold = CreateScaffold();
            object targetInstance = new TestClass2();

            // Act
            target.Execute(scaffold);

            // Assert
            var body = Expression.Block(
               scaffold.Variables,
               scaffold.Expressions.Skip(1).Concat(new[] { scaffold.Target }));
            var result = Expression.Lambda<Func<object, TestClass2>>(
                    body,
                    (ParameterExpression)scaffold.TargetAsObject)
                .Compile()(targetInstance);

            Assert.AreEqual(targetInstance, result);
        }

        private static InitializeMergeVariablesTask CreateTarget()
        {
            return new InitializeMergeVariablesTask();
        }

        private static InjectionScaffold CreateScaffold()
        {
            return new InjectionScaffold(
                Stub.Create<IInjectionDefinitionRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(IInjectionContext)))
                {
                    TargetAsObject = Expression.Parameter(typeof(object))
                };
        }
    }
}