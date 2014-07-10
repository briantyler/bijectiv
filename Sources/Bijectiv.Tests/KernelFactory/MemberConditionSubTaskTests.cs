// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberConditionSubtaskTests.cs" company="Bijectiv">
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
//   Defines the MemberConditionSubtaskTests type.
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
    /// This class tests the <see cref="MemberConditionSubtask"/> class.
    /// </summary>
    [TestClass]
    public class MemberConditionSubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberConditionSubtask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberConditionSubtask();

            // Act
            target.Execute(null, Stub.Create<MemberFragment>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberConditionSubtask();

            // Act
            target.Execute(Stub.Create<InjectionScaffold>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_PredicateConditionMemberShards_AreAddedToProcessedShardsCollection()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var fragmentMock = repository.Create<MemberFragment>();
            var unprocessed = new List<MemberShard>
            {
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
            };
            var processed = new HashSet<MemberShard>();
            fragmentMock.SetupGet(_ => _.UnprocessedShards).Returns(unprocessed);
            fragmentMock.SetupGet(_ => _.ProcessedShards).Returns(processed);
            
            var targetMock = repository.Create<MemberConditionSubtask>(MockBehavior.Loose);
            
            // Act
            targetMock.Object.Execute(Stub.Create<InjectionScaffold>(), fragmentMock.Object);

            // Assert
            new[] { unprocessed[1], unprocessed[3], unprocessed[5] }.AssertSetEqual(processed);
        }

        public void Execute_FirstPredicateConditionMemberShard_IsProcessed()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var fragmentMock = repository.Create<MemberFragment>();
            var shard = Stub.Create<PredicateConditionMemberShard>();
            var unprocessed = new List<MemberShard>
            {
                Stub.Create<MemberShard>(),
                shard,
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
            };
            var processed = new HashSet<MemberShard>();
            fragmentMock.SetupGet(_ => _.UnprocessedShards).Returns(unprocessed);
            fragmentMock.SetupGet(_ => _.ProcessedShards).Returns(processed);

            var targetMock = repository.Create<MemberConditionSubtask>();
            var scaffold = Stub.Create<InjectionScaffold>();
            targetMock.Setup(_ => _.ProcessShard(scaffold, fragmentMock.Object, shard));

            // Act
            targetMock.Object.Execute(scaffold, fragmentMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberConditionSubtask();

            // Act
            target.ProcessShard(null, Stub.Create<MemberFragment>(), Stub.Create<PredicateConditionMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberConditionSubtask();

            // Act
            target.ProcessShard(Stub.Create<InjectionScaffold>(), null, Stub.Create<PredicateConditionMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberConditionSubtask();

            // Act
            target.ProcessShard(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessShard_ShardPredicateIsFalse_GoesToLabel()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var fragmentMock = repository.Create<MemberFragment>();
            var shardMock = repository.Create<PredicateConditionMemberShard>();
            var label = Expression.Label();

            var parameter = Expression.Parameter(typeof(object), "injectionParameters");

            scaffoldMock.Setup(_ => _.GetLabel(fragmentMock.Object, LegendaryLabels.End)).Returns(label);
            scaffoldMock.SetupGet(_ => _.Variables).Returns(new[] { parameter });
            shardMock.SetupGet(_ => _.PredicateParameterType).Returns(typeof(object));

            shardMock.SetupGet(_ => _.Predicate).Returns((Func<object, bool>)(x => true.Equals(x)));

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var target = new MemberConditionSubtask();

            // Act
            target.ProcessShard(scaffoldMock.Object, fragmentMock.Object, shardMock.Object);

            // Assert
            repository.VerifyAll();
            expressions.Add(Expression.Throw(Expression.Constant(new Exception())));
            expressions.Add(Expression.Label(label));

            var action = Expression.Lambda<Action<object>>(Expression.Block(expressions), parameter).Compile();
            action(false);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = false)]
        public void ProcessShard_ShardPredicateIsTrue_Continues()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var fragmentMock = repository.Create<MemberFragment>();
            var shardMock = repository.Create<PredicateConditionMemberShard>();
            var label = Expression.Label();

            var parameter = Expression.Parameter(typeof(object), "injectionParameters");

            scaffoldMock.Setup(_ => _.GetLabel(fragmentMock.Object, LegendaryLabels.End)).Returns(label);
            scaffoldMock.SetupGet(_ => _.Variables).Returns(new[] { parameter });
            shardMock.SetupGet(_ => _.PredicateParameterType).Returns(typeof(object));

            shardMock.SetupGet(_ => _.Predicate).Returns((Func<object, bool>)(x => true.Equals(x)));

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var target = new MemberConditionSubtask();

            // Act
            target.ProcessShard(scaffoldMock.Object, fragmentMock.Object, shardMock.Object);

            // Assert
            repository.VerifyAll();
            expressions.Add(Expression.Throw(Expression.Constant(new Exception())));
            expressions.Add(Expression.Label(label));

            var action = Expression.Lambda<Action<object>>(Expression.Block(expressions), parameter).Compile();
            action(true);
        }
    }
}