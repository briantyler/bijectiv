// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRegistryTests.cs" company="Bijectiv">
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
//   Defines the InstanceRegistryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InstanceRegistry"/> class.
    /// </summary>
    [TestClass]
    public class InstanceRegistryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InstanceRegistry().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_ContainsNoRegistrations()
        {
            // Arrange

            // Act
            var testTarget = new InstanceRegistry();

            // Assert
            Assert.IsFalse(testTarget.Registrations.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Register_InstanceTypeParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Register(null, new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Register_InstanceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Register(typeof(object), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Register_InstanceParameterIsNotInstanceOfInstanceTypeParameter_Throws()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            testTarget.Register(typeof(TestClass1), new object());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_InstanceParameterIsFirstInstanceOfType_CreatesRegistration()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            testTarget.Register(typeof(TestClass1), new TestClass1());

            // Assert
            Assert.IsTrue(testTarget.Registrations.ContainsKey(typeof(TestClass1)));
            Assert.AreEqual(1, testTarget.Registrations[typeof(TestClass1)].Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_InstanceParameterIsFirstInstanceOfType_RegistersInstance()
        {
            // Arrange
            var testTarget = new InstanceRegistry();
            var instance = new TestClass1();

            // Act
            testTarget.Register(typeof(TestClass1), instance);

            // Assert
            Assert.AreEqual(instance, testTarget.Registrations[typeof(TestClass1)].Last());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_InstanceParameterIsNotFirstInstanceOfType_CreatesRegistration()
        {
            // Arrange
            var testTarget = new InstanceRegistry();
            testTarget.Register(typeof(TestClass1), new TestClass1());

            // Act
            testTarget.Register(typeof(TestClass1), new TestClass1());

            // Assert
            Assert.IsTrue(testTarget.Registrations.ContainsKey(typeof(TestClass1)));
            Assert.AreEqual(2, testTarget.Registrations[typeof(TestClass1)].Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_InstanceParameterIsNotFirstInstanceOfType_RegistersInstance()
        {
            // Arrange
            var testTarget = new InstanceRegistry();
            var instance = new TestClass1();
            testTarget.Register(typeof(TestClass1), new TestClass1());

            // Act
            testTarget.Register(typeof(TestClass1), instance);

            // Assert
            Assert.AreEqual(instance, testTarget.Registrations[typeof(TestClass1)].Last());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void RegisterTuple_RegistrationParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Register(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterTuple_ValidParameters_DelegatesToNonTuple()
        {
            // Arrange
            var testTargetMock = new Mock<InstanceRegistry>(MockBehavior.Strict) { CallBase = false };
            var instanceType = typeof(TestClass1);
            var instance = new TestClass1();

            testTargetMock.Setup(_ => _.Register(instanceType, instance));

            // Act
            testTargetMock.Object.Register(Tuple.Create<Type, object>(instanceType, instance));

            // Assert
            testTargetMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ResolveAll_InstanceTypeParameterIsNotRegistered_ReturnsEmptyCollection()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            var result = testTarget.ResolveAll<TestClass1>();

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ResolveAll_InstanceTypeParameterIsRegistered_ReturnsInstances()
        {
            // Arrange
            var testTarget = new InstanceRegistry();
            var registrations = new[] { new TestClass1(), new TestClass1(), new TestClass1() };
            testTarget.Registrations[typeof(TestClass1)] = registrations.Cast<object>().ToList();

            // Act
            var result = testTarget.ResolveAll<TestClass1>();

            // Assert
            registrations.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Resolve_InstanceTypeParameterIsNotRegistered_Throws()
        {
            // Arrange
            var testTarget = new InstanceRegistry();

            // Act
            testTarget.Resolve<TestClass1>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_InstanceTypeParameterIsRegistered_ReturnsLastInstance()
        {
            // Arrange
            var testTarget = new InstanceRegistry();
            var registrations = new[] { new TestClass1(), new TestClass1(), new TestClass1() };
            testTarget.Registrations[typeof(TestClass1)] = registrations.Cast<object>().ToList();

            // Act
            var result = testTarget.Resolve<TestClass1>();

            // Assert
            Assert.AreEqual(registrations.Last(), result);
        }
    }
}