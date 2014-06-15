// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetFinderStoreTests.cs" company="Bijectiv">
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
//   Defines the TargetFinderStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TargetFinderStore"/> class.
    /// </summary>
    [TestClass]
    public class TargetFinderStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TargetFinderStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_FindersPropertyIsEmpty()
        {
            // Arrange

            // Act
            var target = new TargetFinderStore();

            // Assert
            Assert.IsFalse(target.Finders.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Register_RegistrationParameterIsNull_Throws()
        {
            // Arrange
            var target = new TargetFinderStore();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Register(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_RegistrationDoesNotExist_AddsNewRegistration()
        {
            // Arrange
            var registration = new TargetFinderRegistration(
                TestClass1.T,
                TestClass2.T,
                () => Stub.Create<ITargetFinder>());
            var target = new TargetFinderStore();

            // Act
            target.Register(registration);

            // Assert
            Assert.IsTrue(target.Finders.ContainsKey(Tuple.Create(TestClass1.T, TestClass2.T)));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_RegistrationDoesNotExist_TargetFinderFactoryIsRegistered()
        {
            // Arrange
            Func<ITargetFinder> targetFinderFactory = () => Stub.Create<ITargetFinder>();
            var registration = new TargetFinderRegistration(TestClass1.T, TestClass2.T, targetFinderFactory);
            var target = new TargetFinderStore();

            // Act
            target.Register(registration);

            // Assert
            Assert.AreEqual(targetFinderFactory, target.Finders[Tuple.Create(TestClass1.T, TestClass2.T)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_RegistrationExists_OverwritesRegistration()
        {
            // Arrange
            Func<ITargetFinder> targetFinderFactory = () => Stub.Create<ITargetFinder>();
            var registration = new TargetFinderRegistration(
                TestClass1.T,
                TestClass2.T,
                targetFinderFactory);
            var target = new TargetFinderStore();
            target.Finders.Add(Tuple.Create(TestClass1.T, TestClass2.T), () => Stub.Create<ITargetFinder>());

            // Act
            target.Register(registration);

            // Assert
            Assert.AreEqual(targetFinderFactory, target.Finders[Tuple.Create(TestClass1.T, TestClass2.T)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_RegistrationExists_ReturnsTargetFinderFactoryResult()
        {
            // Arrange
            var finder = Stub.Create<ITargetFinder>();
            var target = new TargetFinderStore();
            target.Finders.Add(Tuple.Create(TestClass1.T, TestClass2.T), () => finder);

            // Act
            var result = target.Resolve(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(finder, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_RegistrationDoesNotExist_ReturnsNullTargetFinder()
        {
            // Arrange
            var target = new TargetFinderStore();

            // Act
            var result = target.Resolve(TestClass1.T, TestClass2.T);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NullTargetFinder));
        }
    }
}