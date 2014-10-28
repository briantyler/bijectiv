// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the CollectionInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CollectionInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class CollectionInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_StrategyParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new CollectionInjectionStore(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_StoreIsEmpty()
        {
            // Arrange

            // Act
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>());

            // Assert
            Assert.IsFalse(testTarget.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_StrategyParameter_IsAssignedToStrategyProperty()
        {
            // Arrange
            var strategy = Stub.Create<IInjectionResolutionStrategy>();

            // Act
            var testTarget = new CollectionInjectionStore(strategy);

            // Assert
            Assert.AreEqual(strategy, testTarget.Strategy);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_InjectionParameterIsNull_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsInjectionToStore()
        {
            // Arrange
            var injection1 = Stub.Create<IInjection>();
            var injection2 = Stub.Create<IInjection>();
            
            // Act
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>())
            {
                injection1, injection2
            };

            // Assert
            new[] { injection1, injection2 }.AssertSequenceEqual(testTarget);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_DefaultParameters_GetsEnumerator()
        {
            // Arrange
            var injection1 = Stub.Create<IInjection>();
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>()) { injection1 };

            // Act
            foreach (var item in (IEnumerable)testTarget)
            {
                // Assert
                Assert.AreEqual(injection1, item);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(null, TestClass2.T);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new CollectionInjectionStore(Stub.Create<IInjectionResolutionStrategy>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Resolve<IInjection>(TestClass1.T, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_DelegatesResolutionToStrategy()
        {
            // Arrange
            var injection = Stub.Create<IInjection>();
            var strategyMock = new Mock<IInjectionResolutionStrategy>(MockBehavior.Strict);

            var testTarget = new CollectionInjectionStore(strategyMock.Object);

            strategyMock.Setup(_ => _.Choose(TestClass1.T, TestClass2.T, testTarget.Injections)).Returns(injection);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = testTarget.Resolve<IInjection>(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(injection, result);
        }
    }
}