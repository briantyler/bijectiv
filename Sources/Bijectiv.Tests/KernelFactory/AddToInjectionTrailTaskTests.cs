// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddToInjectionTrailTaskTests.cs" company="Bijectiv">
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
//   Defines the AddToInjectionTrailTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AddToInjectionTrailTask"/> class.
    /// </summary>
    [TestClass]
    public class AddToInjectionTrailTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AddToInjectionTrailTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new AddToInjectionTrailTask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AddsItemToTrail()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            
            var scaffoldMock = repository.Create<InjectionScaffold>();

            var injection = Stub.Create<IInjection>();
            scaffoldMock.SetupGet(_ => _.Injection).Returns(Expression.Constant(injection));
            
            var sourceInstance = new object();
            scaffoldMock.SetupGet(_ => _.SourceAsObject).Returns(Expression.Constant(sourceInstance));
            
            var target = new object();
            scaffoldMock.SetupGet(_ => _.TargetAsObject).Returns(Expression.Constant(target));

            var contextMock = repository.Create<IInjectionContext>();
            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(contextMock.Object));
            
            var trailMock = repository.Create<IInjectionTrail>();
            contextMock.SetupGet(_ => _.InjectionTrail).Returns(trailMock.Object);
            trailMock
                .Setup(_ => _.Add(It.Is<InjectionTrailItem>(
                    x => x.Injection == injection && x.Source == sourceInstance && x.Target == target)))
                .Returns(true);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);
            
            var label = Expression.Label();
            scaffoldMock.Setup(_ => _.GetLabel(null, LegendaryLabels.End)).Returns(label);

            var testTarget = new AddToInjectionTrailTask();
            
            // Act
            testTarget.Execute(scaffoldMock.Object);

            // Assert
            expressions.Add(Expression.Label(label));

            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParametersItemNotInTrail_DoesNotJumpToEndLabel()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<InjectionScaffold>();

            scaffoldMock.SetupGet(_ => _.Injection).Returns(Expression.Constant(Stub.Create<IInjection>()));
            scaffoldMock.SetupGet(_ => _.SourceAsObject).Returns(Expression.Constant(new object()));
            scaffoldMock.SetupGet(_ => _.TargetAsObject).Returns(Expression.Constant(new object()));

            var contextMock = repository.Create<IInjectionContext>();
            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(contextMock.Object));

            var trailMock = repository.Create<IInjectionTrail>();
            contextMock.SetupGet(_ => _.InjectionTrail).Returns(trailMock.Object);
            trailMock
                .Setup(_ => _.Add(It.IsAny<InjectionTrailItem>()))
                .Returns(true);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var label = Expression.Label();
            scaffoldMock.Setup(_ => _.GetLabel(null, LegendaryLabels.End)).Returns(label);

            var testTarget = new AddToInjectionTrailTask();

            // Act
            testTarget.Execute(scaffoldMock.Object);

            // Assert
            var parameter = Expression.Parameter(typeof(bool[]));
            expressions.Add(
                Expression.Assign(
                    Expression.ArrayAccess(parameter, Expression.Constant(0)),
                    Expression.Constant(true)));

            expressions.Add(Expression.Label(label));

            var hit = new[] { false };
            Expression.Lambda<Action<bool[]>>(Expression.Block(expressions), parameter).Compile()(hit);
            repository.VerifyAll();

            Assert.IsTrue(hit[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParametersItemInTrail_JumpsToEndLabel()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<InjectionScaffold>();

            scaffoldMock.SetupGet(_ => _.Injection).Returns(Expression.Constant(Stub.Create<IInjection>()));
            scaffoldMock.SetupGet(_ => _.SourceAsObject).Returns(Expression.Constant(new object()));
            scaffoldMock.SetupGet(_ => _.TargetAsObject).Returns(Expression.Constant(new object()));

            var contextMock = repository.Create<IInjectionContext>();
            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(contextMock.Object));

            var trailMock = repository.Create<IInjectionTrail>();
            contextMock.SetupGet(_ => _.InjectionTrail).Returns(trailMock.Object);
            trailMock
                .Setup(_ => _.Add(It.IsAny<InjectionTrailItem>()))
                .Returns(false);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var label = Expression.Label();
            scaffoldMock.Setup(_ => _.GetLabel(null, LegendaryLabels.End)).Returns(label);

            var testTarget = new AddToInjectionTrailTask();

            // Act
            testTarget.Execute(scaffoldMock.Object);

            // Assert
            var parameter = Expression.Parameter(typeof(bool[]));
            expressions.Add(
                Expression.Assign(
                    Expression.ArrayAccess(parameter, Expression.Constant(0)),
                    Expression.Constant(true)));

            expressions.Add(Expression.Label(label));

            var hit = new[] { false };
            Expression.Lambda<Action<bool[]>>(Expression.Block(expressions), parameter).Compile()(hit);
            repository.VerifyAll();

            Assert.IsFalse(hit[0]);
        }
    }
}