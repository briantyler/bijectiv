// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaceholderExpressionVisitorTests.cs" company="Bijectiv">
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
//   Defines the PlaceholderExpressionVisitorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="PlaceholderExpressionVisitor"/> class.
    /// </summary>
    [TestClass]
    public class PlaceholderExpressionVisitorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new PlaceholderExpressionVisitor("parameter", Expression.Constant(0)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_NameParameterIsNull_Throws()
        {
            // Arrange
            
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new PlaceholderExpressionVisitor(null, Expression.Constant(0)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ReplacementParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new PlaceholderExpressionVisitor("parameter", null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_NoParameter_DoesNothing()
        {
            // Arrange
            var constant = Expression.Constant(31);
            Expression<Func<int>> expression = () => Math.Sign(1);

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(1, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_NoNameParameter_DoesNothing()
        {
            // Arrange
            var constant = Expression.Constant(31);
            Expression<Func<int>> expression = () => Math.Sign(Placeholder.Of<int>());

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(0, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_SingleParameter_SubstitutesExpression()
        {
            // Arrange
            var constant = Expression.Constant(31);
            Expression<Func<int>> expression = () => Placeholder.Of<int>("parameter");

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(31, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_CapturedParameter_SubstitutesExpression()
        {
            // Arrange
            var constant = Expression.Constant(31);
            const string Parameter = "parameter";
            Expression<Func<int>> expression = () => Placeholder.Of<int>(Parameter);

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(31, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_CapturedParameterHasWrongName_DoesNotSubstituteExpression()
        {
            // Arrange
            var constant = Expression.Constant(31);
            const string Parameter = "x";
            Expression<Func<int>> expression = () => Placeholder.Of<int>(Parameter);

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreNotEqual(31, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_CapturedMemberParameter_SubstitutesExpression()
        {
            // Arrange
            var constant = Expression.Constant(31);
            var parameterSource = new TestClass1 { Id = "parameter" };
            Expression<Func<int>> expression = () => Placeholder.Of<int>(parameterSource.Id);

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(31, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_CapturedMemberParameterHasWrongName_DoesNotSubstituteExpression()
        {
            // Arrange
            var constant = Expression.Constant(31);
            var parameterSource = new TestClass1 { Id = "x" };
            Expression<Func<int>> expression = () => Placeholder.Of<int>(parameterSource.Id);

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreNotEqual(31, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_MultipleParameters_SubstitutesExpression()
        {
            // Arrange
            var c7 = Expression.Constant(7);
            var c5 = Expression.Constant(5);
            Expression<Func<int>> expression = () => Placeholder.Of<int>("p1") * Placeholder.Of<int>("p2");

            var target1 = new PlaceholderExpressionVisitor("p1", c7);
            var target2 = new PlaceholderExpressionVisitor("p2", c5);

            // Act
            expression = (Expression<Func<int>>)target1.Visit(expression);
            expression = (Expression<Func<int>>)target2.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(35, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Substitute_ComplexParameters_SubstitutesExpression()
        {
            // Arrange
            var c3 = Expression.Constant(3.0);
            var c2 = Expression.Constant(2.0);
            Expression<Func<int>> expression = 
                () => (int)(Placeholder.Of<double>("p1") +
                        Math.Pow(Placeholder.Of<double>("p1"), Placeholder.Of<double>("p2")));

            var target1 = new PlaceholderExpressionVisitor("p1", c3);
            var target2 = new PlaceholderExpressionVisitor("p2", c2);

            // Act
            expression = (Expression<Func<int>>)target1.Visit(expression);
            expression = (Expression<Func<int>>)target2.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(12, @delegate());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Substitute_DelegateParameter_Throws()
        {
            // Arrange
            var constant = Expression.Constant(31);

            Func<string> parameter = () => "parameter";
            Expression<Func<int>> expression = () => Placeholder.Of<int>(parameter());

            var testTarget = new PlaceholderExpressionVisitor("parameter", constant);

            // Act
            expression = (Expression<Func<int>>)testTarget.Visit(expression);

            // Assert
            var @delegate = expression.Compile();
            Assert.AreEqual(31, @delegate());
        }
    }
}