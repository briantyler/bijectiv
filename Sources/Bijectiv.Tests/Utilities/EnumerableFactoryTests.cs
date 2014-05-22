﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactoryTests.cs" company="Bijectiv">
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
//   Defines the EnumerableFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumerableFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumerableFactory().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_EnumerableRegistered()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.IsTrue(target.Registrations.ContainsKey(typeof(IEnumerable<>)));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_EnumerableIsList()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.AreEqual(typeof(List<>), target.Registrations[typeof(IEnumerable<>)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CollectionRegistered()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.IsTrue(target.Registrations.ContainsKey(typeof(ICollection<>)));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CollectionIsList()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.AreEqual(typeof(List<>), target.Registrations[typeof(ICollection<>)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_ListRegistered()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.IsTrue(target.Registrations.ContainsKey(typeof(IList<>)));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_ListIsList()
        {
            // Arrange

            // Act
            var target = new EnumerableFactory();

            // Assert
            Assert.AreEqual(typeof(List<>), target.Registrations[typeof(IList<>)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Register_InterfaceTypeParameterIsNotGeneric_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<IPlaceholderEnumerable, GenericPlaceholderCollection<Placeholder>>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Register_InterfaceTypeIsNotMonad_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<ICrazyPlaceholderEnumerable<Placeholder, Placeholder>, CrazyPlaceholderCollection<Placeholder>>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Register_ConcreteTypeParameterIsNotGeneric_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<IEnumerable<Placeholder>, PlaceholderCollection>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Register_ConcreteTypeParameterIsNotMonad_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<IEnumerable<Placeholder>, NonMonadicPlaceholderCollection<Placeholder, Placeholder>>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_RegistrationDoesNotExist_CreatesRegistration()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<ISet<Placeholder>, HashSet<Placeholder>>();

            // Assert
            Assert.IsTrue(target.Registrations.ContainsKey(typeof(ISet<>)));
            Assert.AreEqual(typeof(HashSet<>), target.Registrations[typeof(ISet<>)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_RegistrationExists_OverwritesRegistration()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Register<IEnumerable<Placeholder>, HashSet<Placeholder>>();

            // Assert
            Assert.IsTrue(target.Registrations.ContainsKey(typeof(IEnumerable<>)));
            Assert.AreEqual(typeof(HashSet<>), target.Registrations[typeof(IEnumerable<>)]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_EnumerableParameterIsClass_CreatesType()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            var result = target.Resolve(typeof(List<TestClass1>));

            // Assert
            Assert.IsInstanceOfType(result, typeof(List<TestClass1>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Resolve_EnumerableParameterIsNonGeneric_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Resolve(typeof(IPlaceholderEnumerable));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void Resolve_EnumerableParameterIsNotRegistered_Throws()
        {
            // Arrange
            var target = CreateTarget();
            target.Registrations.Clear();

            // Act
            target.Resolve(typeof(IEnumerable<TestClass1>));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_EnumerableParameterIsRegistered_CreatesInstance()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            var result = target.Resolve(typeof(IEnumerable<TestClass1>));

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TestClass1>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_NonGenericEnumerableParameter_CreatesInstanceOfEnumerableObject()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            var result = target.Resolve(typeof(IEnumerable));

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<object>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_NonGenericCollectionParameter_CreatesInstanceOfCollectionObject()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            var result = target.Resolve(typeof(ICollection));

            // Assert
            Assert.IsInstanceOfType(result, typeof(ICollection<object>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_NonGenericListParameter_CreatesInstanceOfListObject()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            var result = target.Resolve(typeof(IList));

            // Assert
            Assert.IsInstanceOfType(result, typeof(IList<object>));
        }

        private static EnumerableFactory CreateTarget()
        {
            return new EnumerableFactory();
        }
    }
}