// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquivalentMemberInfoCollectionTests.cs" company="Bijectiv">
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
//   Defines the EquivalentMemberInfoCollectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="EquivalentMemberInfoCollection"/> class.
    /// </summary>
    [TestClass]
    public class EquivalentMemberInfoCollectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TypeParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new EquivalentMemberInfoCollection(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EquivalentMemberInfoCollection(TestClass1.T).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_CountIsZero()
        {
            // Arrange

            // Act
            var target = new EquivalentMemberInfoCollection(TestClass1.T);

            // Assert
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ReadOnlyIsFalse()
        {
            // Arrange

            // Act
            var target = new EquivalentMemberInfoCollection(TestClass1.T);

            // Assert
            Assert.AreEqual(false, target.IsReadOnly);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TypeParameter_IsAssignedToTypeProperty()
        {
            // Arrange

            // Act
            var target = new EquivalentMemberInfoCollection(TestClass1.T);

            // Assert
            Assert.AreEqual(TestClass1.T, target.Type);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TypeParameter_DeterminesHierarchy()
        {
            // Arrange

            // Act
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy3));

            // Assert
            new[]
                {
                    typeof(object),
                    typeof(MemberInfoHierarchy1), 
                    typeof(MemberInfoHierarchy2),
                    typeof(MemberInfoHierarchy3),
                }.AssertSequenceEqual(target.Hierarchy);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_ItemParameterIsNull_Throws()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6));

            // Act
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_ItemParameterIsNotFieldOrProperty_Throws()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6));
            var member = Reflect<MemberInfoHierarchy6>.Method(_ => _.GetHashCode());

            // Act
            target.Add(member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterIsAnyProperty_IsAdded()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6));
            var member = Reflect<PropertyTestClass>.FieldOrProperty(_ => _.Property);

            // Act
            target.Add(member);

            // Assert
            Assert.AreEqual(1, target.Count(candidate => candidate == member));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ItemParameterIsAnyField_IsAdded()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6));
            var member = Reflect<FieldTestClass>.FieldOrProperty(_ => _.Field);

            // Act
            target.Add(member);

            // Assert
            Assert.AreEqual(1, target.Count(candidate => candidate == member));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(NotSupportedException))]
        public void Remove_ItemParameterIsAnything_Throws()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6));
            var member = Reflect<MemberInfoHierarchy6>.FieldOrProperty(_ => _.Id);
            target.Add(member);

            // Act
            target.Remove(member);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Clear_DefaultParameters_ClearsCollection()
        {
            // Arrange
            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6))
            {
                Reflect<MemberInfoHierarchy6>.FieldOrProperty(_ => _.Id)
            };

            // Sanity check
            Assert.IsTrue(target.Any());

            // Act
            target.Clear();

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_WeaklyTyped_ReturnsEnumerator()
        {
            // Arrange
            IEnumerable target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6))
            {
                Reflect<MemberInfoHierarchy6>.FieldOrProperty(_ => _.Id)
            };

            // Act
            var members = new List<MemberInfo>();
            foreach (var item in target)
            {
                members.Add((MemberInfo)item);
            }

            // Assert
            members.AssertSetEqual(target.Cast<MemberInfo>());
        }
    }
}