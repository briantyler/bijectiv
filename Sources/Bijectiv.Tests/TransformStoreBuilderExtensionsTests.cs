// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformStoreBuilderExtensionsTests.cs" company="Bijectiv">
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
//   Defines the TransformStoreBuilderExtensionsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests
{
    using System;

    using Bijectiv.Builder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TransformStoreBuilderExtensions"/> class.
    /// </summary>
    [TestClass]
    public class TransformStoreBuilderExtensionsTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_BuilderParameterIsNull_Throws()
        {
            // Arrange

            // Act
            default(TransformStoreBuilder).Register<object, object>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_ExpectedResult()
        {
            // Arrange
            var registryMock = new Mock<ITransformArtifactRegistry>();
            var builder = new TransformStoreBuilder(registryMock.Object);

            // Act
            builder.Register<object, object>();

            // Assert
            registryMock.Verify(_ => _.Add(It.IsAny<TransformArtifact>()));
        }
    }
}