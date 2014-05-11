// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskTests.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskTests type.
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
    /// This class tests the <see cref="AutoInjectionTask"/> class.
    /// </summary>
    [TestClass]
    public class AutoInjectionTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DetailParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new AutoInjectionTask(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AutoInjectionTask(Stub.Create<AutoInjectionTaskDetail>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DetailParameter_IsAssignedToDetailProperty()
        {
            // Arrange
            var detail = Stub.Create<AutoInjectionTaskDetail>();

            // Act
            var target = new AutoInjectionTask(detail);

            // Assert
            Assert.AreEqual(detail, target.Detail);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var detail = Stub.Create<AutoInjectionTaskDetail>();
            var target = new AutoInjectionTask(detail);

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
            var strategy1 = Stub.Create<IAutoInjectionStrategy>();
            var strategy2 = Stub.Create<IAutoInjectionStrategy>();

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var injectionFragments = new[]
            {
                new AutoInjectionFragment(TestClass1.T, TestClass2.T, strategy1),                  
                new AutoInjectionFragment(TestClass1.T, TestClass2.T, strategy2)                  
            };

            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(injectionFragments);
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(new HashSet<InjectionFragment>());

            var detailMock = repository.Create<AutoInjectionTaskDetail>();
            detailMock
                .Setup(
                    _ =>
                    _.CreateSourceTargetPairs(
                        scaffoldMock.Object,
                        It.Is<IEnumerable<IAutoInjectionStrategy>>(
                            strategies => injectionFragments.Select(item => item.Strategy).SequenceEqual(strategies))))
                .Returns(new Tuple<MemberInfo, MemberInfo>[0]);

            var target = new AutoInjectionTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_AutoinjectionFragments_AreAllProcessed()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<InjectionScaffold>();
            var injectionFragments = new[]
            {
                Stub.Fragment<TestClass1, TestClass2>(),          
                new AutoInjectionFragment(TestClass1.T, TestClass2.T, Stub.Create<IAutoInjectionStrategy>()),                  
                Stub.Fragment<TestClass1, TestClass2>(),          
                new AutoInjectionFragment(TestClass1.T, TestClass2.T, Stub.Create<IAutoInjectionStrategy>()),                  
                Stub.Fragment<TestClass1, TestClass2>()               
            };

            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(injectionFragments);

            var processedFragments = new HashSet<InjectionFragment>();
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(processedFragments);

            var detailMock = repository.Create<AutoInjectionTaskDetail>();
            detailMock
                .Setup(_ =>
                    _.CreateSourceTargetPairs(
                        scaffoldMock.Object,
                        It.IsAny<IEnumerable<IAutoInjectionStrategy>>()))
                .Returns(new Tuple<MemberInfo, MemberInfo>[0]);

            var target = new AutoInjectionTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(2, processedFragments.Count());
            Assert.IsTrue(processedFragments.Contains(injectionFragments[1]));
            Assert.IsTrue(processedFragments.Contains(injectionFragments[3]));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_CreatedPairs_AreProcessedInOrder()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock.Setup(_ => _.UnprocessedFragments).Returns(new InjectionFragment[0]);
            scaffoldMock.Setup(_ => _.ProcessedFragments).Returns(new HashSet<InjectionFragment>());

            var detailMock = repository.Create<AutoInjectionTaskDetail>();
            var pairs = new[]
            {
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>()),
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>()),
                Tuple.Create(Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>())
            };

            detailMock
                .Setup(_ =>
                    _.CreateSourceTargetPairs(
                        It.IsAny<InjectionScaffold>(),
                        It.IsAny<IEnumerable<IAutoInjectionStrategy>>()))
                .Returns(pairs);

            var call = new[] { 0 };
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[0])).Callback(() => Assert.AreEqual(0, call[0]++));
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[1])).Callback(() => Assert.AreEqual(1, call[0]++));
            detailMock.Setup(_ => _.ProcessPair(scaffoldMock.Object, pairs[2])).Callback(() => Assert.AreEqual(2, call[0]++));

            var target = new AutoInjectionTask(detailMock.Object);

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(3, call[0]);
        }
    }
}