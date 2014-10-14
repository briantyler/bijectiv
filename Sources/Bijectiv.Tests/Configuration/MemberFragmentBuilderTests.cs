// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberFragmentBuilderTests.cs" company="Bijectiv">
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
//   Defines the MemberFragmentBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="MemberFragmentBuilder{TSource,TTarget,TMember}"/> class.
    /// </summary>
    [TestClass]
    public class MemberFragmentBuilderTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_BuilderParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(null, CreateFragment()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FragmentParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), 
                CreateFragment()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_BuilderParameter_IsAssignedToBuilderProperty()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();

            // Act
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                builder,
                CreateFragment());

            // Assert
            Assert.AreEqual(builder, target.Builder);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentParameter_IsAssignedToFragmentProperty()
        {
            // Arrange
            var fragment = CreateFragment();

            // Act
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Assert
            Assert.AreEqual(fragment, target.Fragment);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Condition_PredicateParameterIsNull_Throws()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                    Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                    fragment);
            
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Condition(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Condition_ValidParameters_AddsPredicateConditionMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                    Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                    fragment);

            // Act
            target.Condition(p => true);

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(PredicateConditionMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Condition_ValidParameters_AddedPredicateConditionMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                    Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                    fragment);

            Func<IInjectionParameters<TestClass1, TestClass2>, bool> predicate = p => true;

            // Act
            target.Condition(predicate);

            // Assert
            var shard = (PredicateConditionMemberShard)fragment.Single();
            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(predicate, shard.Predicate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Condition_ValidParameters_ReturnsSelf()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                    Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                    fragment);

            // Act
            var result = target.Condition(p => true);

            // Assert
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Ignore_DefaultParameters_CallsCondition()
        {
            // Arrange
            var targetMock = new Mock<MemberFragmentBuilder<TestClass1, TestClass2, string>>(
                MockBehavior.Loose,
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                CreateFragment()) { CallBase = true };

            Func<IInjectionParameters<TestClass1, TestClass2>, bool> predicate = null;

            targetMock
                .Setup(_ => _.Condition(It.IsAny<Func<IInjectionParameters<TestClass1, TestClass2>, bool>>()))
                .Callback((Func<IInjectionParameters<TestClass1, TestClass2>, bool> f) => predicate = f);

            // Act
            targetMock.Object.Ignore();

            // Assert
            Assert.IsNotNull(predicate);
            Assert.IsFalse(predicate(null));
            Assert.IsFalse(predicate(Stub.Create<IInjectionParameters<TestClass1, TestClass2>>()));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Ignore_DefaultParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.Ignore();

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void InjectValue_ValueParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), 
                CreateFragment());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.InjectValue(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectValue_ValidParameters_AddsValueSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.InjectValue(new object());

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(ValueSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectValue_ValidParameters_AddedValueSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            var value = new object();

            // Act
            target.InjectValue(value);

            // Assert
            var shard = (ValueSourceMemberShard)fragment.Single();

            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(value, shard.Value);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectValue_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.InjectValue(new object());

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignValue_ValidParameters_AddsValueSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.AssignValue("bijectiv");

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(ValueSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignValue_ValidParameters_AddedValueSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.AssignValue("bijectiv");

            // Assert
            var shard = (ValueSourceMemberShard)fragment.Single();

            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual("bijectiv", shard.Value);
            Assert.AreEqual(false, shard.Inject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignValue_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.AssignValue("bijectiv");

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void InjectSource_ExpressionParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), 
                CreateFragment());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.InjectSource<int>(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectSource_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.InjectSource(s => s.Id);

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectSource_ValidParameters_AddsExpressionSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), 
                fragment);

            // Act
            target.InjectSource(s => s.Id);

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(ExpressionSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectSource_ValidParameters_AddedExpressionSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);
            Expression<Func<TestClass1, string>> expression = s => s.Id;

            // Act
            target.InjectSource(expression);

            // Assert
            var shard = (ExpressionSourceMemberShard)fragment.Single();
            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(expression, shard.Expression);
            Assert.AreEqual(true, shard.Inject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AssignSource_ExpressionParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                CreateFragment());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AssignSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignSource_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.AssignSource(s => s.Id);

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignSource_ValidParameters_AddsExpressionSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.AssignSource(s => s.Id);

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(ExpressionSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignSource_ValidParameters_AddedExpressionSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);
            Expression<Func<TestClass1, string>> expression = s => s.Id;

            // Act
            target.AssignSource(expression);

            // Assert
            var shard = (ExpressionSourceMemberShard)fragment.Single();
            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(expression, shard.Expression);
            Assert.AreEqual(false, shard.Inject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void InjectDelegate_DelegateParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                CreateFragment());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.InjectDelegate<int>(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectDelegate_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.InjectDelegate(s => s.Source.Id);

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectDelegate_ValidParameters_AddsDelegateSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.InjectDelegate(p => p.Source.Id);

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(DelegateSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectDelegate_ValidParameters_AddedDelegateSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);
            Func<IInjectionParameters<TestClass1, TestClass2>, string> @delegate = p => p.Source.Id;

            // Act
            target.InjectDelegate(@delegate);

            // Assert
            var shard = (DelegateSourceMemberShard)fragment.Single();
            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(@delegate, shard.Delegate);
            Assert.AreEqual(true, shard.Inject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AssignDelegate_DelegateParameterIsNull_Throws()
        {
            // Arrange
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                CreateFragment());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AssignDelegate(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignDelegate_ValidParameters_ReturnsParentBuilder()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(builder, CreateFragment());

            // Act
            var result = target.AssignDelegate(s => s.Source.Id);

            // Assert
            Assert.AreEqual(builder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignDelegate_ValidParameters_AddsDelegateSourceMemberShardToFragment()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Act
            target.AssignDelegate(p => p.Source.Id);

            // Assert
            Assert.IsInstanceOfType(fragment.Single(), typeof(DelegateSourceMemberShard));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AssignDelegate_ValidParameters_AddedDelegateSourceMemberShardHasExpectedProperties()
        {
            // Arrange
            var fragment = CreateFragment();
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);
            Func<IInjectionParameters<TestClass1, TestClass2>, string> @delegate = p => p.Source.Id;

            // Act
            target.AssignDelegate(@delegate);

            // Assert
            var shard = (DelegateSourceMemberShard)fragment.Single();
            Assert.AreEqual(TestClass1.T, shard.Source);
            Assert.AreEqual(TestClass2.T, shard.Target);
            Assert.AreEqual(Member, shard.Member);
            Assert.AreEqual(@delegate, shard.Delegate);
            Assert.AreEqual(false, shard.Inject);
        }

        private static MemberFragment CreateFragment()
        {
            return new MemberFragment(TestClass1.T, TestClass2.T, Member);
        }
    }
}