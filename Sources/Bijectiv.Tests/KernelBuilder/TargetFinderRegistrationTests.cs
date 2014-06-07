// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetFinderRegistrationTests.cs" company="Bijectiv">
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
//   Defines the TargetFinderRegistrationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelBuilder
{
    using System;

    using Bijectiv.KernelBuilder;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TargetFinderRegistration"/> class.
    /// </summary>
    [TestClass]
    public class TargetFinderRegistrationTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceElementParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TargetFinderRegistration(null, TestClass2.T, () => Stub.Create<ITargetFinder>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetElementParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TargetFinderRegistration(TestClass1.T, null, () => Stub.Create<ITargetFinder>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetFinderFactoryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TargetFinderRegistration(TestClass1.T, TestClass2.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TargetFinderRegistration(TestClass1.T, TestClass2.T, () => Stub.Create<ITargetFinder>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceElementParameter_IsAssignedToSourceElementProperty()
        {
            // Arrange

            // Act
            var target = new TargetFinderRegistration(TestClass1.T, TestClass2.T, () => Stub.Create<ITargetFinder>());

            // Assert
            Assert.AreEqual(TestClass1.T, target.SourceElement);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetElementParameter_IsAssignedToTargetElementProperty()
        {
            // Arrange

            // Act
            var target = new TargetFinderRegistration(TestClass1.T, TestClass2.T, () => Stub.Create<ITargetFinder>());

            // Assert
            Assert.AreEqual(TestClass2.T, target.TargetElement);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetFinderFactoryParameter_IsAssignedToTargetFinderFactoryProperty()
        {
            // Arrange
            Func<ITargetFinder> targetFinderFactory = () => Stub.Create<ITargetFinder>();

            // Act
            var target = new TargetFinderRegistration(TestClass1.T, TestClass2.T, targetFinderFactory);

            // Assert
            Assert.AreEqual(targetFinderFactory, target.TargetFinderFactory);
        }
    }
}