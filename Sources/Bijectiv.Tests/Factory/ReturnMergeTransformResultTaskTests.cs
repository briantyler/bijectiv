// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMergeTransformResultTaskTests.cs" company="Bijectiv">
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
//   Defines the ReturnMergeTransformResultTaskTests type.
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

    /// <summary>
    /// This class tests the <see cref="ReturnMergeTransformResultTask"/> class.
    /// </summary>
    [TestClass]
    public class ReturnMergeTransformResultTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ReturnMergeTransformResultTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
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
        public void Execute_MergeTransformResult_IsReturned()
        {
            // Arrange
            var targetAsObject = new object();
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Constant(targetAsObject);

            var target = CreateTarget();

            // Act
            target.Execute(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IMergeTransformResult>>(Expression.Block(scaffold.Expressions))
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

            var target = CreateTarget();

            // Act
            scaffold.Expressions.Add(Expression.Assign(postMergeAction, Expression.Constant(PostMergeAction.Replace)));
            target.Execute(scaffold);

            // Assert
            var @delegate = Expression
                .Lambda<Func<IMergeTransformResult>>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile();

            var result = @delegate();
            Assert.AreEqual(targetAsObject, result.Target);
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }

        private static ReturnMergeTransformResultTask CreateTarget()
        {
            return new ReturnMergeTransformResultTask();
        }

        private static TransformScaffold CreateScaffold()
        {
            return new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(ITransformContext)));
        }
    }
}