// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMergeResultTaskTests.cs" company="Bijectiv">
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
//   Defines the ReturnMergeResultTaskTests type.
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

    /// <summary>
    /// This class tests the <see cref="ReturnMergeResultTask"/> class.
    /// </summary>
    [TestClass]
    public class ReturnMergeResultTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ReturnMergeResultTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_MergeTransformResult_IsReturned()
        {
            // Arrange
            var targetAsObject = new object();
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Constant(targetAsObject);

            var testTarget = CreateTestTarget();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IMergeResult>>(Expression.Block(scaffold.Expressions))
                .Compile();

            var result = @delegate();
            Assert.AreEqual(targetAsObject, result.Target);
            Assert.AreEqual(PostMergeAction.None, result.Action);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_PostMergeActionVariable_IsReturned()
        {
            // Arrange
            var targetAsObject = new object();
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Constant(targetAsObject);
            var postMergeAction = Expression.Variable(typeof(PostMergeAction), "postMergeAction");
            scaffold.Variables.Add(postMergeAction);

            var testTarget = CreateTestTarget();

            // Act
            scaffold.Expressions.Add(Expression.Assign(postMergeAction, Expression.Constant(PostMergeAction.Replace)));
            testTarget.Execute(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IMergeResult>>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile();

            var result = @delegate();
            Assert.AreEqual(targetAsObject, result.Target);
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        private static ReturnMergeResultTask CreateTestTarget()
        {
            return new ReturnMergeResultTask();
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