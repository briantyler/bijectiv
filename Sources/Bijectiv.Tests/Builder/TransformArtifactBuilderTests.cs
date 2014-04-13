// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformArtifactBuilderTests.cs" company="Bijectiv">
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
//   Defines the TransformArtifactBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Builder
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Tests.TestTools;
    using Bijectiv.Tests.TestTypes;

    using JetBrains.Annotations;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformArtifactBuilder{TSource,TTarget}"/> class.
    /// </summary>
    [TestClass]
    public class TransformArtifactBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_ArtifactParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformArtifactBuilder<TestClass1, TestClass2>(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ArtifactParameter_IsAssignedToArtifactProperty()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);

            // Act
            var target = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Assert
            Assert.AreEqual(artifact, target.Artifact);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateInstance_SourceTypeMismatch_Throws()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);

            // Act
            new TransformArtifactBuilder<BaseTestClass1, TestClass2>(artifact).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateInstance_TargetTypeMismatch_Throws()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);

            // Act
            new TransformArtifactBuilder<TestClass1, BaseTestClass2>(artifact).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Activate_DefaultParameters_AddsActivatorFragmentToArtifact()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            builder.Activate();

            // Assert
            Assert.IsInstanceOfType(artifact.Single(), typeof(ActivateFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Activate_DefaultParameters_ReturnsF()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            var result = builder.Activate();

            // Assert
            AssertBuilder(result, typeof(ITransformArtifactBuilderF<TestClass1, TestClass2>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void DefaultFactory_DefaultParameters_AddsDefaultFactoryFragmentToArtifact()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            builder.DefaultFactory();

            // Assert
            Assert.IsInstanceOfType(artifact.Single(), typeof(DefaultFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void DefaultFactory_DefaultParameters_ReturnsF()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            var result = builder.DefaultFactory();

            // Assert
            AssertBuilder(result, typeof(ITransformArtifactBuilderF<TestClass1, TestClass2>));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomFactory_FactoryParameterNull_Throws()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            builder.CustomFactory(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AddsCustomFactoryFragmentToArtifact()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            builder.CustomFactory(p => default(TestClass2));

            // Assert
            Assert.IsInstanceOfType(artifact.Single(), typeof(CustomFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AssignsFactoryToCustomFactoryFragment()
        {
            // Arrange
            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = p => default(TestClass2);
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            builder.CustomFactory(factory);

            // Assert
            Assert.AreEqual(factory, ((CustomFactoryFragment)artifact.Single()).Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_ReturnsF()
        {
            // Arrange
            var artifact = new TransformArtifact(TestClass1.T, TestClass2.T);
            var builder = new TransformArtifactBuilder<TestClass1, TestClass2>(artifact);

            // Act
            var result = builder.CustomFactory(p => default(TestClass2));

            // Assert
            AssertBuilder(result, typeof(ITransformArtifactBuilderF<TestClass1, TestClass2>));
        }

        [UsedImplicitly]
        private static void AssertBuilder<TSource, TTarget>(
            ITransformArtifactBuilder<TSource, TTarget> builder,
            Type type)
        {
            Assert.AreEqual(type, typeof(ITransformArtifactBuilder<TSource, TTarget>));
        }

        [UsedImplicitly]
        private static void AssertBuilder<TSource, TTarget>(
            ITransformArtifactBuilderF<TSource, TTarget> builder,
            Type type)
        {
            Assert.AreEqual(type, typeof(ITransformArtifactBuilderF<TSource, TTarget>));
        }
    }
}