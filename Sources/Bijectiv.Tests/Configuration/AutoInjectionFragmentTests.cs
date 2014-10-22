// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoInjectionFragmentTests.cs" company="Bijectiv">
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
//   Defines the AutoInjectionFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class test the <see cref="AutoInjectionFragment"/> class.
    /// </summary>
    [TestClass]
    public class AutoInjectionFragmentTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_StrategyParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new AutoInjectionFragment(TestClass1.T, TestClass2.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new AutoInjectionFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IAutoInjectionStrategy>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InheritedProperty_IsTrue()
        {
            // Arrange

            // Act
            var testTarget = new AutoInjectionFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IAutoInjectionStrategy>());

            // Assert
            Assert.IsTrue(testTarget.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentCategoryProperty_IsSourceMemberStrategy()
        {
            // Arrange

            // Act
            var testTarget = new AutoInjectionFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IAutoInjectionStrategy>());

            // Assert
            Assert.AreEqual(LegendaryFragments.AutoInjection, testTarget.FragmentCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_StrategyParameter_IsAssignedToStrategyProperty()
        {
            // Arrange
            var strategy = Stub.Create<IAutoInjectionStrategy>();

            // Act
            var testTarget = new AutoInjectionFragment(TestClass1.T, TestClass2.T, strategy);

            // Assert
            Assert.AreEqual(strategy, testTarget.Strategy);
        }
    }
}