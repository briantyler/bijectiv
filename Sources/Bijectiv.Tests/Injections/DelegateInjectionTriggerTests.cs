// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateInjectionTriggerTests.cs" company="Bijectiv">
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
//   Defines the DelegateInjectionTriggerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using System;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="DelegateInjectionTrigger"/> class.
    /// </summary>
    [TestClass]
    public class DelegateInjectionTriggerTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TriggerParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new DelegateInjectionTrigger(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateInjectionTrigger(p => p.Naught()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TriggerParameter_IsAssignedToTriggerProperty()
        {
            // Arrange
            Action<IInjectionTriggerParameters> trigger = p => p.Naught();

            // Act
            var target = new DelegateInjectionTrigger(trigger);

            // Assert
            Assert.AreEqual(trigger, target.Trigger);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Pull_ParametersParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateInjectionTrigger(p => p.Naught());

            // Act
            target.Pull(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Pull_ValidParameters_PullsTriggerWithParameters()
        {
            // Arrange
            object called = false;
            var parameters = Stub.Create<IInjectionTriggerParameters>();
            var target = new DelegateInjectionTrigger(
                p =>
                    {
                        Assert.AreEqual(parameters, p);
                        called = true;
                    });

            // Act
            target.Pull(parameters);

            // Assert
            Assert.IsTrue((bool)called);
        }
    }
}