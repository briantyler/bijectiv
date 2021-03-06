﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberInjectionTaskTests.cs" company="Bijectiv">
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
//   Defines the MemberInjectionTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="MemberInjectionTask"/> class.
    /// </summary>
    [TestClass]
    public class MemberInjectionTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SubtasksParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new MemberInjectionTask(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SubtasksParameter_IsAssignedToSubtasksProperty()
        {
            // Arrange
            var subtasks = new List<IInjectionSubtask<MemberFragment>>();

            // Act
            var testTarget = new MemberInjectionTask(subtasks);

            // Assert
            Assert.AreEqual(subtasks, testTarget.Subtasks);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_AllUnprocessedMemberFragmentsInScaffold_AreProcessed()
        {
            // Arrange
            var unprocessedFragments = new[]
            {
                new MemberFragment(
                    TestClass1.T,
                    BaseTestClass3.T,
                    Reflect<BaseTestClass3>.Property(_ => _.Id)),
                Stub.Fragment<TestClass1, BaseTestClass3>(LegendaryFragments.AutoInjection),
                new MemberFragment(
                    TestClass1.T,
                    DerivedTestClass3.T,
                    Reflect<DerivedTestClass3>.Property(_ => _.Measure)),
                Stub.Fragment<TestClass1, BaseTestClass3>(LegendaryFragments.Factory),
                new MemberFragment(
                    TestClass1.T,
                    BaseTestClass3.T,
                    Reflect<BaseTestClass3>.Property(_ => _.Value))
            };
            var expected = new[] { unprocessedFragments[0], unprocessedFragments[2], unprocessedFragments[4] };

            var processedFragments = new HashSet<InjectionFragment>();
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock.SetupGet(_ => _.UnprocessedFragments).Returns(unprocessedFragments);
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(processedFragments);
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new List<MemberInfo>());
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(new HashSet<MemberInfo>());

            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());

            // Act
            testTarget.Execute(scaffoldMock.Object);

            // Assert
            processedFragments.AssertSetEqual(expected);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DuplicateMemberFragmentsInScaffold_OnlyFirstFragmentIsProcessed()
        {
            // Arrange
            var unprocessedFragments = new[]
            {
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<BaseTestClass3>.Property(_ => _.Id)),
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<DerivedTestClass3>.Property(_ => _.Id)),
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<BaseTestClass3>.Property(_ => _.Value)),
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<DerivedTestClass3>.Property(_ => _.Value)),
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<BaseTestClass3>.Property(_ => _.Measure)),
                new MemberFragment(TestClass1.T, DerivedTestClass3.T, Reflect<DerivedTestClass3>.Property(_ => _.Measure))
            };

            var unprocessedTargetMembers = new List<MemberInfo>
            {
                Reflect<DerivedTestClass3>.Property(_ => _.Id),
                Reflect<DerivedTestClass3>.Property(_ => _.Value),
                Reflect<BaseTestClass3>.Property(_ => _.Measure),
                Reflect<DerivedTestClass3>.Property(_ => _.Measure),
            };

            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock.SetupGet(_ => _.UnprocessedFragments).Returns(unprocessedFragments);
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(new HashSet<InjectionFragment>());
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(unprocessedTargetMembers);

            var proessedFragments = new List<MemberFragment>();

            var testTargetMock = new Mock<MemberInjectionTask>(
                MockBehavior.Strict,
                new List<IInjectionSubtask<MemberFragment>>());

            testTargetMock
                .Setup(_ => _.ProcessFragment(scaffoldMock.Object, It.IsAny<MemberFragment>()))
                .Callback(
                    (InjectionScaffold s, MemberFragment f) =>
                        {
                            unprocessedTargetMembers.Remove(f.Member);
                            proessedFragments.Add(f);
                        });

            // Act
            testTargetMock.Object.Execute(scaffoldMock.Object);

            // Assert
            testTargetMock.VerifyAll();

            new[]
                {
                    unprocessedFragments[0], 
                    unprocessedFragments[2], 
                    unprocessedFragments[4], 
                    unprocessedFragments[5]
                }.AssertSetEqual(proessedFragments);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessFragment_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());
            var fragment = new MemberFragment(TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id));
            
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.ProcessFragment(null, fragment);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessFragment_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(new HashSet<MemberInfo>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.ProcessFragment(scaffoldMock.Object, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessFragment_FragmentParameter_ProcessedShardsPropertyIsCleared()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());
            
            var processedMock = repository.Create<ISet<MemberShard>>();
            processedMock.Setup(_ => _.Clear());

            var fragmentMock = repository.Create<MemberFragment>();
            fragmentMock.SetupGet(_ => _.ProcessedShards).Returns(processedMock.Object);
            fragmentMock.SetupGet(_ => _.Member).Returns(Stub.Create<MemberInfo>());

            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(new HashSet<MemberInfo>());

            // Act
            testTarget.ProcessFragment(scaffoldMock.Object, fragmentMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessFragment_FragmentParameter_MemberPropertyIsProcessed()
        {
            // Arrange
            var testTarget = new MemberInjectionTask(new List<IInjectionSubtask<MemberFragment>>());
            var fragment = new MemberFragment(TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id));
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            var processedMembers = new HashSet<MemberInfo>();
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(processedMembers);

            // Act
            testTarget.ProcessFragment(scaffoldMock.Object, fragment);

            // Assert
            Assert.AreEqual(Reflect<TestClass2>.Property(_ => _.Id), processedMembers.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessFragment_SubtasksProperty_ExecutesAllTasksInOrder()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            
            var fragment = new MemberFragment(TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id));
            
            var scaffoldMock = repository.Create<InjectionScaffold>(MockBehavior.Loose);
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(new HashSet<MemberInfo>());

            var subTask1Mock = repository.Create<IInjectionSubtask<MemberFragment>>();
            var subTask2Mock = repository.Create<IInjectionSubtask<MemberFragment>>();
            var subTask3Mock = repository.Create<IInjectionSubtask<MemberFragment>>();

            var sequence = new MockSequence();
            subTask1Mock.InSequence(sequence).Setup(_ => _.Execute(scaffoldMock.Object, fragment));
            subTask2Mock.InSequence(sequence).Setup(_ => _.Execute(scaffoldMock.Object, fragment));
            subTask3Mock.InSequence(sequence).Setup(_ => _.Execute(scaffoldMock.Object, fragment));

            var subtasks = new[] { subTask1Mock.Object, subTask2Mock.Object, subTask3Mock.Object };
            var testTarget = new MemberInjectionTask(subtasks);

            // Act
            testTarget.ProcessFragment(scaffoldMock.Object, fragment);

            // Assert
            repository.VerifyAll();
        }
    }
}