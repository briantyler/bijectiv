// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTransformTaskDetailTests.cs" company="Bijectiv">
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
//   Defines the AutoTransformTaskDetailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Factory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AutoTransformTaskDetail"/> class.
    /// </summary>
    [TestClass]
    public class AutoTransformTaskDetailTests
    {
        private const int SourceInt = 17;

        private const int ExpectedInt = 34;

        private static readonly DerivedTestClass1 SourceBase = new DerivedTestClass1();

        private static readonly DerivedTestClass1 ExpectedBase = new DerivedTestClass1();

        private static readonly SealedClass1 SourceSealed = new SealedClass1();

        private static readonly SealedClass1 ExpectedSealed = new SealedClass1();

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AutoTransformTaskDetail().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateSourceTargetPairs_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();
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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

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
            var target = new AutoTransformTaskDetail();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessPair(Stub.Create<InjectionScaffold>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_ValidParameters_TargetMemberAddedToProcessedTargetMebers()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Assert.AreEqual(1, scaffold.ProcessedTargetMembers.Count());
            Assert.IsTrue(scaffold.ProcessedTargetMembers.Contains(targetMember));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsValueTypePropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsValueTypeFieldToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { FieldInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsValueTypePropertyToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.FieldInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsValueTypeFieldToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { FieldInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.FieldInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsNotValueTypePropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsNotValueTypeFieldToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { FieldBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsNotValueTypePropertyToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsNotValueTypeFieldToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { FieldBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_SourceIsSealedPropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertySealed = SourceSealed };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);

            var injectionContext = CreateInjectionContext(
                repository, typeof(SealedClass1), typeof(SealedClass1), SourceSealed, ExpectedSealed);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedSealed, targetInstance.PropertySealed);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessPair_IntPropertyDerivedField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(BaseTestClass1), SourceInt, ExpectedBase);

            List<Expression> expressions;
            var scaffold = CreateScaffoldForProcessPair(
                repository, injectionContext, sourceInstance, targetInstance, out expressions);

            var target = new AutoTransformTaskDetail();

            // Act
            target.ProcessPair(scaffold, Tuple.Create(sourceMember, targetMember));

            // Assert
            Expression.Lambda<Action>(Expression.Block(expressions)).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        private static IInjectionContext CreateInjectionContext(
            MockRepository repository, Type sourceMember, Type targetMember, object sourceValue, object targetValue)
        {
            var transformMock = repository.Create<ITransform>();

            var injectionStoreMock = repository.Create<IInjectionStore>();
            injectionStoreMock.Setup(_ => _.Resolve<ITransform>(sourceMember, targetMember)).Returns(transformMock.Object);

            var injectionContextMock = repository.Create<IInjectionContext>();
            injectionContextMock.SetupGet(_ => _.InjectionStore).Returns(injectionStoreMock.Object);

            transformMock
                .Setup(_ => _.Transform(sourceValue, injectionContextMock.Object))
                .Returns(targetValue);

            return injectionContextMock.Object;
        }

        private static InjectionScaffold CreateScaffoldForProcessPair(
            MockRepository repository,
            IInjectionContext injectionContext,
            AutoTransformTestClass1 sourceInstance,
            AutoTransformTestClass1 targetInstance,
            out List<Expression> expressions)
        {
            var scaffoldMock = repository.Create<InjectionScaffold>();

            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(injectionContext));
            scaffoldMock.SetupGet(_ => _.Source).Returns(Expression.Constant(sourceInstance));
            scaffoldMock.SetupGet(_ => _.Target).Returns(Expression.Constant(targetInstance));

            expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var processedTargetMembers = new HashSet<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(processedTargetMembers);

            return scaffoldMock.Object;
        }
    }
}