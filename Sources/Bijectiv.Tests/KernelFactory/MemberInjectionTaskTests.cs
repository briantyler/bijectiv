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
        public void CreateInstance_SubTasksParameterIsNull_Throws()
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
            new MemberInjectionTask(new List<IInjectionSubTask<MemberFragment>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SubTasksParameter_IsAssignedToSubTasksProperty()
        {
            // Arrange
            var subTasks = new List<IInjectionSubTask<MemberFragment>>();

            // Act
            var target = new MemberInjectionTask(subTasks);

            // Assert
            Assert.AreEqual(subTasks, target.SubTasks);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberInjectionTask(new List<IInjectionSubTask<MemberFragment>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_AllUnprocessedMemberFragmentsInScaffold_AreProcessed()
        {
            // Arrange
            var unprocessedFragments = CreateUnprocessedFragments();

            var processedFragments = new HashSet<InjectionFragment>();
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock.SetupGet(_ => _.UnprocessedFragments).Returns(unprocessedFragments);
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(processedFragments);
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(new List<MemberInfo>());
            scaffoldMock.SetupGet(_ => _.ProcessedTargetMembers).Returns(new HashSet<MemberInfo>());

            var target = new MemberInjectionTask(new List<IInjectionSubTask<MemberFragment>>());

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            processedFragments.AssertSetEqual(unprocessedFragments.OfType<MemberFragment>());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_UnprocessedIdenticalMember_FragmentIsProcessed()
        {
            // Arrange
            var unprocessedFragments = new[]
            {
                new MemberFragment(TestClass1.T, BaseTestClass3.T, Reflect<BaseTestClass3>.Property(_ => _.Id))
            };

            var unprocessedTargetMembers = new MemberInfo[]
            {
                Reflect<BaseTestClass3>.Property(_ => _.Id)
            };

            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            scaffoldMock.SetupGet(_ => _.UnprocessedFragments).Returns(unprocessedFragments);
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(new HashSet<InjectionFragment>());
            scaffoldMock.SetupGet(_ => _.UnprocessedTargetMembers).Returns(unprocessedTargetMembers);

            var targetMock = new Mock<MemberInjectionTask>(
                MockBehavior.Strict, 
                new List<IInjectionSubTask<MemberFragment>>());
            targetMock.Setup(_ => _.ProcessFragment(scaffoldMock.Object, unprocessedFragments[0]));

            // Act
            targetMock.Object.Execute(scaffoldMock.Object);

            // Assert
            targetMock.VerifyAll();
        }

        private static List<InjectionFragment> CreateUnprocessedFragments()
        {
            return new List<InjectionFragment>
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
                    Reflect<BaseTestClass3>.Property(_ => _.Value)),
            };
        }
    }
}