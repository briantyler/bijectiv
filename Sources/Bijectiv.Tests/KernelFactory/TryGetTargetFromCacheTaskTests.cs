// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryGetTargetFromCacheTaskTests.cs" company="Bijectiv">
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
//   Defines the TryGetTargetFromCacheTaskTests type.
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
    /// This class tests the <see cref="TryGetTargetFromCacheTask"/> class.
    /// </summary>
    [TestClass]
    public class TryGetTargetFromCacheTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TryGetTargetFromCacheTask().Naught();

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
        public void Execute_ValidParameters_TryGetsTargetFromCacheAndExitsOnSuccess()
        {
            // Arrange
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Variable(typeof(object));

            var target = CreateTarget();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(new object())));
            new ReturnTargetAsObjectTask().Execute(scaffold);

            var @delegate = Expression
                .Lambda<Func<IInjectionContext, object, object>>(
                    Expression.Block(
                        new[] { (ParameterExpression)scaffold.TargetAsObject },
                        scaffold.Expressions),
                    (ParameterExpression)scaffold.InjectionContext,
                    (ParameterExpression)scaffold.SourceAsObject)
                .Compile();

            var repository = new MockRepository(MockBehavior.Strict);
            var contextMock = repository.Create<IInjectionContext>();
            var cacheMock = repository.Create<ITargetCache>();

            contextMock.SetupGet(_ => _.TargetCache).Returns(cacheMock.Object);

            var sourceInstance = new TestClass1();
            object targetInstance = new TestClass2();

            cacheMock
                .Setup(_ => _.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out targetInstance))
                .Returns(true);

            Assert.AreEqual(targetInstance, @delegate(contextMock.Object, sourceInstance));
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_TryGetsTargetFromCacheAndContinuesOnFailure()
        {
            // Arrange
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Variable(typeof(object));

            var target = CreateTarget();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(new object())));
            new ReturnTargetAsObjectTask().Execute(scaffold);

            var @delegate = Expression
                .Lambda<Func<IInjectionContext, object, object>>(
                    Expression.Block(
                        new[] { (ParameterExpression)scaffold.TargetAsObject },
                        scaffold.Expressions),
                    (ParameterExpression)scaffold.InjectionContext,
                    (ParameterExpression)scaffold.SourceAsObject)
                .Compile();

            var repository = new MockRepository(MockBehavior.Strict);
            var contextMock = repository.Create<IInjectionContext>();
            var cacheMock = repository.Create<ITargetCache>();

            contextMock.SetupGet(_ => _.TargetCache).Returns(cacheMock.Object);

            var sourceInstance = new TestClass1();
            object targetInstance = new TestClass2();

            cacheMock
                .Setup(_ => _.TryGet(TestClass1.T, TestClass2.T, sourceInstance, out targetInstance))
                .Returns(false);

            Assert.AreNotEqual(targetInstance, @delegate(contextMock.Object, sourceInstance));
            repository.VerifyAll();
        }

        private static TryGetTargetFromCacheTask CreateTarget()
        {
            return new TryGetTargetFromCacheTask();
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