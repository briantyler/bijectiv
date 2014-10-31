// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the CachingInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class CachingInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_UnderlyingStoreParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new CachingInjectionStore(null, Stub.Create<IInjectionCache>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_CacheParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new CachingInjectionStore(Stub.Create<IInjectionStore>(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CachingInjectionStore(Stub.Create<IInjectionStore>(), Stub.Create<IInjectionCache>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_UnderlyingStoreParameter_IsAssignedToUnderlyingStoreProperty()
        {
            // Arrange
            var underlyingStore = Stub.Create<IInjectionStore>();

            // Act
            var testTarget = new CachingInjectionStore(underlyingStore, Stub.Create<IInjectionCache>());

            // Assert
            Assert.AreEqual(underlyingStore, testTarget.UnderlyingStore);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CacheParameter_IsAssignedToCacheProperty()
        {
            // Arrange
            var cache = Stub.Create<IInjectionCache>();

            // Act
            var testTarget = new CachingInjectionStore(Stub.Create<IInjectionStore>(), cache);

            // Assert
            Assert.AreEqual(cache, testTarget.Cache);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new CachingInjectionStore(Stub.Create<IInjectionStore>(), Stub.Create<IInjectionCache>());
            
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(null, TestClass1.T);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new CachingInjectionStore(Stub.Create<IInjectionStore>(), Stub.Create<IInjectionCache>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(TestClass1.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ResolvesFromCache()
        {
            // Arrange
            var underlyingStore = Stub.Create<IInjectionStore>();
            var cacheMock = new Mock<IInjectionCache>(MockBehavior.Strict);
            var injection = Stub.Create<IInjection>();
            cacheMock
                .Setup(_ => _.GetOrAdd<IInjection>(TestClass1.T, TestClass2.T, underlyingStore))
                .Returns(injection);

            var testTarget = new CachingInjectionStore(underlyingStore, cacheMock.Object);

            // Act
            var result = testTarget.Resolve<IInjection>(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(injection, result);
        }
    }
}