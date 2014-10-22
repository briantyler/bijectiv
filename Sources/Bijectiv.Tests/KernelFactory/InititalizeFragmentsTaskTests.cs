// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InititalizeFragmentsTaskTests.cs" company="Bijectiv">
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
//   Defines the InititalizeFragmentsTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InitializeFragmentsTask"/> class.
    /// </summary>
    [TestClass]
    public class InititalizeFragmentsTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InitializeFragmentsTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ScaffoldContainsCandidateFragments_CandidateFragmentsCleared()
        {
            // Arrange
            var scaffold = CreateScaffold(new InstanceRegistry());
            scaffold.CandidateFragments.Add(Stub.Fragment<object, object>());
            
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.IsFalse(scaffold.CandidateFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ScaffoldContainsProcessedFragments_ProcessedFragmentsCleared()
        {
            // Arrange
            var scaffold = CreateScaffold(new InstanceRegistry());
            scaffold.ProcessedFragments.Add(Stub.Fragment<object, object>());

            var testTarget = CreateTestTarget();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.IsFalse(scaffold.ProcessedFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DefinitionFragments_AddedToScaffoldCandidatesInReverseOrder()
        {
            // Arrange
            var fragments = new[]
            {
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>()
            };

            var scaffold = CreateScaffold(new InstanceRegistry(), fragments);
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(3, scaffold.CandidateFragments.Count());
            Assert.AreEqual(fragments[2], scaffold.CandidateFragments.ElementAt(0));
            Assert.AreEqual(fragments[1], scaffold.CandidateFragments.ElementAt(1));
            Assert.AreEqual(fragments[0], scaffold.CandidateFragments.ElementAt(2));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_DefinitionFragmentsOfInheritedDefinitions_AddedToScaffoldCandidatesInReverseOrder()
        {
            // Arrange
            var fragments = new[]
            {
                Stub.Fragment<DerivedTestClass1, DerivedTestClass2>(),
                new InheritsFragment(DerivedTestClass1.T, DerivedTestClass2.T, typeof(object), BaseTestClass2.T), 
                Stub.Fragment<DerivedTestClass1, DerivedTestClass2>(),
                new InheritsFragment(DerivedTestClass1.T, DerivedTestClass2.T, BaseTestClass1.T, BaseTestClass2.T) 
            };

            var baseDefinition1 = new InjectionDefinition(BaseTestClass1.T, BaseTestClass2.T)
            {
                Stub.Fragment<BaseTestClass1, BaseTestClass2>(),
                new InheritsFragment(BaseTestClass1.T, BaseTestClass2.T, typeof(object), BaseTestClass2.T), 
                Stub.Fragment<BaseTestClass1, BaseTestClass2>(),
            };

            var baseDefinition2 = new InjectionDefinition(BaseTestClass1.T, BaseTestClass2.T)
            {
                Stub.Fragment<BaseTestClass1, BaseTestClass2>(),
                new InheritsFragment(BaseTestClass1.T, BaseTestClass2.T, typeof(object), BaseTestClass2.T), 
                Stub.Fragment<BaseTestClass1, BaseTestClass2>(),
            };

            var notUsedDefinition = new InjectionDefinition(typeof(object), typeof(object))
            {
                Stub.Fragment<object, object>(),
            };

            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            registryMock
                .Setup(_ => _.ResolveAll<InjectionDefinition>())
                .Returns(new[] { baseDefinition1, baseDefinition2, notUsedDefinition });

            var scaffold = CreateScaffold(registryMock.Object, fragments);
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Execute(scaffold);

            // Assert
            Assert.AreEqual(7, scaffold.CandidateFragments.Count());
            Assert.AreEqual(fragments[3], scaffold.CandidateFragments.ElementAt(0));
            Assert.AreEqual(fragments[2], scaffold.CandidateFragments.ElementAt(1));
            Assert.AreEqual(fragments[1], scaffold.CandidateFragments.ElementAt(2));
            Assert.AreEqual(fragments[0], scaffold.CandidateFragments.ElementAt(3));
            Assert.AreEqual(baseDefinition2.ElementAt(2), scaffold.CandidateFragments.ElementAt(4));
            Assert.AreEqual(baseDefinition2.ElementAt(1), scaffold.CandidateFragments.ElementAt(5));
            Assert.AreEqual(baseDefinition2.ElementAt(0), scaffold.CandidateFragments.ElementAt(6));

            Assert.AreEqual(3, scaffold.ProcessedFragments.Count());
            Assert.IsTrue(scaffold.ProcessedFragments.Contains(fragments[1]));
            Assert.IsTrue(scaffold.ProcessedFragments.Contains(fragments[3]));
            Assert.IsTrue(scaffold.ProcessedFragments.Contains(baseDefinition2.ElementAt(1)));
        }

        private static InitializeFragmentsTask CreateTestTarget()
        {
            return new InitializeFragmentsTask();
        }

        private static InjectionScaffold CreateScaffold(
            IInstanceRegistry registry,
            params InjectionFragment[] fragments)
        {
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T, fragments);

            return new InjectionScaffold(
                registry,
                definition,
                Expression.Parameter(typeof(object)),
                Expression.Parameter(typeof(IInjectionContext)));
        }
    }
}