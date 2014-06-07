// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernelTests.cs" company="Bijectiv">
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
//   Defines the InjectionKernelTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelBuilder
{
    using Bijectiv.KernelBuilder;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InjectionKernel"/> class.
    /// </summary>
    [TestClass]
    public class InjectionKernelTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_StoreParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionKernel(null, Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_RegistryParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionKernel(Stub.Create<IInjectionStore>(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionKernel(Stub.Create<IInjectionStore>(), Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_StoreParameter_IsAssignedToStoreProperty()
        {
            // Arrange
            var store = Stub.Create<IInjectionStore>();

            // Act
            var target = new InjectionKernel(store, Stub.Create<IInstanceRegistry>());

            // Assert
            Assert.AreEqual(store, target.Store);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_RegistryParameter_IsAssignedToStoreRegistry()
        {
            // Arrange
            var registry = Stub.Create<IInstanceRegistry>();

            // Act
            var target = new InjectionKernel(Stub.Create<IInjectionStore>(), registry);

            // Assert
            Assert.AreEqual(registry, target.Registry);
        }
    }
}