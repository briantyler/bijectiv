// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMemberInjectorTests.cs" company="Bijectiv">
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
//   Defines the TargetMemberInjectorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TargetMemberInjector"/> class.
    /// </summary>
    [TestClass]
    public class TargetMemberInjectorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TargetMemberInjector().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_ScaffoldIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddTransformExpressionToScaffold(null, Stub.Create<MemberInfo>(), Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_MemberIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddTransformExpressionToScaffold(Stub.Create<InjectionScaffold>(), null, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_ExpressionIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddTransformExpressionToScaffold(Stub.Create<InjectionScaffold>(), Stub.Create<MemberInfo>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_MemberIsReadOnly_Merges()
        {
            // Arrange
            var targetMock = new Mock<TargetMemberTransformInjector> { CallBase = true };

            var memberMock = new Mock<PropertyInfo>();
            memberMock.SetupGet(_ => _.CanRead).Returns(true);
            memberMock.SetupGet(_ => _.CanWrite).Returns(false);

            var scaffold = Stub.Create<InjectionScaffold>();
            var sourceExpression = Expression.Empty();

            // Act
            targetMock.Object.AddTransformExpressionToScaffold(scaffold, memberMock.Object, sourceExpression);

            // Assert
            targetMock.Verify(_ => _.AddMergeExpressionToScaffold(scaffold, memberMock.Object, sourceExpression));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void AddTransformExpressionToScaffold_MemberIsHidden_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();
            var memberMock = new Mock<PropertyInfo>();
            memberMock.SetupGet(_ => _.CanRead).Returns(false);
            memberMock.SetupGet(_ => _.CanWrite).Returns(false);

            // Act
            testTarget.AddTransformExpressionToScaffold(
                Stub.Create<InjectionScaffold>(), memberMock.Object, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_SourceValue_TransformsMember()
        {
            // Arrange
            var target = new TestClass1();
            const int SourceInstance = 123;
            var member = Reflect<TestClass1>.Property(_ => _.Id);
            const string TransformResult = "bijectiv";

            var scaffold = CreateScaffoldForTransform(target, SourceInstance, member, TransformResult);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddTransformExpressionToScaffold(scaffold, member, Expression.Constant(SourceInstance));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            Assert.AreEqual(TransformResult, target.Id);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_SourceSealed_TransformsMember()
        {
            // Arrange
            var target = new TestClass1();
            var source = new SealedClass1();
            var member = Reflect<TestClass1>.Property(_ => _.Id);
            const string TransformResult = "bijectiv";

            var scaffold = CreateScaffoldForTransform(target, source, member, TransformResult);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddTransformExpressionToScaffold(scaffold, member, Expression.Constant(source));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            Assert.AreEqual(TransformResult, target.Id);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_SourceDerived_TransformsMember()
        {
            // Arrange
            var target = new TestClass1();
            var source = new DerivedTestClass1();
            var member = Reflect<TestClass1>.Property(_ => _.Id);
            const string TransformResult = "bijectiv";

            var scaffold = CreateScaffoldForTransform(target, source, member, TransformResult);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddTransformExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            Assert.AreEqual(TransformResult, target.Id);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_SourceNull_TransformsMember()
        {
            // Arrange
            var target = new TestClass1();
            var member = Reflect<TestClass1>.Property(_ => _.Id);
            const string TransformResult = "bijectiv";

            var scaffold = CreateScaffoldForTransform<TestClass2>(target, null, member, TransformResult);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddTransformExpressionToScaffold(scaffold, member, Expression.Constant(null, typeof(TestClass2)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            Assert.AreEqual(TransformResult, target.Id);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_ScaffoldIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddMergeExpressionToScaffold(null, Stub.Create<MemberInfo>(), Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_MemberIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddMergeExpressionToScaffold(Stub.Create<InjectionScaffold>(), null, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_ExpressionIsNull_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AddMergeExpressionToScaffold(Stub.Create<InjectionScaffold>(), Stub.Create<MemberInfo>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_MemberIsReadOnly_Merges()
        {
            // Arrange
            var targetMock = new Mock<TargetMemberMergeInjector> { CallBase = true };

            var memberMock = new Mock<PropertyInfo>();
            memberMock.SetupGet(_ => _.CanRead).Returns(false);
            memberMock.SetupGet(_ => _.CanWrite).Returns(true);

            var scaffold = Stub.Create<InjectionScaffold>();
            var sourceExpression = Expression.Empty();

            // Act
            targetMock.Object.AddMergeExpressionToScaffold(scaffold, memberMock.Object, sourceExpression);

            // Assert
            targetMock.Verify(_ => _.AddTransformExpressionToScaffold(scaffold, memberMock.Object, sourceExpression));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void AddMergeExpressionToScaffold_MemberIsHidden_Throws()
        {
            // Arrange
            var testTarget = new TargetMemberInjector();
            var memberMock = new Mock<PropertyInfo>();
            memberMock.SetupGet(_ => _.CanRead).Returns(false);
            memberMock.SetupGet(_ => _.CanWrite).Returns(false);

            // Act
            testTarget.AddMergeExpressionToScaffold(
                Stub.Create<InjectionScaffold>(), memberMock.Object, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_SourceValue_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyInt = 31 };
            const int SourceInstance = 123;
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, SourceInstance, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(scaffold, member, Expression.Constant(SourceInstance));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_SourceSealed_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyInt = 31 };
            const string SourceInstance = "bijectiv";
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, SourceInstance, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(scaffold, member, Expression.Constant(SourceInstance));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_SourceDerived_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyInt = 31 };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_TargetValue_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyInt = 31 };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyInt);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_TargetSealed_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertySealed = new SealedClass1() };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertySealed);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_TargetDerived_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyBase = new DerivedTestClass1() };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_ReplaceActionNone_MergesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyBase = new DerivedTestClass1() };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var mergeResult = new MergeResult(PostMergeAction.None, null);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_ReplaceActionReplaceWritableMember_ReplacesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyBase = new DerivedTestClass1() };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var replacedMember = new BaseTestClass1();
            var mergeResult = new MergeResult(PostMergeAction.Replace, replacedMember);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
            Assert.AreEqual(replacedMember, target.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_ReplaceActionReplaceWritableDerivedMember_ReplacesMember()
        {
            // Arrange
            var target = new AutoInjectionTestClass1 { PropertyBase = new DerivedTestClass1() };
            var source = new DerivedTestClass1();
            var member = Reflect<AutoInjectionTestClass1>.Property(_ => _.PropertyBase);
            var replacedMember = new DerivedTestClass1();
            var mergeResult = new MergeResult(PostMergeAction.Replace, replacedMember);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();

            repository.VerifyAll();
            Assert.AreEqual(replacedMember, target.PropertyBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void AddMergeExpressionToScaffold_ReplaceActionReplaceReadonlyMember_Throws()
        {
            // Arrange
            var target = new PropertyTestClass();
            var source = new DerivedTestClass1();
            var member = PropertyTestClass.ReadonlyPropertyPi;
            const int ReplacedMember = 897;
            var mergeResult = new MergeResult(PostMergeAction.Replace, ReplacedMember);

            MockRepository repository;
            var scaffold = CreateScaffoldForMerge(target, source, member, mergeResult, out repository);

            var testTarget = new TargetMemberInjector();

            // Act
            testTarget.AddMergeExpressionToScaffold(
                scaffold, member, Expression.Constant(source, typeof(BaseTestClass1)));

            // Assert
            Expression.Lambda<Action>(Expression.Block(scaffold.Expressions)).Compile()();
        }

        private static InjectionScaffold CreateScaffoldForTransform<TSource>(
            TestClass1 target,
            TSource source,
            PropertyInfo member,
            object transformResult)
        {
            var repository = new MockRepository(MockBehavior.Strict);
            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock
                .SetupGet(_ => _.Target)
                .Returns(Expression.Constant(target));

            var injectionContextMock = repository.Create<IInjectionContext>();
            scaffoldMock
                .SetupGet(_ => _.InjectionContext)
                .Returns(Expression.Constant(injectionContextMock.Object));

            var injectionStoreMock = repository.Create<IInjectionStore>();
            injectionContextMock
                .SetupGet(_ => _.InjectionStore)
                .Returns(injectionStoreMock.Object);

            var transformMock = repository.Create<ITransform>();
            var sourceType = ReferenceEquals(source, null) ? typeof(TSource) : source.GetType();
            var targetMemberType = member.PropertyType;
            injectionStoreMock
                .Setup(_ => _.Resolve<ITransform>(sourceType, targetMemberType))
                .Returns(transformMock.Object);

            transformMock
                .Setup(_ => _.Transform(source, injectionContextMock.Object, null))
                .Returns(transformResult);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            return scaffoldMock.Object;
        }

        private static InjectionScaffold CreateScaffoldForMerge<TSource, TTarget>(
            TTarget target,
            TSource source,
            PropertyInfo member,
            IMergeResult mergeResult,
            out MockRepository repository)
        {
            repository = new MockRepository(MockBehavior.Strict);
            var scaffoldMock = repository.Create<InjectionScaffold>();
            scaffoldMock
                .SetupGet(_ => _.Target)
                .Returns(Expression.Constant(target));

            var injectionContextMock = repository.Create<IInjectionContext>();
            scaffoldMock
                .SetupGet(_ => _.InjectionContext)
                .Returns(Expression.Constant(injectionContextMock.Object));

            var injectionStoreMock = repository.Create<IInjectionStore>();
            injectionContextMock
                .SetupGet(_ => _.InjectionStore)
                .Returns(injectionStoreMock.Object);

            var targetMember = member.GetValue(target);
            var sourceType = ReferenceEquals(source, null) ? typeof(TSource) : source.GetType();
            var targetMemberType = ReferenceEquals(targetMember, null) ? member.PropertyType : targetMember.GetType();
            
            var transformMock = repository.Create<IMerge>();
            injectionStoreMock
                .Setup(_ => _.Resolve<IMerge>(sourceType, targetMemberType)).Returns(transformMock.Object);

            transformMock
                .Setup(_ => _.Merge(source, targetMember, injectionContextMock.Object, null))
                .Returns(mergeResult);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            return scaffoldMock.Object;
        }

        internal class TargetMemberTransformInjector : TargetMemberInjector
        {
            public override void AddMergeExpressionToScaffold(InjectionScaffold scaffold, MemberInfo member, Expression sourceExpression)
            {
            }
        }

        internal class TargetMemberMergeInjector : TargetMemberInjector
        {
            public override void AddTransformExpressionToScaffold(InjectionScaffold scaffold, MemberInfo member, Expression sourceExpression)
            {
            }
        }
    }
}