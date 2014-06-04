// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskDetailTests.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskDetailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.InjectionFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.InjectionFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AutoInjectionTaskDetail"/> class.
    /// </summary>
    [TestClass]
    public class AutoInjectionTaskDetailTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateSourceTargetPairs_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateSourceTargetPairs(null, new IAutoInjectionStrategy[0]).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateSourceTargetPairs_StrategiesParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateSourceTargetPairs(Stub.Create<InjectionScaffold>(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_NoUnprocessedTargetMembers_ReturnsEmptyCollection()
        {
            // Arrange
            var target = CreateTarget();
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock
                .SetupGet(_ => _.UnprocessedTargetMembers)
                .Returns(Enumerable.Empty<MemberInfo>());
            
            // Act
            var result = target
                .CreateSourceTargetPairs(
                    scaffoldMock.Object,
                    new[] { Stub.Create<IAutoInjectionStrategy>() })
                .ToArray();

            // Assert
            scaffoldMock.VerifyAll();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_StrategyFails_ReturnsEmptyCollection()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget();

            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new[] { Stub.Create<MemberInfo>() });
            scaffoldMock.SetupGet(_ => _.SourceMembers).Returns(new List<MemberInfo>());

            // ReSharper disable once RedundantAssignment
            var sourceMember = Stub.Create<MemberInfo>();
            var strategyMock = repository.Create<IAutoInjectionStrategy>();
            strategyMock
                .Setup(_ => 
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        It.IsAny<MemberInfo>(),
                        out sourceMember))
                .Returns(false);

            // Act
            var result = target
                .CreateSourceTargetPairs(
                    scaffoldMock.Object,
                    new[] { strategyMock.Object })
                .ToArray();

            // Assert
            repository.VerifyAll();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_StrategyPasses_ReturnsPair()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget();

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var targetMember = Stub.Create<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new[] { targetMember });
            scaffoldMock.SetupGet(_ => _.SourceMembers).Returns(new List<MemberInfo>());

            // ReSharper disable once RedundantAssignment
            var sourceMember = Stub.Create<MemberInfo>();
            var strategyMock = repository.Create<IAutoInjectionStrategy>();
            strategyMock
                .Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        It.IsAny<MemberInfo>(),
                        out sourceMember))
                .Returns(true);

            // Act
            var result = target
                .CreateSourceTargetPairs(
                    scaffoldMock.Object,
                    new[] { strategyMock.Object })
                .ToArray();

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(sourceMember, result[0].Item1);
            Assert.AreEqual(targetMember, result[0].Item2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_MultipleStrategiesPass_ReturnsSinglePair()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget();

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var targetMember = Stub.Create<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new[] { targetMember });
            scaffoldMock.SetupGet(_ => _.SourceMembers).Returns(new List<MemberInfo>());

            // ReSharper disable once RedundantAssignment
            var sourceMember = Stub.Create<MemberInfo>();
            var strategyMock = repository.Create<IAutoInjectionStrategy>();
            strategyMock
                .Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        It.IsAny<MemberInfo>(),
                        out sourceMember))
                .Returns(true);

            // Act
            var result = target
                .CreateSourceTargetPairs(
                    scaffoldMock.Object,
                    new[] { strategyMock.Object, strategyMock.Object })
                .ToArray();

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_StrategyCall_MadeWithExpectedParameters()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var target = CreateTarget();

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var targetMember = Stub.Create<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new[] { targetMember });

            var sourceMembers = new List<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.SourceMembers).Returns(sourceMembers);

            // ReSharper disable once RedundantAssignment
            var sourceMember = Stub.Create<MemberInfo>();
            var strategyMock = repository.Create<IAutoInjectionStrategy>();
            strategyMock
                .Setup(_ =>
                    _.TryGetSourceForTarget(
                        sourceMembers,
                        targetMember,
                        out sourceMember))
                .Returns(false);

            // Act
            target.CreateSourceTargetPairs(
                scaffoldMock.Object,
                new[] { strategyMock.Object, strategyMock.Object }).Naught();

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateSourceTargetPairs_MultipleMembersAndStrategies_ExpectedPairsReturned()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Loose);
            var target = CreateTarget();

            var scaffoldMock = repository.Create<InjectionScaffold>();

            var targetMember1 = Stub.Create<MemberInfo>();
            var targetMember2 = Stub.Create<MemberInfo>();
            var targetMember3 = Stub.Create<MemberInfo>();
            var targetMember4 = Stub.Create<MemberInfo>();
            scaffoldMock
                .SetupGet(_ => _.UnprocessedTargetMembers)
                .Returns(new[] { targetMember1, targetMember2, targetMember3, targetMember4 });

            scaffoldMock.SetupGet(_ => _.SourceMembers).Returns(new List<MemberInfo>());

            // ReSharper disable RedundantAssignment
            var sourceMemberX = Stub.Create<MemberInfo>();
            var sourceMember1 = Stub.Create<MemberInfo>();
            var sourceMember2 = Stub.Create<MemberInfo>();
            var sourceMember3 = Stub.Create<MemberInfo>();
            var strategyMock1 = repository.Create<IAutoInjectionStrategy>();
            strategyMock1.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember1,
                        out sourceMember1))
                .Returns(true);
            strategyMock1.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember2,
                        out sourceMember2))
                .Returns(true);
            strategyMock1.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember3,
                        out sourceMemberX))
                .Returns(false);
            strategyMock1.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember4,
                        out sourceMemberX))
                .Returns(false);

            var strategyMock2 = repository.Create<IAutoInjectionStrategy>();
            strategyMock2.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember1,
                        out sourceMemberX))
                .Returns(true);
            strategyMock2.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember2,
                        out sourceMemberX))
                .Returns(false);
            strategyMock2.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember3,
                        out sourceMember3))
                .Returns(true);
            strategyMock2.Setup(_ =>
                    _.TryGetSourceForTarget(
                        It.IsAny<IEnumerable<MemberInfo>>(),
                        targetMember4,
                        out sourceMemberX))
                .Returns(false);

            // Act
            var result = target.CreateSourceTargetPairs(
                scaffoldMock.Object,
                new[] { strategyMock1.Object, strategyMock2.Object }).ToArray();

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.Any(x => x.Item1 == sourceMember1 && x.Item2 == targetMember1));
            Assert.IsTrue(result.Any(x => x.Item1 == sourceMember2 && x.Item2 == targetMember2));
            Assert.IsTrue(result.Any(x => x.Item1 == sourceMember3 && x.Item2 == targetMember3));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessPair_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessPair(null, Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>()));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessPair_PairParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessPair(Stub.Create<InjectionScaffold>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_ValidParameters_TargetMemberAddedToProcessedTargetMembers()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Loose) { CallBase = true };

            var targetMember = Stub.Create<MemberInfo>();

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(repository, out expressions);

            var targetMock = repository.Create<AutoInjectionTaskDetail>();
            targetMock
                .Setup(_ => _.CreateExpression(
                    It.IsAny<InjectionScaffold>(), It.IsAny<MemberInfo>(), It.IsAny<MemberInfo>()))
                .Returns(Stub.Create<Expression>());

            // Act
            targetMock.Object.ProcessPair(scaffold, Tuple.Create(Stub.Create<MemberInfo>(), targetMember));

            // Assert
            Assert.AreEqual(1, scaffold.ProcessedTargetMembers.Count());
            Assert.IsTrue(scaffold.ProcessedTargetMembers.Contains(targetMember));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_ValidParameters_CallsCreateExpression()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Loose) { CallBase = true };

            var sourceMember = Stub.Create<MemberInfo>();
            var targetMember = Stub.Create<MemberInfo>();

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(repository, out expressions);

            var targetMock = repository.Create<AutoInjectionTaskDetail>();
            targetMock
                .Setup(_ => _.CreateExpression(scaffold, sourceMember, targetMember))
                .Returns(Stub.Create<Expression>())
                .Verifiable();

            // Act
            targetMock.Object.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            repository.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_ValidParameters_AddsExpressionToScaffold()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Loose) { CallBase = true };

            var sourceMember = Stub.Create<MemberInfo>();
            var targetMember = Stub.Create<MemberInfo>();

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(repository, out expressions);

            var expression = Stub.Create<Expression>();
            var targetMock = repository.Create<AutoInjectionTaskDetail>();
            targetMock
                .Setup(_ => _.CreateExpression(scaffold, sourceMember, targetMember))
                .Returns(expression);

            // Act
            targetMock.Object.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Assert.AreEqual(1, expressions.Count());
            Assert.IsTrue(expressions.Contains(expression));
        }

        private static AutoInjectionTaskDetail CreateTarget()
        {
            return new Mock<AutoInjectionTaskDetail>(MockBehavior.Loose) { CallBase = true }.Object;
        }

        private static InjectionScaffold CreateScaffoldForProcessPair(
            MockRepository repository,
            out List<Expression> expressions)
        {
            var scaffoldMock = repository.Create<InjectionScaffold>();

            expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var processedTargetMembers = new HashSet<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(processedTargetMembers);

            return scaffoldMock.Object;
        }
    }
}