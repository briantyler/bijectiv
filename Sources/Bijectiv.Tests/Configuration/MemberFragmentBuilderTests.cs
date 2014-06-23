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

        private static MemberFragment CreateFragment()
        {
            return new MemberFragment(TestClass1.T, TestClass2.T, Member);
        }
    }
}