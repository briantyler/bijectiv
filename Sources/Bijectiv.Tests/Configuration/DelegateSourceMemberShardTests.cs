// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateSourceMemberShardTests.cs" company="Bijectiv">
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
//   Defines the DelegateSourceMemberShardTests type.
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
    /// This class tests the <see cref="DelegateSourceMemberShard"/> class.
    /// </summary>
    [TestClass]
    public class DelegateSourceMemberShardTests
    {
        private static readonly Func<IInjectionParameters<TestClass1, TestClass2>, BaseTestClass1> Delegate =
           p => default(BaseTestClass1);

        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);
        
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DelegateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateSourceMemberShard(TestClass1.T, TestClass2.T, Member, null, false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_DelegateParameterHasTooFewParameters_Throws()
        {
            // Arrange

            // Act
            new DelegateSourceMemberShard(TestClass1.T, TestClass2.T, Member, new Func<int>(() => 1), false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_DelegateParameterHasTooManyParameters_Throws()
        {
            // Arrange

            // Act
            new DelegateSourceMemberShard(
                TestClass1.T, 
                TestClass2.T, 
                Member,
                new Func<IInjectionParameters<TestClass1, TestClass2>, int, int>((x, y) => 1),
                false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_DelegateParameterHasWrongParameterType_Throws()
        {
            // Arrange

            // Act
            new DelegateSourceMemberShard(
                TestClass1.T,
                TestClass2.T,
                Member,
                new Func<int, int>(x => 1),
                false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_DelegateParameterIsAnAction_Throws()
        {
            // Arrange

            // Act
            new DelegateSourceMemberShard(
                TestClass1.T,
                TestClass2.T,
                Member,
                new Action<IInjectionParameters<TestClass1, TestClass2>>(x => 1.Naught()),
                false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateSourceMemberShard(TestClass1.T, TestClass2.T, Member, Delegate, false).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DelegateParameterIsAssignedToDelegateProperty()
        {
            // Arrange

            // Act
            var target = new DelegateSourceMemberShard(TestClass1.T, TestClass2.T, Member, Delegate, false);

            // Assert
            Assert.AreEqual(Delegate, target.Delegate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ParameterTypePropertyIsAssigned()
        {
            // Arrange

            // Act
            var target = new DelegateSourceMemberShard(TestClass1.T, TestClass2.T, Member, Delegate, false);

            // Assert
            Assert.AreEqual(typeof(IInjectionParameters<TestClass1, TestClass2>), target.ParameterType);
        }
    }
}