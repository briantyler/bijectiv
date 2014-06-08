// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteringInstanceFactoryTests.cs" company="Bijectiv">
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
//   Defines the RegisteringInstanceFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using System;
    using System.Linq;

    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="RegisteringInstanceFactory{TRegistration}"/> class.
    /// </summary>
    [TestClass]
    public class RegisteringInstanceFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InstanceTypeParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new RegisteringInstanceFactory<RegistrationTest>(null, () => Stub.Create<IRegisterTest>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InstanceFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new RegisteringInstanceFactory<RegistrationTest>(typeof(IRegisterTest), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new RegisteringInstanceFactory<RegistrationTest>(
                typeof(IRegisterTest), () => Stub.Create<IRegisterTest>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InstanceTypeParameter_IsAssignedToInstanceTypeProperty()
        {
            // Arrange
            var instanceType = typeof(IRegisterTest);

            // Act
            var target = new RegisteringInstanceFactory<RegistrationTest>(
                instanceType, () => Stub.Create<IRegisterTest>());

            // Assert
            Assert.AreEqual(instanceType, target.InstanceType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InstanceFactoryParameter_IsAssignedToInstanceFactoryProperty()
        {
            // Arrange
            Func<object> instanceFactory = () => Stub.Create<IRegisterTest>();

            // Act
            var target = new RegisteringInstanceFactory<RegistrationTest>(
                typeof(IRegisterTest), instanceFactory);

            // Assert
            Assert.AreEqual(instanceFactory, target.InstanceFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_RegistryParameterIsNull_Throws()
        {
            // Arrange
            var target = new RegisteringInstanceFactory<RegistrationTest>(
                typeof(IRegisterTest), () => Stub.Create<IRegisterTest>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsInstanceFactoryResult()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var instance = Stub.Create<IRegisterTest>();
            var target = new RegisteringInstanceFactory<RegistrationTest>(typeof(IRegisterTest), () => instance);
            var registryMock = repository.Create<IInstanceRegistry>();
            registryMock
                .Setup(_ => _.ResolveAll<RegistrationTest>())
                .Returns(Enumerable.Empty<RegistrationTest>());

            // Act
            var result = target.Create(registryMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(typeof(IRegisterTest), result.Item1);
            Assert.AreEqual(instance, result.Item2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_RegistersEachResolvedItemWithInstance()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var instanceMock = repository.Create<IRegisterTest>();
            var target = new RegisteringInstanceFactory<RegistrationTest>(
                typeof(IRegisterTest), () => instanceMock.Object);

            var registrations = new[] { new RegistrationTest(), new RegistrationTest(), new RegistrationTest() };
            var registryMock = repository.Create<IInstanceRegistry>();
            registryMock
                .Setup(_ => _.ResolveAll<RegistrationTest>())
                .Returns(registrations);

            registrations.ForEach(item => instanceMock.Setup(_ => _.Register(item)));

            // Act
            target.Create(registryMock.Object);

            // Assert
            repository.VerifyAll();
        }
    }
}