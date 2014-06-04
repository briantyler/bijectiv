// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactoryInstanceFactoryTests.cs" company="Bijectiv">
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
//   Defines the EnumerableFactoryInstanceFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="EnumerableFactoryInstanceFactory"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableFactoryInstanceFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_CreateFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableFactoryInstanceFactory(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableFactoryInstanceFactory(() => Stub.Create<IEnumerableFactory>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_CreateFactoryParameterIsAssignedToCreateFactoryProperty()
        {
            // Arrange
            Func<IEnumerableFactory> createFactory = () => Stub.Create<IEnumerableFactory>();

            // Act
            var target = new EnumerableFactoryInstanceFactory(createFactory);

            // Assert
            Assert.AreEqual(createFactory, target.CreateFactory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_RegistryParameterIsNull_Throws()
        {
            // Arrange
            var target = new EnumerableFactoryInstanceFactory(() => Stub.Create<IEnumerableFactory>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsEnumerableFactoryType()
        {
            // Arrange
            var target = new EnumerableFactoryInstanceFactory(() => Stub.Create<IEnumerableFactory>());

            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            registryMock
                .Setup(_ => _.ResolveAll<EnumerableRegistration>())
                .Returns(Enumerable.Empty<EnumerableRegistration>());

            // Act
            var result = target.Create(registryMock.Object);

            // Assert
            Assert.AreEqual(typeof(IEnumerableFactory), result.Item1);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsEnumerableFactory()
        {
            // Arrange
            var factory = Stub.Create<IEnumerableFactory>();
            
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            registryMock
                .Setup(_ => _.ResolveAll<EnumerableRegistration>())
                .Returns(Enumerable.Empty<EnumerableRegistration>());

            var target = new EnumerableFactoryInstanceFactory(() => factory);

            // Act
            var result = target.Create(registryMock.Object);

            // Assert
            Assert.AreEqual(factory, result.Item2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_RegistersAllEnumerableRegistrations()
        {
            var repository = new MockRepository(MockBehavior.Strict);

            var registration1 = new EnumerableRegistration(typeof(IEnumerable<>), typeof(Collection<>));
            var registration2 = new EnumerableRegistration(typeof(IEnumerable<>), typeof(Collection<>));
            var factoryMock = repository.Create<IEnumerableFactory>();

            var sequence = new MockSequence();
            factoryMock.InSequence(sequence).Setup(_ => _.Register(registration1));
            factoryMock.InSequence(sequence).Setup(_ => _.Register(registration2));

            var registryMock = repository.Create<IInstanceRegistry>();
            registryMock
                .Setup(_ => _.ResolveAll<EnumerableRegistration>())
                .Returns(new[] { registration1, registration2 });

            var target = new EnumerableFactoryInstanceFactory(() => factoryMock.Object);

            // Act
            target.Create(registryMock.Object);

            // Assert
            repository.VerifyAll();
        }
    }
}