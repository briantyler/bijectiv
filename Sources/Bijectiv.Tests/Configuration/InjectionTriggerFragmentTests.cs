﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTriggerFragmentTests.cs" company="Bijectiv">
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
//   Defines the InjectionTriggerFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the see <see cref="InjectionTriggerFragment"/> tests.
    /// </summary>
    [TestClass]
    public class InjectionTriggerFragmentTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TriggerParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionTriggerFragment(TestClass1.T, TestClass2.T, null, TriggeredBy.InjectionEnded).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IInjectionTrigger>(), TriggeredBy.InjectionEnded).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_FragmentCategoryPropertyIsTrigger()
        {
            // Arrange

            // Act
            var target = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IInjectionTrigger>(), TriggeredBy.InjectionEnded);

            // Assert
            Assert.AreEqual(LegendaryFragments.Trigger, target.FragmentCategory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InheritedPropertyIsTrue()
        {
            // Arrange

            // Act
            var target = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IInjectionTrigger>(), TriggeredBy.InjectionEnded);

            // Assert
            Assert.IsTrue(target.Inherited);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TriggerParameter_IsAssignedToTriggerProperty()
        {
            // Arrange
            var trigger = Stub.Create<IInjectionTrigger>();

            // Act
            var target = new InjectionTriggerFragment(TestClass1.T, TestClass2.T, trigger, TriggeredBy.InjectionEnded);

            // Assert
            Assert.AreEqual(trigger, target.Trigger);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TriggeredByParameter_IsAssignedToTriggeredByProperty()
        {
            // Arrange

            // Act
            var target = new InjectionTriggerFragment(
                TestClass1.T, TestClass2.T, Stub.Create<IInjectionTrigger>(), TriggeredBy.InjectionEnded);

            // Assert
            Assert.AreEqual(TriggeredBy.InjectionEnded, target.TriggeredBy);
        }
    }
}