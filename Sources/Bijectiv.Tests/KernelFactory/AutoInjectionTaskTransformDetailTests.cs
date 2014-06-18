// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionTaskTransformDetailTests.cs" company="Bijectiv">
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
//   Defines the AutoInjectionTaskTransformDetailTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="AutoInjectionTaskTransformDetail"/> class.
    /// </summary>
    [TestClass]
    public class AutoInjectionTaskTransformDetailTests
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
            new AutoInjectionTaskTransformDetail().Naught();

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
        public void CreateExpression_SourceIsValueTypePropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsValueTypeFieldToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { FieldInt = SourceInt };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldInt);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.PropertyInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsValueTypePropertyToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.FieldInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsValueTypeFieldToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { FieldInt = SourceInt };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldInt);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldInt);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(int), SourceInt, ExpectedInt);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedInt, targetInstance.FieldInt);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsNotValueTypePropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsNotValueTypeFieldToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { FieldBase = SourceBase };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldBase);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsNotValueTypePropertyToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertyBase = SourceBase };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsNotValueTypeFieldToField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { FieldBase = SourceBase };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldBase);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(DerivedTestClass1), typeof(BaseTestClass1), SourceBase, ExpectedBase);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_SourceIsSealedPropertyToProperty_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertySealed = SourceSealed };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertySealed);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertySealed);

            var injectionContext = CreateInjectionContext(
                repository, typeof(SealedClass1), typeof(SealedClass1), SourceSealed, ExpectedSealed);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedSealed, targetInstance.PropertySealed);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateExpression_IntPropertyDerivedField_Injects()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new AutoInjectionTestClass1 { PropertyInt = SourceInt };
            var targetInstance = new AutoInjectionTestClass1();

            var sourceMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var targetMember = (MemberInfo)Reflect<AutoInjectionTestClass1>.Field(_ => _.FieldBase);

            var injectionContext = CreateInjectionContext(
                repository, typeof(int), typeof(BaseTestClass1), SourceInt, ExpectedBase);

            var scaffold = CreateScaffold(
                repository, injectionContext, sourceInstance, targetInstance);

            var target = CreateTarget();

            // Act
            var result  = target.CreateExpression(scaffold, sourceMember, targetMember);

            // Assert
            Expression.Lambda<Action>(result).Compile()();
            Assert.AreEqual(ExpectedBase, targetInstance.FieldBase);
        }

        private static IInjectionContext CreateInjectionContext(
            MockRepository repository, Type sourceMember, Type targetMember, object sourceValue, object targetValue)
        {
            var transformMock = repository.Create<ITransform>();

            var injectionStoreMock = repository.Create<IInjectionStore>();
            injectionStoreMock
                .Setup(_ => _.Resolve<ITransform>(sourceMember, targetMember))
                .Returns(transformMock.Object);

            var injectionContextMock = repository.Create<IInjectionContext>();
            injectionContextMock.SetupGet(_ => _.InjectionStore).Returns(injectionStoreMock.Object);

            transformMock
                .Setup(_ => _.Transform(sourceValue, injectionContextMock.Object, null))
                .Returns(targetValue);

            return injectionContextMock.Object;
        }

        private static InjectionScaffold CreateScaffold(
            MockRepository repository,
            IInjectionContext injectionContext,
            AutoInjectionTestClass1 sourceInstance,
            AutoInjectionTestClass1 targetInstance)
        {
            var scaffoldMock = repository.Create<InjectionScaffold>();

            scaffoldMock.SetupGet(_ => _.InjectionContext).Returns(Expression.Constant(injectionContext));
            scaffoldMock.SetupGet(_ => _.Source).Returns(Expression.Constant(sourceInstance));
            scaffoldMock.SetupGet(_ => _.Target).Returns(Expression.Constant(targetInstance));

            return scaffoldMock.Object;
        }

        private static AutoInjectionTaskTransformDetail CreateTarget()
        {
            return new AutoInjectionTaskTransformDetail();
        }
    }
}