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
#pragma warning disable 1720
namespace Bijectiv.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Tests.TestTypes;

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
            default(TransformStoreBuilder).Register<TestClass1, TestClass2>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_ArtifactIsAddedToRegistry()
        {
            // Arrange
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock.Setup(_ => _.Add(It.IsAny<TransformArtifact>()));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedArtifactIsEmpty()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var artifact = artifacts.Single();
            Assert.IsFalse(artifact.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedArtifactHasCorrectSource()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var artifact = artifacts.Single();
            Assert.AreEqual(TestClass1.T, artifact.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedArtifactHasCorrectTarget()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var artifact = artifacts.Single();
            Assert.AreEqual(TestClass2.T, artifact.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterInherited_BuilderParameterIsNull_Throws()
        {
            // Arrange

            // Act
            default(TransformStoreBuilder)
                .RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_ArtifactIsAddedToRegistry()
        {
            // Arrange
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);
            registryMock.Setup(_ => _.Add(It.IsAny<TransformArtifact>()));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedArtifactHasCorrectSource()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var artifact = artifacts.Single();
            Assert.AreEqual(DerivedTestClass1.T, artifact.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedArtifactHasCorrectTarget()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var artifact = artifacts.Single();
            Assert.AreEqual(DerivedTestClass2.T, artifact.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedArtifactHasContainsInheritsFragment()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = artifacts.Single().Single();
            Assert.IsInstanceOfType(fragment, typeof(InheritsFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_InheritsFragmentHasCorrectSourceBase()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = (InheritsFragment)artifacts.Single().Single();
            Assert.AreEqual(BaseTestClass1.T, fragment.SourceBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_InheritsFragmentHasCorrectTargetBase()
        {
            // Arrange
            var artifacts = new List<TransformArtifact>();
            var registryMock = new Mock<ITransformArtifactRegistry>(MockBehavior.Strict);
            var target = new TransformStoreBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Add(It.IsAny<TransformArtifact>()))
                .Callback((TransformArtifact a) => artifacts.Add(a));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = (InheritsFragment)artifacts.Single().Single();
            Assert.AreEqual(BaseTestClass2.T, fragment.TargetBase);
        }
    }
}