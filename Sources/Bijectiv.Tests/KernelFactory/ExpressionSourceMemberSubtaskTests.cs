﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSourceMemberSubtaskTests.cs" company="Bijectiv">
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
//   Defines the ExpressionSourceMemberSubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="ExpressionSourceMemberShard"/> class.
    /// </summary>
    [TestClass]
    public class ExpressionSourceMemberSubtaskTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ExpressionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, null, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ExpressionParameterHasLessThanOneArgument_Throws()
        {
            // Arrange
            var expression = Expression.Lambda(Expression.Constant(1));
            
            // Act
            new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ExpressionParameterHasMoreThanOneArgument_Throws()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1), 
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(TestClass1.T));

            // Act
            new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ExpressionParameterArgumentTypeIsNotAssignableFromSource_Throws()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(TestClass2.T));

            // Act
            new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ExpressionParameterReturnTypeIsVoid_Throws()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Empty(),
                Expression.Parameter(TestClass1.T));

            // Act
            new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ExpressionParameterIsEqualToSource_InstanceCreated()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(BaseTestClass1.T));

            // Act
            new ExpressionSourceMemberShard(BaseTestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ExpressionParameterIsAssignableFromSource_InstanceCreated()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(BaseTestClass1.T));

            // Act
            new ExpressionSourceMemberShard(DerivedTestClass1.T, TestClass2.T, Member, expression, true).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ExpressionParameterIsAssignedToExpressionProperty()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(TestClass1.T));

            // Act
            var testTarget = new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true);

            // Assert
            Assert.AreEqual(expression, testTarget.Expression);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ExpressionParameterArgumentTypeIsAssignedToParameterTypeProperty()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(BaseTestClass1.T));

            // Act
            var testTarget = new ExpressionSourceMemberShard(DerivedTestClass1.T, TestClass2.T, Member, expression, true);

            // Assert
            Assert.AreEqual(BaseTestClass1.T, testTarget.ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ShardCategoryPropertyIsSource()
        {
            // Arrange
            var expression = Expression.Lambda(
                Expression.Constant(1),
                Expression.Parameter(TestClass1.T));

            // Act
            var testTarget = new ExpressionSourceMemberShard(TestClass1.T, TestClass2.T, Member, expression, true);

            // Assert
            Assert.AreEqual(LegendaryShards.Source, testTarget.ShardCategory);
        }
    }
}