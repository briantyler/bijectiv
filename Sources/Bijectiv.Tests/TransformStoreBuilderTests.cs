﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformStoreBuilderTests.cs" company="Bijectiv">
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
//   Defines the TransformStoreBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests
{
    using System;

    using Bijectiv.Builder;
    using Bijectiv.Tests.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TransformStoreBuilder"/> class.
    /// </summary>
    [TestClass]
    public class TransformStoreBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformStoreBuilder(new Mock<ITransformArtifactRegistry>().Object).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_RegistryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformStoreBuilder(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_RegistryParameter_IsAssignedToRegistryProperty()
        {
            // Arrange
            var registry = new Mock<ITransformArtifactRegistry>().Object;
            
            // Act
            var target = new TransformStoreBuilder(registry);

            // Assert
            Assert.AreEqual(registry, target.Registry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterCallback_CallbackParameterIsNull_Throws()
        {
            // Arrange
            var registry = new Mock<ITransformArtifactRegistry>().Object;
            var target = new TransformStoreBuilder(registry);

            // Act
            target.RegisterCallback(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterCallback_CallbackParameter_IsCalledOnRegistryProperty()
        {
            // Arrange
            var registry = new Mock<ITransformArtifactRegistry>().Object;
            var target = new TransformStoreBuilder(registry);
            ITransformArtifactRegistry calledRegistry = null;

            // Act
            target.RegisterCallback(_ => calledRegistry = _);

            // Assert
            Assert.AreEqual(registry, calledRegistry);
        }
    }
}