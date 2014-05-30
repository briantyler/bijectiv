// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFactoryRegistrationTests.cs" company="Bijectiv">
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
//   Defines the EnumerableFactoryRegistrationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="EnumerableFactoryRegistration"/> class.
    /// </summary>
    [TestClass]
    public class EnumerableFactoryRegistrationTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableFactoryRegistration(null, typeof(Collection<>)).Naught();
            
            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ConcretTypeParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumerableFactoryRegistration(typeof(IEnumerable<>), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterIsNotInterface_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(typeof(Collection<>), typeof(List<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterIsNotGeneric_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IPlaceholderEnumerable), typeof(GenericPlaceholderCollection<Placeholder>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterIsNotMonad_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(ICrazyPlaceholderEnumerable<,>), typeof(CrazyPlaceholderCollection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ConcreteTypeParameterIsNotClass_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(ICollection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ConcreteTypeParameterIsNotGeneric_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<Placeholder>), typeof(NonMonadicPlaceholderCollection<Placeholder, Placeholder>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ConcreteTypeParameterIsNotMonad_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<Placeholder>), typeof(PlaceholderCollection)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterDoesNotImplementEnumerable_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEquatable<>), typeof(EquatableCollection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_ConcreteTypeParameterDoesNotImplementCollection_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<>), typeof(Queue<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_InterfaceTypeParameterIsNotAssignableFromConcreteTypeParameter_Throws()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(ISet<>), typeof(Collection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidOpenGenericParameters_CreatesInstance()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<>), typeof(Collection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidGenericParameters_CreatesInstance()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<Placeholder>), typeof(Collection<Placeholder>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidMixedGenericParameters_CreatesInstance()
        {
            // Arrange

            // Act
            new EnumerableFactoryRegistration(
                typeof(IEnumerable<Placeholder>), typeof(Collection<>)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidOpenGenericParameters_AssignsInterfaceTypeProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<>));

            // Assert
            Assert.AreEqual(typeof(IEnumerable<>), target.InterfaceType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidOpenGenericParameters_AssignsConcreteTypeProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<>));

            // Assert
            Assert.AreEqual(typeof(Collection<>), target.ConcreteType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidGenericParameters_AssignsInterfaceTypeProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableFactoryRegistration(typeof(IEnumerable<Placeholder>), typeof(Collection<>));

            // Assert
            Assert.AreEqual(typeof(IEnumerable<>), target.InterfaceType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidGenericParameters_AssignsConcreteTypeProperty()
        {
            // Arrange

            // Act
            var target = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<Placeholder>));

            // Assert
            Assert.AreEqual(typeof(Collection<>), target.ConcreteType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_IdenticalInstances_HashCodesAreIdentical()
        {
            // Arrange
            var target1 = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<>));
            var target2 = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<>));

            // Act
            var result1 = target1.GetHashCode();
            var result2 = target2.GetHashCode();

            // Assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetHashCode_DifferentInstances_HashCodesAreDifferent()
        {
            // Arrange
            var target1 = new EnumerableFactoryRegistration(typeof(IEnumerable<>), typeof(Collection<>));
            var target2 = new EnumerableFactoryRegistration(typeof(IList<>), typeof(List<>));

            // Act
            var result1 = target1.GetHashCode();
            var result2 = target2.GetHashCode();

            // Assert
            Assert.AreNotEqual(result1, result2);
        }
    }
}