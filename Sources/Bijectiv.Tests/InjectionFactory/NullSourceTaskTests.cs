// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSourceTaskTests.cs" company="Bijectiv">
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
//   Defines the NullSourceTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.InjectionFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.InjectionFactory;
    using Bijectiv.KernelBuilder;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="NullSourceTask"/> class.
    /// </summary>
    [TestClass]
    public class NullSourceTaskTests
    {
        private static readonly TestClass2 DummyTarget = new TestClass2();

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NullSourceTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new NullSourceTask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_NoNullSourceUnprocessedFragmentsHasProcessedFragments_DoesNothing()
        {
            // Arrange
            var scaffold = CreateScaffold();
            scaffold.ProcessedFragments.Add(
                Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.NullSource));

            var target = CreateTarget();

            // Act
            target.Execute(scaffold);

            // Assert
            Assert.IsFalse(scaffold.Expressions.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_NoNullSourceUnprocessedFragments_CreatesAndReturnsDefaultTarget()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            // Act
            target.Execute(scaffold);
            
            // Assert
            var @delegate = CreateDelegate(scaffold);

            Assert.AreEqual(default(object), @delegate(Stub.Create<IInjectionContext>(), null));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_NoNullSourceUnprocessedFragments_IgnoresWhenSourceNotNull()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            // Act
            target.Execute(scaffold);

            // Assert
            var @delegate = CreateDelegate(scaffold);

            Assert.AreEqual(DummyTarget, @delegate(Stub.Create<IInjectionContext>(), new TestClass1()));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_HasSourceUnprocessedFragments_CreatesTargetFromFragmentDelegate()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            var expected = new TestClass2();
            scaffold.CandidateFragments.Add(
                new NullSourceFragment(
                    TestClass1.T, 
                    TestClass2.T,
                    new Func<IInjectionContext, TestClass2>(c => expected)));

            // Act
            target.Execute(scaffold);

            // Assert
            var @delegate = CreateDelegate(scaffold);

            Assert.AreEqual(expected, @delegate(Stub.Create<IInjectionContext>(), null));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_HasSourceUnprocessedFragments_IgnoresWhenSourceNotNull()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            var unexpected = new TestClass2();
            scaffold.CandidateFragments.Add(
                new NullSourceFragment(
                    TestClass1.T,
                    TestClass2.T,
                    new Func<IInjectionContext, TestClass2>(c => unexpected)));

            // Act
            target.Execute(scaffold);

            // Assert
            var @delegate = CreateDelegate(scaffold);

            Assert.AreEqual(DummyTarget, @delegate(Stub.Create<IInjectionContext>(), new TestClass1()));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_HasSourceUnprocessedFragments_TakesFirstFragment()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            var expected = new TestClass2();
            scaffold.CandidateFragments.Add(
                new NullSourceFragment(
                    TestClass1.T,
                    TestClass2.T,
                    new Func<IInjectionContext, TestClass2>(c => expected)));
            scaffold.CandidateFragments.Add(
                new NullSourceFragment(
                    TestClass1.T,
                    TestClass2.T,
                    new Func<IInjectionContext, TestClass2>(c => { throw new Exception(); })));

            // Act
            target.Execute(scaffold);

            // Assert
            var @delegate = CreateDelegate(scaffold);

            Assert.AreEqual(expected, @delegate(Stub.Create<IInjectionContext>(), null));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_HasSourceUnprocessedFragments_ProcessesAllFragments()
        {
            // Arrange
            var scaffold = CreateScaffold();
            var target = CreateTarget();

            var expected = new TestClass2();
            scaffold.CandidateFragments.Add(
                new NullSourceFragment(
                    TestClass1.T,
                    TestClass2.T,
                    new Func<IInjectionContext, TestClass2>(c => expected)));
            scaffold.CandidateFragments.Add(
                Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.NullSource));
            scaffold.CandidateFragments.Add(
                Stub.Fragment<TestClass1, TestClass2>(false, LegendryFragments.NullSource));

            // Act
            target.Execute(scaffold);

            // Assert
            Assert.AreEqual(3, scaffold.ProcessedFragments.Count());
        }

        private static Func<IInjectionContext, object, object> CreateDelegate(InjectionScaffold scaffold)
        {
            var assignDummy = Expression.Assign(
                scaffold.TargetAsObject, 
                Expression.Constant(DummyTarget, typeof(object)));
            scaffold.Expressions.Add(assignDummy);

            new ReturnTargetAsObjectTask().Execute(scaffold);

            return
                Expression.Lambda<Func<IInjectionContext, object, object>>(
                    Expression.Block(
                        new[]
                        {
                            (ParameterExpression)scaffold.TargetAsObject, 
                            (ParameterExpression)scaffold.Target
                        },
                        scaffold.Expressions),
                    (ParameterExpression)scaffold.InjectionContext,
                    (ParameterExpression)scaffold.SourceAsObject).Compile();
        }

        private static NullSourceTask CreateTarget()
        {
            return new NullSourceTask();
        }

        private static InjectionScaffold CreateScaffold()
        {
            return new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(IInjectionContext)))
            {
                Target = Expression.Variable(TestClass2.T),
                TargetAsObject = Expression.Variable(typeof(object))
            };
        }
    }
}