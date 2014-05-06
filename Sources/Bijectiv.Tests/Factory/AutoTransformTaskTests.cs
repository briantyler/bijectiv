// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTransformTaskTests.cs" company="Bijectiv">
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
//   Defines the AutoTransformTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AutoTransformTask"/> class.
    /// </summary>
    [TestClass]
    public class AutoTransformTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DetailParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new AutoTransformTask(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AutoTransformTask(Stub.Create<AutoTransformTaskDetail>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DetailParameter_IsAssignedToDetailProperty()
        {
            // Arrange
            var detail = Stub.Create<AutoTransformTaskDetail>();

            // Act
            var target = new AutoTransformTask(detail);

            // Assert
            Assert.AreEqual(detail, target.Detail);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var detail = Stub.Create<AutoTransformTaskDetail>();
            var target = new AutoTransformTask(detail);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_SourceTargetPairs_CreatedInFragmentStrategyOrder()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var strategy1 = Stub.Create<IAutoTransformStrategy>();
            var strategy2 = Stub.Create<IAutoTransformStrategy>();

            var scaffoldMock = repository.Create<TransformScaffold>();
            var transformFragments = new[]
            {
                new AutoTransformFragment(TestClass1.T, TestClass2.T, strategy1),                  
                new AutoTransformFragment(TestClass1.T, TestClass2.T, strategy2)                  
            };

            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(transformFragments);
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(new HashSet<TransformFragment>());

            var detailMock = repository.Create<AutoTransformTaskDetail>();
            detailMock
                .Setup(
                    _ =>
                    _.CreateSourceTargetPairs(
                        scaffoldMock.Object,
                        It.Is<IEnumerable<IAutoTransformStrategy>>(
                            strategies => transformFragments.Select(item => item.Strategy).SequenceEqual(strategies))))
                .Returns(new Tuple<MemberInfo, MemberInfo>[0]);

            var target = new AutoTransformTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_AutoTransformFragments_AreAllProcessed()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<TransformScaffold>();
            var transformFragments = new[]
            {
                Stub.Fragment<TestClass1, TestClass2>(),          
                new AutoTransformFragment(TestClass1.T, TestClass2.T, Stub.Create<IAutoTransformStrategy>()),                  
                Stub.Fragment<TestClass1, TestClass2>(),          
                new AutoTransformFragment(TestClass1.T, TestClass2.T, Stub.Create<IAutoTransformStrategy>()),                  
                Stub.Fragment<TestClass1, TestClass2>()               
            };

            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(transformFragments);

            var processedFragments = new HashSet<TransformFragment>();
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(processedFragments);

            var detailMock = repository.Create<AutoTransformTaskDetail>();
            detailMock
                .Setup(_ =>
                    _.CreateSourceTargetPairs(
                        scaffoldMock.Object,
                        It.IsAny<IEnumerable<IAutoTransformStrategy>>()))
                .Returns(new Tuple<MemberInfo, MemberInfo>[0]);

            var target = new AutoTransformTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(2, processedFragments.Count());
            Assert.IsTrue(processedFragments.Contains(transformFragments[1]));
            Assert.IsTrue(processedFragments.Contains(transformFragments[3]));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_CreatedPairs_AreProcessedInOrder()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<TransformScaffold>();
            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(new TransformFragment[0]);
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(new HashSet<TransformFragment>());

            var detailMock = repository.Create<AutoTransformTaskDetail>();
            var pairs = new[]
            {
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>()),
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>()),
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>())
            };

            detailMock
                .Setup(_ =>
                    _.CreateSourceTargetPairs(
                        It.IsAny<TransformScaffold>(),
                        It.IsAny<IEnumerable<IAutoTransformStrategy>>()))
                .Returns(pairs);

            var call = new[] { 0 };
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[0])).Callback(() => Assert.AreEqual(0, call[0]++));
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[1])).Callback(() => Assert.AreEqual(1, call[0]++));
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[2])).Callback(() => Assert.AreEqual(2, call[0]++));

            var target = new AutoTransformTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(3, call[0]);
        }
    }
}