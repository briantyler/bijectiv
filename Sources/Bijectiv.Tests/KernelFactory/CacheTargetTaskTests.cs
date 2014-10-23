// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTargetTaskTests.cs" company="Bijectiv">
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
//   Defines the CacheTargetTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CacheTargetTask"/>
    /// </summary>
    [TestClass]
    public class CacheTargetTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CacheTargetTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new CacheTargetTask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_ExpressionAddsToInjectionContextCache()
        {
            // Arrange
            var scaffold = CreateScaffold();
            scaffold.TargetAsObject = Expression.Parameter(typeof(object));

            var testTarget = new CacheTargetTask();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            var @delegate =
                Expression.Lambda<Action<IInjectionContext, object, object>>(
                    scaffold.Expressions.Single(),
                    (ParameterExpression)scaffold.InjectionContext,
                    (ParameterExpression)scaffold.SourceAsObject,
                    (ParameterExpression)scaffold.TargetAsObject).Compile();

            var repository = new MockRepository(MockBehavior.Strict);
            var contextMock = repository.Create<IInjectionContext>();
            var cacheMock = repository.Create<ITargetCache>();

            contextMock.SetupGet(_ => _.TargetCache).Returns(cacheMock.Object);

            var sourceInstance = new TestClass1();
            var target = new TestClass2();
            cacheMock.Setup(_ => _.Add(TestClass1.T, TestClass2.T, sourceInstance, target));
            
            @delegate(contextMock.Object, sourceInstance, target);
            repository.VerifyAll();
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