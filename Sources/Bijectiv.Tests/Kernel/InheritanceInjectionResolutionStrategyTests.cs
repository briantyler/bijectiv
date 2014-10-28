// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritanceInjectionResolutionStrategyTests.cs" company="Bijectiv">
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
//   Defines the InheritanceInjectionResolutionStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InheritanceInjectionResolutionStrategy"/> class.
    /// </summary>
    [TestClass]
    public class InheritanceInjectionResolutionStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InheritanceInjectionResolutionStrategy().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose(null, TestClass1.T, new IInjection[0]);
            
            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose(TestClass1.T, null, new IInjection[0]);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Choose_CandidatesParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Choose<IInjection>(TestClass1.T, TestClass2.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesIsEmpty_ChoosesNull()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();

            // Act
            var result = testTarget.Choose(TestClass1.T, TestClass2.T, Ix());

            // Assert
            Assert.IsNull(result, Print(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsSingleExactMatch_ChoosesExact()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, TestClass2>();

            // Act
            var result = testTarget.Choose(TestClass1.T, TestClass2.T, Ix(injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsSingleSubMatch_ChoosesSub()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<object, TestClass2>();

            // Act
            var result = testTarget.Choose(TestClass1.T, TestClass2.T, Ix(injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsSingleSuperMatch_ChoosesSuper()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, DerivedTestClass1>();

            // Act
            var result = testTarget.Choose(TestClass1.T, BaseTestClass1.T, Ix(injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMultipleExactMatches_ChoosesLast()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, TestClass2>();

            // Act
            var result = testTarget.Choose(
                TestClass1.T,
                TestClass2.T,
                Ix(I<TestClass1, TestClass2>(), I<TestClass1, TestClass2>(), injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMultipleSubMatches_ChoosesLast()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<object, TestClass2>();

            // Act
            var result = testTarget.Choose(
                TestClass1.T,
                TestClass2.T,
                Ix(I<object, TestClass2>(), I<object, TestClass2>(), injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMultipleSuperMatches_ChoosesLast()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, DerivedTestClass1>();

            // Act
            var result = testTarget.Choose(
                TestClass1.T,
                BaseTestClass1.T,
                Ix(I<TestClass1, DerivedTestClass1>(), I<TestClass1, DerivedTestClass1>(), injection));

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMatches_ChoosesMostDerivedSourceLimitedBySourceType()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<MemberInfoHierarchy2, TestClass1>();
            var candidates = Ix(
                I<object, TestClass1>(),
                I<MemberInfoHierarchy1, TestClass1>(),
                I<MemberInfoHierarchy3, TestClass1>(),
                injection,
                I<object, TestClass1>(),
                I<MemberInfoHierarchy1, TestClass1>(),
                I<MemberInfoHierarchy3, TestClass1>());

            // Act
            var result = testTarget.Choose(typeof(MemberInfoHierarchy2), TestClass1.T, candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMatches_ChoosesMostDerivedTargetUnlimitedByTargetType()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, MemberInfoHierarchy3>();
            var candidates = Ix(
                I<TestClass1, object>(),
                I<TestClass1, MemberInfoHierarchy1>(),
                I<TestClass1, MemberInfoHierarchy2>(),
                injection,
                I<TestClass1, object>(),
                I<TestClass1, MemberInfoHierarchy1>(),
                I<TestClass1, MemberInfoHierarchy2>());

            // Act
            var result = testTarget.Choose(TestClass1.T, typeof(object), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_WhenMergeCandidatesContainsMatches_ChoosesMostDerivedTargetLimitedByTargetType()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = M<TestClass1, MemberInfoHierarchy2>();
            var candidates = Mx(
                M<TestClass1, object>(),
                M<TestClass1, MemberInfoHierarchy1>(),
                M<TestClass1, MemberInfoHierarchy3>(),
                injection,
                M<TestClass1, object>(),
                M<TestClass1, MemberInfoHierarchy1>(),
                M<TestClass1, MemberInfoHierarchy3>());

            // Act
            var result = testTarget.Choose(TestClass1.T, typeof(MemberInfoHierarchy2), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMultipleTargetHierarchyMatches_ChoosesMostDerivedInLastRegisteredHierarchy()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<TestClass1, DerivedTestClass1>();
            var candidates = Ix(
                I<TestClass1, object>(),
                I<TestClass1, MemberInfoHierarchy1>(),
                I<TestClass1, MemberInfoHierarchy2>(),
                injection,
                I<TestClass1, object>(),
                I<TestClass1, BaseTestClass1>());

            // Act
            var result = testTarget.Choose(TestClass1.T, typeof(object), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsMatches_ChoosesSourceTypeFirstThenTarget()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<DerivedTestClass1, MemberInfoHierarchy3>();
            var candidates = Ix(
                I<DerivedTestClass1, object>(),
                I<DerivedTestClass1, MemberInfoHierarchy1>(),
                I<DerivedTestClass1, MemberInfoHierarchy2>(),
                I<BaseTestClass1, object>(),
                I<BaseTestClass1, MemberInfoHierarchy1>(),
                I<BaseTestClass1, MemberInfoHierarchy2>(),
                I<BaseTestClass1, MemberInfoHierarchy3>(),
                injection,
                I<DerivedTestClass1, object>(),
                I<DerivedTestClass1, MemberInfoHierarchy1>(),
                I<DerivedTestClass1, MemberInfoHierarchy2>(),
                I<BaseTestClass1, object>(),
                I<BaseTestClass1, MemberInfoHierarchy1>(),
                I<BaseTestClass1, MemberInfoHierarchy2>(),
                I<BaseTestClass1, MemberInfoHierarchy3>());

            // Act
            var result = testTarget.Choose(DerivedTestClass1.T, typeof(MemberInfoHierarchy2), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsNoMatch_ChoosesNull()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var candidates = Ix(I<TestClass1, TestClass2>(), I<TestClass1, object>(), I<object, TestClass2>());

            // Act
            var result = testTarget.Choose(TestClass2.T, TestClass1.T, candidates);

            // Assert
            Assert.IsNull(result, Print(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsInterfaceMatchForConcrete_ChoosesNull()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var candidates = Ix(I<IMemberInfoHierarchy1, IMemberInfoHierarchy2>());

            // Act
            var result = testTarget.Choose(typeof(MemberInfoHierarchy1), typeof(MemberInfoHierarchy2), candidates);

            // Assert
            Assert.IsNull(result, Print(result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsInterfaceMatchForInterface_ChoosesInterface()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<IMemberInfoHierarchy1, IMemberInfoHierarchy2>();
            var candidates = Ix(injection);

            // Act
            var result = testTarget.Choose(typeof(IMemberInfoHierarchy1), typeof(IMemberInfoHierarchy2), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsInterfaceMatchForInterfaceSource_ChoosesInterface()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<MemberInfoHierarchy1, IMemberInfoHierarchy1>();
            var candidates = Ix(injection);

            // Act
            var result = testTarget.Choose(typeof(MemberInfoHierarchy2), typeof(IMemberInfoHierarchy1), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_CandidatesContainsInterfaceMatchForInterfaceTarget_ChoosesInterface()
        {
            // Arrange
            var testTarget = new InheritanceInjectionResolutionStrategy();
            var injection = I<IMemberInfoHierarchy1, MemberInfoHierarchy2>();
            var candidates = Ix(injection);

            // Act
            var result = testTarget.Choose(typeof(IMemberInfoHierarchy1), typeof(MemberInfoHierarchy1), candidates);

            // Assert
            Assert.AreEqual(injection, result, Print(injection, result));
        }

        private static IInjection I<TSource, TTarget>()
        {
            var injectionMock = new Mock<IInjection>(MockBehavior.Strict);
            
            injectionMock.SetupGet(_ => _.Source).Returns(typeof(TSource));
            injectionMock.SetupGet(_ => _.Target).Returns(typeof(TTarget));
            
            return injectionMock.Object;
        }

        private static IEnumerable<IInjection> Ix(params IInjection[] injections)
        {
            return injections;
        }

        private static IMerge M<TSource, TTarget>()
        {
            var injectionMock = new Mock<IMerge>(MockBehavior.Strict);

            injectionMock.SetupGet(_ => _.Source).Returns(typeof(TSource));
            injectionMock.SetupGet(_ => _.Target).Returns(typeof(TTarget));

            return injectionMock.Object;
        }

        private static IEnumerable<IMerge> Mx(params IMerge[] injections)
        {
            return injections;
        }

        private static string Print(params IInjection[] injections)
        {
            return string.Join(
                ", ", 
                injections
                    .Where(candidate => candidate != null)
                    .Select(item => string.Format("{0} --> {1}", item.Source, item.Target)));
        }
    }
}