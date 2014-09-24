// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberAssignSubtaskTests.cs" company="Bijectiv">
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
//   Defines the SourceMemberAssignSubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="SourceMemberAssignSubtask{TShard}"/> class.
    /// </summary>
    [TestClass]
    public class SourceMemberAssignSubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ProtectedConstructorDefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            Stub.Create<SourceMemberAssignSubtask<SourceMemberShard>>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceExpressionFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new SourceMemberAssignSubtask<SourceMemberShard>(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new SourceMemberAssignSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceExpressionFactoryParameter_IsAssignedToSourceExpressionFactoryProperty()
        {
            // Arrange
            var factory = Stub.Create<ISourceExpressionFactory<SourceMemberShard>>();

            // Act
            var target = new SourceMemberAssignSubtask<SourceMemberShard>(factory);

            // Assert
            Assert.AreEqual(factory, target.SourceExpressionFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanProcess_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = Stub.Create<SourceMemberAssignSubtask<SourceMemberShard>>();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CanProcess(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanProcess_ShardParameterCanInject_ReturnsFalse()
        {
            // Arrange
            var target = Stub.Create<SourceMemberAssignSubtask<SourceMemberShard>>();
            var shardMock = new Mock<SourceMemberShard>(MockBehavior.Strict);
            shardMock.SetupGet(_ => _.Inject).Returns(true);

            // Act
            var result = target.CanProcess(shardMock.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanProcess_ShardParameterCannotInject_ReturnsTrue()
        {
            // Arrange
            var target = Stub.Create<SourceMemberAssignSubtask<SourceMemberShard>>();
            var shardMock = new Mock<SourceMemberShard>(MockBehavior.Strict);
            shardMock.SetupGet(_ => _.Inject).Returns(false);

            // Act
            var result = target.CanProcess(shardMock.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new SourceMemberAssignSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(null, Stub.Create<MemberFragment>(), Stub.Create<SourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new SourceMemberAssignSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(Stub.Create<InjectionScaffold>(), null, Stub.Create<SourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new SourceMemberAssignSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessShard_SourceExpressionFactoryResult_IsAssignedToFragmentMember()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            
            var shard = Stub.Create<SourceMemberShard>();
            var fragmentMock = repository.Create<MemberFragment>();
            var scaffoldMock = repository.Create<InjectionScaffold>();

            var factoryMock = repository.Create<ISourceExpressionFactory<SourceMemberShard>>();
            factoryMock
                .Setup(_ => _.Create(scaffoldMock.Object, fragmentMock.Object, shard))
                .Returns(Expression.Constant("123"));

            var targetInstance = new TestClass1();
            scaffoldMock.SetupGet(_ => _.Target).Returns(Expression.Constant(targetInstance));

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            fragmentMock.SetupGet(_ => _.Member).Returns(Reflect<TestClass1>.FieldOrProperty(_ => _.Id));

            var target = new SourceMemberAssignSubtask<SourceMemberShard>(factoryMock.Object);

            // Act
            target.ProcessShard(scaffoldMock.Object, fragmentMock.Object, shard);

            // Assert
            Expression.Lambda<Action>(expressions.Single()).Compile()();

            Assert.AreEqual("123", targetInstance.Id);
        }
    }
}