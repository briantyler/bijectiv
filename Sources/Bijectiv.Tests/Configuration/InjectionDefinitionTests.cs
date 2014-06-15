// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionDefinitionTests.cs" company="Bijectiv">
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
//   Defines the InjectionDefinitionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Configuration
{
    using System.Collections;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InjectionDefinition"/> class.
    /// </summary>
    [TestClass]
    public class InjectionDefinitionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionDefinition(TestClass1.T, TestClass1.T).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DefintionIsEmpty()
        {
            // Arrange

            // Act
            var target = new InjectionDefinition(TestClass1.T, TestClass1.T);

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionDefinition(null, TestClass1.T).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionDefinition(TestClass1.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FragmentsParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionDefinition(TestClass1.T, TestClass1.T, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = new InjectionDefinition(TestClass2.T, TestClass1.T);

            // Assert
            Assert.AreEqual(TestClass2.T, target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Assert
            Assert.AreEqual(TestClass2.T, target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentsParameter_IsCopiedToFragmentsProperty()
        {
            // Arrange
            var fragments = new[]
            {
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>()
            };

            // Act
            var target = new InjectionDefinition(TestClass1.T, TestClass1.T, fragments);

            // Assert
            Assert.IsTrue(fragments.SequenceEqual(target));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CollectionInitializer_IsCopiedToToFragmentsProperty()
        {
            // Arrange
            var fragments = new[]
            {
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>(),
                Stub.Fragment<TestClass1, TestClass2>()
            };

            // Act
            var target = new InjectionDefinition(TestClass1.T, TestClass2.T)
            {
                fragments[0], fragments[1], fragments[2]
            };

            // Assert
            Assert.IsTrue(fragments.SequenceEqual(target));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = CreateTarget();

            // Act
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidFragment_IsAddedToCollection()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<InjectionFragment>(TestClass1.T, TestClass1.T);

            // Act
            target.Add(fragment);

            // Assert
            Assert.AreEqual(fragment, target.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_FragmentSourceMismatch_Throws()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<InjectionFragment>(TestClass2.T, TestClass1.T);

            // Act
            target.Add(fragment);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_FragmentTargetMismatch_Throws()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<InjectionFragment>(TestClass1.T, TestClass2.T);

            // Act
            target.Add(fragment);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Collection_ViewedAsNonGeneric_IsExposed()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<InjectionFragment>(TestClass1.T, TestClass1.T);
            target.Add(fragment);
            
            // Act
            foreach (var item in (IEnumerable)target)
            {
                // Assert
                Assert.AreEqual(fragment, item);
            }
        }

        private static InjectionDefinition CreateTarget()
        {
            return new InjectionDefinition(TestClass1.T, TestClass1.T);
        }
    }
}