// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberValueSourceTransformSubtaskTests.cs" company="Bijectiv">
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
//   Defines the MemberValueSourceTransformSubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="MemberValueSourceTransformSubtask"/> class.
    /// </summary>
    [TestClass]
    public class MemberValueSourceTransformSubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberValueSourceTransformSubtask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberValueSourceTransformSubtask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(null, Stub.Create<MemberFragment>(), Stub.Create<ValueSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberValueSourceTransformSubtask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(Stub.Create<InjectionScaffold>(), null, Stub.Create<ValueSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberValueSourceTransformSubtask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessShard_ValidParameters_AddsTransformMemberExpressionToScaffold()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<InjectionScaffold>();

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var contextMock = repository.Create<IInjectionContext>();
            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(contextMock.Object));

            var targetParameter = Expression.Parameter(typeof(AutoInjectionTestClass1));
            scaffoldMock.SetupGet(_ => _.Target).Returns(targetParameter);

            var storeMock = repository.Create<IInjectionStore>();
            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);

            var transformMock = repository.Create<ITransform>();
            storeMock
                .Setup(_ => _.Resolve<ITransform>(typeof(string), typeof(BaseTestClass1)))
                .Returns(transformMock.Object);
            var transformedValue = new DerivedTestClass1();
            transformMock.Setup(_ => _.Transform("bijectiv", contextMock.Object, null)).Returns(transformedValue);

            var fragmentMock = repository.Create<MemberFragment>();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            fragmentMock.SetupGet(_ => _.Member).Returns(member);

            var shardMock = repository.Create<ValueSourceMemberShard>();
            shardMock.SetupGet(_ => _.Value).Returns("bijectiv");

            var target = new MemberValueSourceTransformSubtask();

            // Act
            target.ProcessShard(scaffoldMock.Object, fragmentMock.Object, shardMock.Object);

            // Assert
            var @delegate = Expression
                .Lambda<Action<AutoInjectionTestClass1>>(Expression.Block(expressions), targetParameter)
                .Compile();

            var targetInstance = new AutoInjectionTestClass1();
            @delegate(targetInstance);

            repository.VerifyAll();
            
            Assert.AreEqual(transformedValue, targetInstance.PropertyBase);
        }
    }
}