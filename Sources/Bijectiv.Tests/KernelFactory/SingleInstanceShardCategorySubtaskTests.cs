// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleInstanceShardCategorySubtaskTests.cs" company="Bijectiv">
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
//   Defines the SingleInstanceShardCategorySubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the see <see cref="SingleInstanceShardCategorySubtask{TShard}"/> class.
    /// </summary>
    [TestClass]
    public class SingleInstanceShardCategorySubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            Stub.Create<SingleInstanceShardCategorySubtask<MemberShard>>(new Guid("9616527D-588F-4A94-B58B-B3692DC9190A"))
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CategoryParameter_IsAssignedToCategoryProperty()
        {
            // Arrange
            var category = new Guid("9616527D-588F-4A94-B58B-B3692DC9190A");

            // Act
            var testTarget = Stub.Create<SingleInstanceShardCategorySubtask<MemberShard>>(category);

            // Assert
            Assert.AreEqual(category, testTarget.Category);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var category = new Guid("9616527D-588F-4A94-B58B-B3692DC9190A");
            var testTarget = Stub.Create<SingleInstanceShardCategorySubtask<MemberShard>>(category);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null, Stub.Create<MemberFragment>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var category = new Guid("9616527D-588F-4A94-B58B-B3692DC9190A");
            var testTarget = Stub.Create<SingleInstanceShardCategorySubtask<MemberShard>>(category);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(Stub.Create<InjectionScaffold>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_MatchingCategoryShards_AreAddedToProcessedShardsCollection()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var fragmentMock = repository.Create<MemberFragment>();
            var shardMock = repository.Create<MemberShard>();
            shardMock.SetupGet(_ => _.ShardCategory).Returns(LegendaryShards.Condition);
            var unprocessed = new List<MemberShard>
            {
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
                Stub.Create<MemberShard>(),
                shardMock.Object,
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
            };
            var processed = new HashSet<MemberShard>();
            fragmentMock.SetupGet(_ => _.UnprocessedShards).Returns(unprocessed);
            fragmentMock.SetupGet(_ => _.ProcessedShards).Returns(processed);

            var targetMock = repository.Create<SingleInstanceShardCategorySubtask<MemberShard>>(
                MockBehavior.Loose,
                LegendaryShards.Condition);
            targetMock.Setup(_ => _.CanProcess(It.IsAny<PredicateConditionMemberShard>())).Returns(true);

            // Act
            targetMock.Object.Execute(Stub.Create<InjectionScaffold>(), fragmentMock.Object);

            // Assert
            new[] { unprocessed[1], unprocessed[3], unprocessed[5] }.AssertSetEqual(processed);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_FirstCategoryShardHasExpectedType_IsProcessed()
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

            var targetMock = repository.Create<SingleInstanceShardCategorySubtask<MemberShard>>(
                MockBehavior.Loose,
                LegendaryShards.Condition);

            var scaffold = Stub.Create<InjectionScaffold>();
            targetMock.Setup(_ => _.CanProcess(shard)).Returns(true);
            targetMock.Setup(_ => _.ProcessShard(scaffold, fragmentMock.Object, shard));

            // Act
            targetMock.Object.Execute(scaffold, fragmentMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_FirstCategoryShardDoesNotHaveExpectedType_IsNotProcessed()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };

            var fragmentMock = repository.Create<MemberFragment>();
            var shardMock = repository.Create<MemberShard>();
            shardMock.SetupGet(_ => _.ShardCategory).Returns(LegendaryShards.Condition);
            var unprocessed = new List<MemberShard>
            {
                Stub.Create<MemberShard>(),
                shardMock.Object,
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
                Stub.Create<MemberShard>(),
                Stub.Create<PredicateConditionMemberShard>(),
            };
            fragmentMock.SetupGet(_ => _.UnprocessedShards).Returns(unprocessed);

            var targetMock = repository.Create<SingleInstanceShardCategorySubtask<PredicateConditionMemberShard>>(
                MockBehavior.Strict,
                LegendaryShards.Condition);

            var scaffold = Stub.Create<InjectionScaffold>();

            // Act
            targetMock.Object.Execute(scaffold, fragmentMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_FirstCategoryShardHasExpectedTypeButCannotBeProcessed_IsNotProcessed()
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
            fragmentMock.SetupGet(_ => _.UnprocessedShards).Returns(unprocessed);

            var targetMock = repository.Create<SingleInstanceShardCategorySubtask<MemberShard>>(
                MockBehavior.Strict,
                LegendaryShards.Condition);

            var scaffold = Stub.Create<InjectionScaffold>();
            targetMock.Setup(_ => _.CanProcess(shard)).Returns(false);

            // Act
            targetMock.Object.Execute(scaffold, fragmentMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanProcess_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var testTarget = Stub.Create<SingleInstanceShardCategorySubtask<MemberShard>>(LegendaryShards.Condition);
            
            // Act
            var result = testTarget.CanProcess(null);

            // Assert
            Assert.IsTrue(result);
        }
    }
}