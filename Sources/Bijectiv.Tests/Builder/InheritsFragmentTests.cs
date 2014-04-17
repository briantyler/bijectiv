// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritsFragmentTests.cs" company="Bijectiv">
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
//   Defines the InheritsFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Builder
{
    using Bijectiv.Builder;
    using Bijectiv.Tests.TestTools;
    using Bijectiv.Tests.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InheritsFragment"/> class.
    /// </summary>
    [TestClass]
    public class InheritsFragmentTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceBaseParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                null,
                typeof(BaseTestClass2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetBaseParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceBaseParameterIsNotAssignableFromSourceParameter_Throws()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(TestClass1),
                typeof(BaseTestClass2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetBaseParameterIsNotAssignableFromTargetParameter_Throws()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                typeof(TestClass2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceBaseParameterEqualsTargetBaseParameter_Throws()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(TestClass1),
                typeof(TestClass2),
                typeof(TestClass1),
                typeof(TestClass2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InheritsFragment(
                typeof(DerivedTestClass1), 
                typeof(DerivedTestClass2), 
                typeof(BaseTestClass1), 
                typeof(BaseTestClass2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceBaseParameter_IsAssignedToSourceBaseProperty()
        {
            // Arrange

            // Act
            var target = new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                typeof(BaseTestClass2));

            // Assert
            Assert.AreEqual(typeof(BaseTestClass1), target.SourceBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetBaseParameter_IsAssignedToTargetBaseProperty()
        {
            // Arrange

            // Act
            var target = new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                typeof(BaseTestClass2));

            // Assert
            Assert.AreEqual(typeof(BaseTestClass2), target.TargetBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InheritedProperty_IsFalse()
        {
            // Arrange

            // Act
            var target = new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                typeof(BaseTestClass2));

            // Assert
            Assert.IsTrue(target.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentCategoryProperty_IsInherits()
        {
            // Arrange

            // Act
            var target = new InheritsFragment(
                typeof(DerivedTestClass1),
                typeof(DerivedTestClass2),
                typeof(BaseTestClass1),
                typeof(BaseTestClass2));

            // Assert
            Assert.AreEqual(LegendryFragments.Inherits, target.FragmentCategory);
        }
    }
}