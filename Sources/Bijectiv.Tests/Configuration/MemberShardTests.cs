// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberShardTests.cs" company="Bijectiv">
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
//   Defines the MemberShardTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="MemberShard"/> class.
    /// </summary>
    [TestClass]
    public class MemberShardTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            Stub.Create<MemberShard>(null, TestClass2.T, Member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            Stub.Create<MemberShard>(TestClass1.T, null, Member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_MemberParameterIsNull_Throws()
        {
            // Arrange

            // Act
            Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_MemberParameterHasNoDeclaringType_Throws()
        {
            // Arrange

            // Act
            Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Stub.Create<MemberInfo>());

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
            Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_MemberParameterDeclaringTypeIsBaseOfTarget_InstanceCreated()
        {
            // Arrange
            var member = Reflect<BaseTestClass1>.FieldOrProperty(_ => _.Id);

            // Act
            Stub.Create<MemberShard>(TestClass1.T, DerivedTestClass1.T, member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourceParameterIsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);          

            // Assert
            Assert.AreEqual(TestClass1.T, target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.AreEqual(TestClass2.T, target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_MemberParameterIsAssignedToMemberProperty()
        {
            // Arrange

            // Act
            var target = Stub.Create<MemberShard>(TestClass1.T, TestClass2.T, Member);

            // Assert
            Assert.AreEqual(Member, target.Member);
        }
    }
}