// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionMergerTests.cs" company="Bijectiv">
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
//   Defines the CollectionMergerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CollectionMerger"/> class.
    /// </summary>
    [TestClass]
    public class CollectionMergerTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CollectionMerger().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeNonGeneric_ValidParameters_CallsGenericOverload()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTargetMock = repository.Create<CollectionMerger>();
            var target = Stub.Create<ICollection<TestClass1>>();
            var context = Stub.Create<IInjectionContext>();
            var sourceSequence = new object[] { 1, 2, 3 };

            testTargetMock
                .Setup(_ => _.Merge(
                    It.Is<IEnumerable<object>>(p => sourceSequence.SequenceEqual(p)), 
                    target, 
                    context));

            var testTarget = testTargetMock.Object;

            // Act
            testTarget.Merge(new ArrayList(sourceSequence), target, context);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeNonGeneric_SourceParameterIsNull_CallsGenericOverload()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTargetMock = repository.Create<CollectionMerger>();
            var target = Stub.Create<ICollection<TestClass1>>();
            var context = Stub.Create<IInjectionContext>();

            testTargetMock.Setup(_ => _.Merge(It.IsAny<IEnumerable<object>>(), target, context));

            var testTarget = testTargetMock.Object;

            // Act
             testTarget.Merge(null, target, context);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MergeGeneric_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Merge(
                // ReSharper disable once AssignNullToNotNullAttribute
                default(IEnumerable<TestClass1>), 
                Stub.Create<ICollection<TestClass1>>(), 
                Stub.Create<IInjectionContext>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MergeGeneric_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Merge(
                // ReSharper disable once AssignNullToNotNullAttribute
                Stub.Create<IEnumerable<TestClass1>>(),
                default(ICollection<TestClass1>),
                Stub.Create<IInjectionContext>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MergeGeneric_ContextParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            testTarget.Merge(
                // ReSharper disable once AssignNullToNotNullAttribute
                Stub.Create<IEnumerable<TestClass1>>(),
                Stub.Create<ICollection<TestClass1>>(),
                null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeGeneric_ValueToValue_TransformsEachElement()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTarget = CreateTestTarget();
            var transformMock = repository.Create<ITransform>();
            var storeMock = repository.Create<IInjectionStore>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(typeof(int), typeof(int))).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(1, contextMock.Object, It.IsAny<object>())).Returns(2);
            transformMock.Setup(_ => _.Transform(2, contextMock.Object, It.IsAny<object>())).Returns(4);
            transformMock.Setup(_ => _.Transform(3, contextMock.Object, It.IsAny<object>())).Returns(6);

            var source = new[] { 1, 2, 3 };
            var target = new List<int> { 12, 59, 99, 21 };

            // Act
            testTarget.Merge(source, target, contextMock.Object);

            // Assert
            repository.VerifyAll();
            new[] { 2, 4, 6 }.AssertSequenceEqual(target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeGeneric_ValueToValue_ProvidesEnumerableInjectionHint()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTarget = CreateTestTarget();
            var transformMock = repository.Create<ITransform>();
            var storeMock = repository.Create<IInjectionStore>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(typeof(int), typeof(int))).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(1, contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 0))).Returns(2);
            transformMock.Setup(_ => _.Transform(2, contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 1))).Returns(4);
            transformMock.Setup(_ => _.Transform(3, contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 2))).Returns(6);

            var source = new[] { 1, 2, 3 };
            var target = new List<int> { 12, 59, 99, 21 };

            // Act
            testTarget.Merge(source, target, contextMock.Object);

            // Assert
            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeGeneric_ClassToClass_TransformsWhereNotFoundAndMergesWhereFound()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTarget = CreateTestTarget();
            var finderMock = repository.Create<ITargetFinder>();
            var finderStoreMock = repository.Create<ITargetFinderStore>();
            var baseTransformMock = repository.Create<ITransform>();
            var derivedTransformMock = repository.Create<ITransform>();
            var baseMergeMock = repository.Create<IMerge>();
            var derivedMergeMock = repository.Create<IMerge>();
            var storeMock = repository.Create<IInjectionStore>();
            var registryMock = repository.Create<IInstanceRegistry>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock
                .Setup(_ => _.Resolve<ITransform>(BaseTestClass1.T, BaseTestClass2.T))
                .Returns(baseTransformMock.Object);
            storeMock
                .Setup(_ => _.Resolve<ITransform>(DerivedTestClass1.T, BaseTestClass2.T))
                .Returns(derivedTransformMock.Object);
            storeMock
                .Setup(_ => _.Resolve<IMerge>(BaseTestClass1.T, BaseTestClass2.T))
                .Returns(baseMergeMock.Object);
            storeMock
                .Setup(_ => _.Resolve<IMerge>(DerivedTestClass1.T, DerivedTestClass2.T))
                .Returns(derivedMergeMock.Object);

            registryMock.Setup(_ => _.Resolve<ITargetFinderStore>()).Returns(finderStoreMock.Object);
            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);
            finderStoreMock.Setup(_ => _.Resolve(BaseTestClass1.T, BaseTestClass2.T)).Returns(finderMock.Object);

            var source = new[]
            {
                new BaseTestClass1(), new DerivedTestClass1(), null, 
                new BaseTestClass1(), new DerivedTestClass1()
            };

            var targetExpected = new object[]
            {
                new BaseTestClass2(), new DerivedTestClass2(), null, 
                new BaseTestClass2(), new DerivedTestClass2() 
            };

            var target = new List<BaseTestClass2> { new BaseTestClass2(), new DerivedTestClass2() };

            finderMock.Setup(_ => _.Initialize(target, contextMock.Object));

            object dummy;
            finderMock.Setup(_ => _.TryFind(source[0], out dummy)).Returns(false);
            finderMock.Setup(_ => _.TryFind(source[1], out dummy)).Returns(false);
            finderMock.Setup(_ => _.TryFind(source[2], out dummy)).Returns(false);

            baseTransformMock
                .Setup(_ => _.Transform(source[0], contextMock.Object, It.IsAny<object>()))
                .Returns(targetExpected[0]);
            derivedTransformMock
                .Setup(_ => _.Transform(source[1], contextMock.Object, It.IsAny<object>()))
                .Returns(targetExpected[1]);
            baseTransformMock
                .Setup(_ => _.Transform(source[2], contextMock.Object, It.IsAny<object>()))
                .Returns(targetExpected[2]);

            finderMock.Setup(_ => _.TryFind(source[3], out targetExpected[3])).Returns(true);
            finderMock.Setup(_ => _.TryFind(source[4], out targetExpected[4])).Returns(true);
            baseMergeMock
                .Setup(_ => _.Merge(source[3], targetExpected[3], contextMock.Object, It.IsAny<object>()))
                .Returns(new MergeResult(PostMergeAction.None, targetExpected[3]));
            derivedMergeMock
                .Setup(_ => _.Merge(source[4], targetExpected[4], contextMock.Object, It.IsAny<object>()))
                .Returns(new MergeResult(PostMergeAction.None, targetExpected[4]));

            // Act
            testTarget.Merge(source, target, contextMock.Object);

            // Assert
            repository.VerifyAll();
            targetExpected.AssertSequenceEqual(target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeGeneric_ClassToClass_ProvidesEnumerableInjectionHint()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTarget = CreateTestTarget();
            var finderMock = repository.Create<ITargetFinder>();
            var finderStoreMock = repository.Create<ITargetFinderStore>();
            var transformMock = repository.Create<ITransform>();
            var mergeMock = repository.Create<IMerge>();
            var storeMock = repository.Create<IInjectionStore>();
            var registryMock = repository.Create<IInstanceRegistry>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock
                .Setup(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T))
                .Returns(transformMock.Object);
            storeMock
                .Setup(_ => _.Resolve<IMerge>(TestClass1.T, TestClass2.T))
                .Returns(mergeMock.Object);

            registryMock.Setup(_ => _.Resolve<ITargetFinderStore>()).Returns(finderStoreMock.Object);
            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);
            finderStoreMock.Setup(_ => _.Resolve(TestClass1.T, TestClass2.T)).Returns(finderMock.Object);

            var source = new[]
            {
                new TestClass1(), null, new TestClass1(),
                new TestClass1(), new TestClass1()
            };

            var targetExpected = new object[]
            {
                new TestClass2(), null, new TestClass2(),
                new TestClass2(), new TestClass2()
            };

            var target = new List<TestClass2> { new TestClass2(), new TestClass2() };

            finderMock.Setup(_ => _.Initialize(target, contextMock.Object));

            object dummy;
            finderMock.Setup(_ => _.TryFind(source[0], out dummy)).Returns(false);
            finderMock.Setup(_ => _.TryFind(source[1], out dummy)).Returns(false);
            finderMock.Setup(_ => _.TryFind(source[2], out dummy)).Returns(false);

            transformMock
                .Setup(_ => _.Transform(source[0], contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 0)))
                .Returns(targetExpected[0]);
            transformMock
                .Setup(_ => _.Transform(source[1], contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 1)))
                .Returns(targetExpected[1]);
            transformMock
                .Setup(_ => _.Transform(source[2], contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 2)))
                .Returns(targetExpected[2]);

            finderMock.Setup(_ => _.TryFind(source[3], out targetExpected[3])).Returns(true);
            finderMock.Setup(_ => _.TryFind(source[4], out targetExpected[4])).Returns(true);
            mergeMock
                .Setup(_ => _.Merge(source[3], targetExpected[3], contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 3)))
                .Returns(new MergeResult(PostMergeAction.None, targetExpected[3]));
            mergeMock
                .Setup(_ => _.Merge(source[4], targetExpected[4], contextMock.Object, It.Is<EnumerableInjectionHint>(h => h.Index == 4)))
                .Returns(new MergeResult(PostMergeAction.None, targetExpected[4]));

            // Act
            testTarget.Merge(source, target, contextMock.Object);

            // Assert
            repository.VerifyAll();
            targetExpected.AssertSequenceEqual(target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeGeneric_ClassNullToClassNullFound_Merges()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict) { CallBase = false };
            var testTarget = CreateTestTarget();
            var finderMock = repository.Create<ITargetFinder>();
            var finderStoreMock = repository.Create<ITargetFinderStore>();
            var baseMergeMock = repository.Create<IMerge>();
            var storeMock = repository.Create<IInjectionStore>();
            var registryMock = repository.Create<IInstanceRegistry>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock
                .Setup(_ => _.Resolve<IMerge>(BaseTestClass1.T, BaseTestClass2.T))
                .Returns(baseMergeMock.Object);

            registryMock.Setup(_ => _.Resolve<ITargetFinderStore>()).Returns(finderStoreMock.Object);
            contextMock.SetupGet(_ => _.InstanceRegistry).Returns(registryMock.Object);
            finderStoreMock.Setup(_ => _.Resolve(BaseTestClass1.T, BaseTestClass2.T)).Returns(finderMock.Object);

            var source = new BaseTestClass1[] { null };
            var targetExpected = new object[] { null };

            var target = new List<BaseTestClass2> { new BaseTestClass2(), new DerivedTestClass2() };

            finderMock.Setup(_ => _.Initialize(target, contextMock.Object));

            finderMock.Setup(_ => _.TryFind(source[0], out targetExpected[0])).Returns(true);
            baseMergeMock
                .Setup(_ => _.Merge(source[0], targetExpected[0], contextMock.Object, It.IsAny<object>()))
                .Returns(new MergeResult(PostMergeAction.None, targetExpected[0]));

            // Act
            testTarget.Merge(source, target, contextMock.Object);
            
            // Assert
            repository.VerifyAll();
            targetExpected.AssertSequenceEqual(target);
        }

        private static CollectionMerger CreateTestTarget()
        {
            return new CollectionMerger();
        }
    }
}