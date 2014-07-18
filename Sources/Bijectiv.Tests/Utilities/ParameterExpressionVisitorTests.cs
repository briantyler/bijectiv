// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterExpressionVisitorTests.cs" company="Bijectiv">
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
//   Defines the ParameterExpressionVisitorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.TestUtilities;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="ParameterExpressionVisitor"/> class.
    /// </summary>
    [TestClass]
    public class ParameterExpressionVisitorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ParameterParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new ParameterExpressionVisitor(null, Stub.Create<Expression>()).Naught();

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
            new ParameterExpressionVisitor(Expression.Parameter(typeof(object)), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ParameterExpressionVisitor(Expression.Parameter(typeof(object)), Stub.Create<Expression>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Visit_SingleSubstitution_SubstitutesParameter()
        {
            // Arrange
            Expression<Func<bool, int, int>> expression = (b, i) => b ? i : 2;
            var parameter = expression.Parameters[1];
            var target = new ParameterExpressionVisitor(parameter, Expression.Constant(7));

            // Act
            var substituted = target.Visit(expression.Body);

            // Assert
            var @delegate = Expression.Lambda<Func<bool, int>>(substituted, expression.Parameters[0]).Compile();
            Assert.AreEqual(7, @delegate(true));
            Assert.AreEqual(2, @delegate(false));
        }
    }
}