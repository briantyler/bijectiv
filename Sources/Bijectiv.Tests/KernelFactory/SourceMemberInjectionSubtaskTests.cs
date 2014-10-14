// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberInjectionSubtaskTests.cs" company="Bijectiv">
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
//   Defines the SourceMemberInjectionSubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="SourceMemberInjectionSubtask{TShard}"/> class.
    /// </summary>
    [TestClass]
    public class SourceMemberInjectionSubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceExpressionFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new SourceMemberInjectionSubtask<SourceMemberShard>(null, Stub.Create<ITargetMemberInjector>(), false).Naught();
            
            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InjectorParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new SourceMemberInjectionSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>(), 
                null, 
                false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new SourceMemberInjectionSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>(),
                Stub.Create<ITargetMemberInjector>(),
                false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceExpressionFactoryParameter_IsAssignedToSourceExpressionFactoryProperty()
        {
            // Arrange
            var sourceExpressionFactory = Stub.Create<ISourceExpressionFactory<SourceMemberShard>>();

            // Act
            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                sourceExpressionFactory,
                Stub.Create<ITargetMemberInjector>(),
                false);

            // Assert
            Assert.AreEqual(sourceExpressionFactory, target.SourceExpressionFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectorParameter_IsAssignedToInjectorProperty()
        {
            // Arrange
            var injector = Stub.Create<ITargetMemberInjector>();

            // Act
            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>(),
                injector,
                false);

            // Assert
            Assert.AreEqual(injector, target.Injector);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_IsMergeParameter_IsAssignedToIsMergeProperty()
        {
            // Arrange

            // Act
            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>(),
                Stub.Create<ITargetMemberInjector>(),
                true);

            // Assert
            Assert.AreEqual(true, target.IsMerge);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ShardCategoryIsSource()
        {
            // Arrange

            // Act
            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                Stub.Create<ISourceExpressionFactory<SourceMemberShard>>(),
                Stub.Create<ITargetMemberInjector>(),
                true);

            // Assert
            Assert.AreEqual(LegendaryShards.Source, target.Category);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessShard_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

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
            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

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
            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessShard(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessShard_IsMergeFalse_Transforms()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffold = Stub.Create<InjectionScaffold>();
            var member = Stub.Create<MemberInfo>();

            var fragmentMock = repository.Create<MemberFragment>();
            fragmentMock.SetupGet(_ => _.Member).Returns(member);

            var shard = Stub.Create<SourceMemberShard>();
            var sourceExpression = Expression.Empty();

            var sourceExpressionFactoryMock = repository.Create<ISourceExpressionFactory<SourceMemberShard>>();
            sourceExpressionFactoryMock.Setup(_ => _.Create(scaffold, fragmentMock.Object, shard)).Returns(sourceExpression);

            var injectorMock = repository.Create<ITargetMemberInjector>();
            injectorMock.Setup(_ => _.AddTransformExpressionToScaffold(scaffold, member, sourceExpression));

            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                sourceExpressionFactoryMock.Object,
                injectorMock.Object,
                false);

            // Act
            target.ProcessShard(scaffold, fragmentMock.Object, shard);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessShard_IsMergeTrue_Merges()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffold = Stub.Create<InjectionScaffold>();
            var member = Stub.Create<MemberInfo>();

            var fragmentMock = repository.Create<MemberFragment>();
            fragmentMock.SetupGet(_ => _.Member).Returns(member);

            var shard = Stub.Create<SourceMemberShard>();
            var sourceExpression = Expression.Empty();

            var sourceExpressionFactoryMock = repository.Create<ISourceExpressionFactory<SourceMemberShard>>();
            sourceExpressionFactoryMock.Setup(_ => _.Create(scaffold, fragmentMock.Object, shard)).Returns(sourceExpression);

            var injectorMock = repository.Create<ITargetMemberInjector>();
            injectorMock.Setup(_ => _.AddMergeExpressionToScaffold(scaffold, member, sourceExpression));

            var target = new SourceMemberInjectionSubtask<SourceMemberShard>(
                sourceExpressionFactoryMock.Object,
                injectorMock.Object,
                true);

            // Act
            target.ProcessShard(scaffold, fragmentMock.Object, shard);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CanProcess_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CanProcess(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanProcess_ShardParameterInjectIsTrue_ReturnsTrue()
        {
            // Arrange
            var shardMock = new Mock<SourceMemberShard>(MockBehavior.Strict);
            shardMock.SetupGet(_ => _.Inject).Returns(true);
            
            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

            // Act
            var result = target.CanProcess(shardMock.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CanProcess_ShardParameterInjectIsFalse_ReturnsFalse()
        {
            // Arrange
            var shardMock = new Mock<SourceMemberShard>(MockBehavior.Strict);
            shardMock.SetupGet(_ => _.Inject).Returns(false);

            var target = new Mock<SourceMemberInjectionSubtask<SourceMemberShard>>(MockBehavior.Loose) { CallBase = true }.Object;

            // Act
            var result = target.CanProcess(shardMock.Object);

            // Assert
            Assert.IsFalse(result);
        }
    }
}