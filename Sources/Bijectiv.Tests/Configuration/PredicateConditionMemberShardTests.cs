// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredicateConditionMemberShardTests.cs" company="Bijectiv">
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
//   Defines the PredicateConditionMemberShardTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="PredicateConditionMemberShard"/> class.
    /// </summary>
    [TestClass]
    public class PredicateConditionMemberShardTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        private static readonly Func<IInjectionParameters<TestClass1, TestClass2>, bool> Predicate = p => true;
            
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_PredicateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PredicateParameterHasWrongFuncType_Throws()
        {
            // Arrange
            Func<bool> predicate = () => true;

            // Act
            new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PredicateParameterHasWrongResultType_Throws()
        {
            // Arrange
            Func<IInjectionParameters<TestClass1, TestClass2>, int> predicate = p => 1;

            // Act
            new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PredicateParameterHasMoreDerivedParameters_Throws()
        {
            // Arrange
            var member = Reflect<BaseTestClass2>.FieldOrProperty(_ => _.Id);
            Func<IInjectionParameters<DerivedTestClass1, DerivedTestClass2>, bool> predicate = p => true;

            // Act
            new PredicateConditionMemberShard(BaseTestClass1.T, BaseTestClass2.T, member, predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, Predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_PredicateParameterHasBaseParameters_InstanceCreated()
        {
            // Arrange
            var member = Reflect<BaseTestClass2>.FieldOrProperty(_ => _.Id);
            Func<IInjectionParameters<BaseTestClass1, BaseTestClass2>, bool> predicate = p => true;

            // Act
            new PredicateConditionMemberShard(DerivedTestClass1.T, DerivedTestClass2.T, member, predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_PredicateParameterHasPathalogicallyBaseParameters_InstanceCreated()
        {
            // Arrange
            var member = Reflect<BaseTestClass2>.FieldOrProperty(_ => _.Id);
            Func<object, bool> predicate = p => true;

            // Act
            new PredicateConditionMemberShard(DerivedTestClass1.T, DerivedTestClass2.T, member, predicate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_PredicateParameterIsAssignedToPredicateProperty()
        {
            // Arrange

            // Act
            var target = new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, Predicate);
            
            // Assert
            Assert.AreEqual(Predicate, target.Predicate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ShardCategoryPropertyIsCondidtion()
        {
            // Arrange

            // Act
            var target = new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, Predicate);

            // Assert
            Assert.AreEqual(LegendaryShards.Condition, target.ShardCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_AssignsPredicateParameterType()
        {
            // Arrange

            // Act
            var target = new PredicateConditionMemberShard(TestClass1.T, TestClass2.T, Member, Predicate);

            // Assert
            Assert.AreEqual(
                typeof(IInjectionParameters<TestClass1, TestClass2>),
                target.PredicateParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_PredicateParameterHasBaseParameters_AssignsPredicateParameterType()
        {
            // Arrange
            var member = Reflect<BaseTestClass2>.FieldOrProperty(_ => _.Id);
            Func<IInjectionParameters<BaseTestClass1, BaseTestClass2>, bool> predicate = p => true;

            // Act
            var target = new PredicateConditionMemberShard(
                DerivedTestClass1.T, DerivedTestClass2.T, member, predicate);

            // Assert
            Assert.AreEqual(
                typeof(IInjectionParameters<BaseTestClass1, BaseTestClass2>), 
                target.PredicateParameterType);
        }
    }
}