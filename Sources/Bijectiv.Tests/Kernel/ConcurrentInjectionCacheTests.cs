// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentInjectionCacheTests.cs" company="Bijectiv">
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
//   Defines the ConcurrentInjectionCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ConcurrentInjectionCache"/> class.
    /// </summary>
    [TestClass]
    public class ConcurrentInjectionCacheTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ConcurrentInjectionCache().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetOrAdd_StoreParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new ConcurrentInjectionCache();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.GetOrAdd<IInjection>(TestClass1.T, TestClass2.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAdd_InjectionIsNotInCache_ResolvesFromStore()
        {
            // Arrange
            var storeMock = new Mock<IInjectionStore>(MockBehavior.Strict);
            var injection = Stub.Create<ITransform>();
            storeMock
                .Setup(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T))
                .Returns(injection);

            var testTarget = new ConcurrentInjectionCache();

            // Act
            var result = testTarget.GetOrAdd<ITransform>(TestClass1.T, TestClass2.T, storeMock.Object);
            
            // Assert
            Assert.AreEqual(injection, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAdd_InjectionIsInCache_ReturnsFromCache()
        {
            // Arrange
            var storeMock = new Mock<IInjectionStore>(MockBehavior.Strict);
            var injection = Stub.Create<ITransform>();
            storeMock
                .Setup(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T))
                .Returns(injection);

            var testTarget = new ConcurrentInjectionCache();
            testTarget.GetOrAdd<ITransform>(TestClass1.T, TestClass2.T, storeMock.Object);

            // Act
            var result = testTarget.GetOrAdd<ITransform>(TestClass1.T, TestClass2.T, Stub.Create<IInjectionStore>());

            // Assert
            Assert.AreEqual(injection, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAdd_InjectionIsInNotCache_IsThreadSafe()
        {
            // Arrange
            var storeMock = new Mock<IInjectionStore>(MockBehavior.Strict);
            var injection = Stub.Create<ITransform>();
            storeMock
                .Setup(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T))
                .Returns(injection);

            var testTarget = new ConcurrentInjectionCache();

            var tasks = Enumerable
                .Range(0, 32)
                .Select(_ => new Task(() => testTarget.GetOrAdd<ITransform>(TestClass1.T, TestClass2.T, storeMock.Object)))
                .ToArray();

            tasks.ForEach(item => item.Start());

            Task.WaitAll(tasks);

            // Act
            var result = testTarget.GetOrAdd<ITransform>(TestClass1.T, TestClass2.T, Stub.Create<IInjectionStore>());

            // Assert
            Assert.AreEqual(injection, result);
            storeMock.Verify(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T), Times.Once);
        }
    }
}