﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomFactoryFragmentTests.cs" company="Bijectiv">
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
//   Defines the CustomFactoryFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Configuration
{
    using System;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="CustomFactoryFragment"/> class.
    /// </summary>
    [TestClass]
    public class CustomFactoryFragmentTests
    {
        private static readonly Func<CustomFactoryParameters<TestClass1>, TestClass2> Factory = 
            p => default(TestClass2);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(TestClass1.T, TestClass2.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryHasTooFewParameters_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(TestClass2.T, TestClass1.T, new Func<TestClass2>(() => new TestClass2())).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryHasTooManyParameters_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(
                TestClass2.T, TestClass1.T, new Func<int, int, TestClass2>((x, y) => new TestClass2())).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryParameterHasWrongType_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(TestClass2.T, TestClass1.T, Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryIsAction_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(
                TestClass1.T, TestClass2.T, new Action<CustomFactoryParameters<TestClass1>>(x => 1.Naught())).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryHasWrongReturnType_Throws()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(
                TestClass1.T, TestClass2.T, new Func<CustomFactoryParameters<TestClass1>, int>(x => 1)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(TestClass1.T, TestClass2.T, Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InheritedProperty_IsFalse()
        {
            // Arrange

            // Act
            var testTarget = new CustomFactoryFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.IsFalse(testTarget.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FactoryReturnTypeIsMoreDerived_InstanceCreated()
        {
            // Arrange

            // Act
            new CustomFactoryFragment(TestClass1.T, typeof(object), Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentCategoryProperty_IsFactory()
        {
            // Arrange

            // Act
            var testTarget = new CustomFactoryFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(LegendaryFragments.Factory, testTarget.FragmentCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FactoryParameter_IsAssignedToFactoryProperty()
        {
            // Arrange

            // Act
            var testTarget = new CustomFactoryFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(Factory, testTarget.Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ParameterPropertyIsCalculatedFromSoureParameter()
        {
            // Arrange

            // Act
            var testTarget = new CustomFactoryFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(typeof(CustomFactoryParameters<TestClass1>), testTarget.ParameterType);
        }
    }
}