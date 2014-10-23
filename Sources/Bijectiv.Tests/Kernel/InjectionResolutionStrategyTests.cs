﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionResolutionStrategyTests.cs" company="Bijectiv">
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
//   Defines the InjectionResolutionStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class InjectionResolutionStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionResolutionStrategy().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        //[ArgumentNullExceptionExpected]
        public void Choose_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new InjectionResolutionStrategy();

            // Act


            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Choose_ExactMatch_Chooses()
        {
            // Arrange


            // Act

            // Assert
        }

        private static IInjection I(Type source, Type target)
        {
            var injectionMock = new Mock<IInjection>(MockBehavior.Strict);
            
            injectionMock.SetupGet(_ => _.Source).Returns(source);
            injectionMock.SetupGet(_ => _.Target).Returns(target);
            
            return injectionMock.Object;
        }

        private static IEnumerable<IInjection> Ix(params IInjection[] injections)
        {
            return injections;
        }
    }
}