// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSourceFragmentTests.cs" company="Bijectiv">
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
//   Defines the NullSourceFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Builder
{
    using System;

    using Bijectiv.Builder;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="NullSourceFragment"/> class.
    /// </summary>
    [TestClass]
    public class NullSourceFragmentTests
    {
        private static readonly Func<IInjectionContext, TestClass2> Factory = context => default(TestClass2);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new NullSourceFragment(TestClass1.T, TestClass2.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_FactoryParameterHasWrongType_Throws()
        {
            // Arrange

            // Act
            new NullSourceFragment(TestClass2.T, TestClass1.T, Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NullSourceFragment(TestClass1.T, TestClass2.T, Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InheritedProperty_IsFalse()
        {
            // Arrange

            // Act
            var target = new NullSourceFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.IsFalse(target.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FactoryReturnTypeIsMoreDerived_InstanceCreated()
        {
            // Arrange

            // Act
            new NullSourceFragment(TestClass1.T, typeof(object), Factory).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentCategoryProperty_IsNullSource()
        {
            // Arrange

            // Act
            var target = new NullSourceFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(LegendryFragments.NullSource, target.FragmentCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FactoryParameter_IsAssignedToFactoryProperty()
        {
            // Arrange

            // Act
            var target = new NullSourceFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(Factory, target.Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ExactFactory_FactoryTypePropertyIsCalculatedFromTargetParameter()
        {
            // Arrange

            // Act
            var target = new NullSourceFragment(TestClass1.T, TestClass2.T, Factory);

            // Assert
            Assert.AreEqual(typeof(Func<IInjectionContext, TestClass2>), target.FactoryType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CovariantFactory_FactoryTypePropertyIsCalculatedFromTargetParameter()
        {
            // Arrange

            // Act
            var target = new NullSourceFragment(TestClass1.T, typeof(object), Factory);

            // Assert
            Assert.AreEqual(typeof(Func<IInjectionContext, object>), target.FactoryType);
        }
    }
}