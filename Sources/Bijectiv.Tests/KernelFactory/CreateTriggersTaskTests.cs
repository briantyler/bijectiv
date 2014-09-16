// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTriggersTaskTests.cs" company="Bijectiv">
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
//   Defines the CreateTriggersTaskTests type.
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CreateTriggersTask"/> class.
    /// </summary>
    [TestClass]
    public class CreateTriggersTaskTests
    {
        private static readonly Action<IInjectionParameters<TestClass1, TestClass2>> Trigger = p => p.Naught(); 

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CreateTriggersTask(TriggeredBy.Nothing).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TriggeredByParameter_IsAssignedToTriggeredByProperty()
        {
            // Arrange

            // Act
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            // Assert
            Assert.AreEqual(TriggeredBy.InjectionEnded, target.TriggeredBy);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_ProcessesAllAppropriateUnprocessedInjectionTriggerFragments()
        {
            // Arrange
            var fragment1 = Stub.Fragment<TestClass1, TestClass2>();
            var fragment2 = Stub.Fragment<TestClass1, TestClass2>(LegendaryFragments.Trigger);
            var fragment3 = Stub.Fragment<TestClass1, TestClass2>();
            var fragment4 = Stub.Fragment<TestClass1, TestClass2>(LegendaryFragments.Trigger);
            var triggerFragment1 = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Trigger, TriggeredBy.Nothing);
            var triggerFragment2 = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Trigger, TriggeredBy.InjectionEnded);
            var triggerFragment3 = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Trigger, TriggeredBy.Nothing);
            var triggerFragment4 = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Trigger, TriggeredBy.InjectionEnded);

            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock
                .SetupGet(_ => _.UnprocessedFragments)
                .Returns(new[]
                {
                    fragment1,
                    triggerFragment1,
                    fragment2,
                    triggerFragment2,
                    fragment3,
                    triggerFragment3,
                    triggerFragment4,
                    fragment4
                });

            var targetMock = repository.Create<CreateTriggersTask>(TriggeredBy.InjectionEnded);
            targetMock.Setup(_ => _.ProcessFragment(triggerFragment2, scaffoldMock.Object));
            targetMock.Setup(_ => _.ProcessFragment(triggerFragment4, scaffoldMock.Object));
            
            var target = targetMock.Object;

            // Act
            target.Execute(scaffoldMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessFragment_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessFragment(null, Stub.Create<InjectionScaffold>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void ProcessFragment_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var fragment = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Trigger, TriggeredBy.InjectionEnded);
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.ProcessFragment(fragment, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessFragment_ValidParameters_ProcessesFragment()
        {
            // Arrange
            var fragment = new InjectionTriggerFragment(TestClass1.T, TestClass2.T, Trigger, TriggeredBy.InjectionEnded);
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            var scaffoldMock = new Mock<InjectionScaffold>();
            var parameter = Expression.Variable(typeof(IInjectionParameters<TestClass1, TestClass2>), "injectionParameters");
            scaffoldMock
                .SetupGet(_ => _.Variables)
                .Returns(new[] { parameter });
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(new List<Expression>());
            
            var processedFragments = new HashSet<InjectionFragment>();
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(processedFragments);

            // Act
            target.ProcessFragment(fragment, scaffoldMock.Object);

            // Assert
            Assert.AreEqual(1, processedFragments.Count());
            Assert.IsTrue(processedFragments.Contains(fragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ProcessFragment_ValidParameters_BuildsTrigger()
        {
            // Arrange
            var parameters = Stub.Create<IInjectionParameters<TestClass1, TestClass2>>();
            object called = false;
            Action<IInjectionParameters<TestClass1, TestClass2>> trigger = p =>
                {
                    Assert.AreEqual(p, parameters);
                    called = true;
                }; 


            var fragment = new InjectionTriggerFragment(TestClass1.T, TestClass2.T, trigger, TriggeredBy.InjectionEnded);
            var target = new CreateTriggersTask(TriggeredBy.InjectionEnded);

            var scaffoldMock = new Mock<InjectionScaffold>();
            var variable = Expression.Variable(typeof(IInjectionParameters<TestClass1, TestClass2>), "injectionParameters");
            var expressions = new List<Expression>();

            scaffoldMock.SetupGet(_ => _.Variables).Returns(new[] { variable });
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);
            scaffoldMock.SetupGet(_ => _.ProcessedFragments).Returns(new HashSet<InjectionFragment>());

            expressions.Add(Expression.Assign(variable, Expression.Constant(parameters)));

            // Act
            target.ProcessFragment(fragment, scaffoldMock.Object);

            // Assert
            Expression.Lambda<Action>(Expression.Block(new[] { variable }, expressions)).Compile()();
            Assert.IsTrue((bool)called);
        }
    }
}