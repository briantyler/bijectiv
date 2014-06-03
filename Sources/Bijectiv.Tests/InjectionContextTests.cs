// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionContextTests.cs" company="Bijectiv">
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
//   Defines the InjectionContextTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests
{
    using System.Globalization;

    using Bijectiv.Builder;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InjectionContext"/> class.
    /// </summary>
    [TestClass]
    public class InjectionContextTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_CultureParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionContext(null, t => new object(), Stub.Create<IInjectionKernel>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ResolveDelegateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionContext(CultureInfo.InvariantCulture, null, Stub.Create<IInjectionKernel>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InjectionKernelParameterParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionContext(CultureInfo.InvariantCulture, t => new object(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionContext(
                CultureInfo.InvariantCulture, t => new object(), Stub.Create<IInjectionKernel>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetCacheProperty_IsNotNull()
        {
            // Arrange

            // Act
            var target = new InjectionContext(
                CultureInfo.InvariantCulture, t => new object(), Stub.Create<IInjectionKernel>());

            // Assert
            Assert.IsNotNull(target.TargetCache);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CultureParameter_IsAssignedToCultureProperty()
        {
            // Arrange
            var culture = CultureInfo.CreateSpecificCulture("en-GB");

            // Act
            var target = new InjectionContext(culture, t => new object(), Stub.Create<IInjectionKernel>());

            // Assert
            Assert.AreEqual(culture, target.Culture);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectionKernelParameter_ProvidesInjectionStoreProperty()
        {
            // Arrange
            var kernelMock = new Mock<IInjectionKernel>(MockBehavior.Strict);
            var injectionStore = Stub.Create<IInjectionStore>();
            kernelMock.SetupGet(_ => _.Store).Returns(injectionStore);

            // Act
            var target = new InjectionContext(CultureInfo.InvariantCulture, t => new object(), kernelMock.Object);

            // Assert
            Assert.AreEqual(injectionStore, target.InjectionStore);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectionKernelParameter_ProvidesInstanceRegistryProperty()
        {
            // Arrange
            var kernelMock = new Mock<IInjectionKernel>(MockBehavior.Strict);
            var instanceRegistry = Stub.Create<IInstanceRegistry>();
            kernelMock.SetupGet(_ => _.Registry).Returns(instanceRegistry);

            // Act
            var target = new InjectionContext(CultureInfo.InvariantCulture, t => new object(), kernelMock.Object);

            // Assert
            Assert.AreEqual(instanceRegistry, target.InstanceRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TypeParameterIsNull_Throws()
        {
            // Arrange
            var target = new InjectionContext(
                CultureInfo.InvariantCulture, t => new object(), Stub.Create<IInjectionKernel>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Resolve(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_InvokesResolveDelegate()
        {
            // Arrange
            var expected = new object();
            var target = new InjectionContext(
                CultureInfo.InvariantCulture, 
                t => t == TestClass1.T ? expected : null,
                Stub.Create<IInjectionKernel>());

            // Act
            var result = target.Resolve(TestClass1.T);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}