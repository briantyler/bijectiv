// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskMergeDetailTests.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskMergeDetailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.InjectionFactory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.InjectionFactory;
    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AutoInjectionTaskMergeDetail"/> class.
    /// </summary>
    [TestClass]
    public class AutoInjectionTaskMergeDetailTests
    {
        private const int SourceInt = 17;

        private const int ExpectedInt = 34;

        private static readonly DerivedTestClass1 SourceBase = new DerivedTestClass1();

        private static readonly DerivedTestClass1 ExpectedBase = new DerivedTestClass1();

        private static readonly SealedClass1 SourceSealed = new SealedClass1();

        private static readonly SealedClass1 ExpectedSealed = new SealedClass1();

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AutoInjectionTaskMergeDetail().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateExpression_ScaffoldParameterNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateExpression(null, Stub.Create<MemberInfo>(), Stub.Create<MemberInfo>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateExpression_SourceMemberParameterNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateExpression(Stub.Create<InjectionScaffold>(), null, Stub.Create<MemberInfo>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateExpression_TargetMemberParameterNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.CreateExpression(Stub.Create<InjectionScaffold>(), Stub.Create<MemberInfo>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_IntToIntReplace_ReplacesTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1 { PropertyInt = 123 };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository, 
                typeof(int), 
                typeof(int), 
                SourceInt, 
                123, 
                new MergeResult(PostMergeAction.Replace, ExpectedInt));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_IntToIntNone_DoesNothingToTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoTransformTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1 { PropertyInt = ExpectedInt };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(int),
                typeof(int),
                SourceInt,
                ExpectedInt,
                new MergeResult(PostMergeAction.None, 123));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SealedToSealedReplace_ReplacesTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetMemberInstance = new SealedClass1();
            var sourceInstance = new AutoTransformTestClass1 { PropertySealed = SourceSealed };
            var targetInstance = new AutoTransformTestClass1 { PropertySealed = targetMemberInstance };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(SealedClass1),
                typeof(SealedClass1),
                SourceSealed,
                targetMemberInstance,
                new MergeResult(PostMergeAction.Replace, ExpectedSealed));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(ExpectedSealed, targetInstance.PropertySealed);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SealedToSealedNone_DoesNothingToTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetMemberInstance = new SealedClass1();
            var sourceInstance = new AutoTransformTestClass1 { PropertySealed = SourceSealed };
            var targetInstance = new AutoTransformTestClass1 { PropertySealed = targetMemberInstance };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertySealed);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(SealedClass1),
                typeof(SealedClass1),
                SourceSealed,
                targetMemberInstance,
                new MergeResult(PostMergeAction.None, ExpectedSealed));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(targetMemberInstance, targetInstance.PropertySealed);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_ClassToClassReplace_ReplacesTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetMemberInstance = new DerivedTestClass1();
            var sourceInstance = new AutoTransformTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1 { PropertyBase = targetMemberInstance };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass1),
                SourceBase,
                targetMemberInstance,
                new MergeResult(PostMergeAction.Replace, ExpectedBase));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(ExpectedBase, targetInstance.PropertyBase);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_ClassToClassNone_DoesNothingToTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetMemberInstance = new DerivedTestClass1();
            var sourceInstance = new AutoTransformTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoTransformTestClass1 { PropertyBase = targetMemberInstance };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass1),
                SourceBase,
                targetMemberInstance,
                new MergeResult(PostMergeAction.None, ExpectedBase));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(targetMemberInstance, targetInstance.PropertyBase);

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_IntToClassReplace_ReplacesTarget()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetMemberInstance = new DerivedTestClass1();
            var sourceInstance = new AutoTransformTestClass1 { FieldInt = SourceInt };
            var targetInstance = new AutoTransformTestClass1 { FieldBase = targetMemberInstance };

            var sourceMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldInt);
            var targetMember = (MemberInfo)Reflect<AutoTransformTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository,
                typeof(int),
                typeof(DerivedTestClass1),
                SourceInt,
                targetMemberInstance,
                new MergeResult(PostMergeAction.Replace, ExpectedBase));

            var targetInstanceParameter = Expression.Parameter(typeof(AutoTransformTestClass1));
            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstanceParameter);

            var target = CreateTarget();

            // Act
            var result = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression
                .Lambda<Action<AutoTransformTestClass1>>(result, targetInstanceParameter)
                .Compile()(targetInstance);
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);

            repository.VerifyAll();
        }

        private static InjectionScaffold CreateScaffold(
            MockRepository repository,
            IInjectionContext injectionContext,
            AutoTransformTestClass1 sourceInstance,
            ParameterExpression targetInstanceParameter)
        {
            var scaffoldMock = repository.Create<InjectionScaffold>();

            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(injectionContext));
            scaffoldMock.SetupGet(_ => _.Source).Returns(Expression.Constant(sourceInstance));
            scaffoldMock.SetupGet(_ => _.Target).Returns(targetInstanceParameter);

            return scaffoldMock.Object;
        }

        private static IInjectionContext CreateInjectionContext(
            MockRepository repository, 
            Type sourceMember, 
            Type targetMember, 
            object sourceValue, 
            object targetValue,
            IMergeResult result)
        {
            var transformMock = repository.Create<IMerge>();

            var injectionStoreMock = repository.Create<IInjectionStore>();
            injectionStoreMock
                .Setup(_ => _.Resolve<IMerge>(sourceMember, targetMember))
                .Returns(transformMock.Object);

            var injectionContextMock = repository.Create<IInjectionContext>();
            injectionContextMock.SetupGet(_ => _.InjectionStore).Returns(injectionStoreMock.Object);

            transformMock
                .Setup(_ => _.Merge(sourceValue, targetValue, injectionContextMock.Object))
                .Returns(result);

            return injectionContextMock.Object;
        }

        private static AutoInjectionTaskMergeDetail CreateTarget()
        {
            return new AutoInjectionTaskMergeDetail();
        }
    }
}