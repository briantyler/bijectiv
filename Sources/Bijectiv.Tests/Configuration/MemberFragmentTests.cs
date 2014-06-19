// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberFragmentTests.cs" company="Bijectiv">
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
//   Defines the MemberFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="MemberFragment"/> class.
    /// </summary>
    [TestClass]
    public class MemberFragmentTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_MemberParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new MemberFragment(TestClass1.T, TestClass2.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_MemberParameterHasNoDeclaringType_Throws()
        {
            // Arrange

            // Act
            new MemberFragment(TestClass1.T, TestClass2.T, Stub.Create<MemberInfo>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_MemberParameterDeclaringTypeIsNotAssignableFromTarget_Throws()
        {
            // Arrange
            var member = Reflect<TestClass1>.FieldOrProperty(_ => _.Id);

            // Act
            new MemberFragment(TestClass1.T, TestClass2.T, member).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberFragment(TestClass1.T, TestClass2.T, Member).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InheritedPropertyIsTrue()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.IsTrue(target.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_FragmentCategoryPropertyIsMember()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.AreEqual(LegendaryFragments.Member, target.FragmentCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_MemberParameterIsAssignedToMemberyProperty()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.AreEqual(Member, target.Member);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ThisIsEmpty()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_UnprocessedShardsIsEmpty()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.IsFalse(target.UnprocessedShards.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ProcessedShardsIsEmpty()
        {
            // Arrange

            // Act
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.IsFalse(target.ProcessedShards.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberParameterDeclaringTypeIsBaseOfTarget_InstanceCreated()
        {
            // Arrange
            var member = Reflect<BaseTestClass1>.FieldOrProperty(_ => _.Id);

            // Act
            new MemberFragment(TestClass1.T, DerivedTestClass1.T, member).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_ShardParameterIsNull_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_ShardParameterSourcePropertyIsNotEqualToFragmentSourceProperty_Throws()
        {
            // Arrange
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);
            var shard = Stub.Create<MemberShard>(DerivedTestClass1.T, TestClass2.T, Member);

            // Act
            target.Add(shard);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_ShardParameterTargetPropertyIsNotEqualToFragmentTargetProperty_Throws()
        {
            // Arrange
            var member = Reflect<BaseTestClass1>.FieldOrProperty(_ => _.Id);
            var target = new MemberFragment(TestClass1.T, BaseTestClass1.T, member);
            var shard = Stub.Create<MemberShard>(TestClass1.T, DerivedTestClass1.T, member);

            // Act
            target.Add(shard);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_ShardParameterMemberPropertyIsNotEqualToFragmentMemberProperty_Throws()
        {
            // Arrange
            var member1 = Reflect<AutoInjectionTestClass1>.FieldOrProperty(_ => _.FieldInt);
            var member2 = Reflect<AutoInjectionTestClass1>.FieldOrProperty(_ => _.PropertyInt);
            var target = new MemberFragment(TestClass1.T, AutoInjectionTestClass1.T, member1);
            var shard = Stub.Create<MemberShard>(TestClass1.T, AutoInjectionTestClass1.T, member2);

            // Act
            target.Add(shard);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsShard()
        {
            // Arrange
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);
            var shard = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            // Act
            target.Add(shard);

            // Assert
            Assert.AreEqual(shard, target.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_UnprocessedShards_ReturnsUnprocessedShardsInReverseOrder()
        {
            // Arrange
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);
            var shard1 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard2 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard3 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard4 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard5 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            // Act
            target.Add(shard1);
            target.Add(shard2);
            target.Add(shard3);
            target.Add(shard4);
            target.Add(shard5);

            target.ProcessedShards.Add(shard2);
            target.ProcessedShards.Add(shard4);

            // Assert
            new[] { shard5, shard3, shard1 }.AssertSequenceEqual(target.UnprocessedShards);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_NonGeneric_ReturnsShards()
        {
            // Arrange
            var target = new MemberFragment(TestClass1.T, TestClass2.T, Member);
            var shard1 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard2 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard3 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard4 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);
            var shard5 = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            target.Add(shard1);
            target.Add(shard2);
            target.Add(shard3);
            target.Add(shard4);
            target.Add(shard5);

            var enumerable = (IEnumerable)target;

            // Act
            var items = new List<object>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in enumerable)
            {
                items.Add(item);
            }

            // Assert
            new[] { shard1, shard2, shard3, shard4, shard5 }.AssertSequenceEqual(target);
        }
    }
}