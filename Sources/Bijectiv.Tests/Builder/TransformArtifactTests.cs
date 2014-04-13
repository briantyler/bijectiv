// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformArtifactTests.cs" company="Bijectiv">
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
//   Defines the TransformArtifactTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Builder
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Tests.TestTools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformArtifact"/> class.
    /// </summary>
    [TestClass]
    public class TransformArtifactTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformArtifact(typeof(object), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ArtifactIsEmpty()
        {
            // Arrange

            // Act
            var target = new TransformArtifact(typeof(object), typeof(object));

            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformArtifact(null, typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformArtifact(typeof(object), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = new TransformArtifact(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(int), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new TransformArtifact(typeof(object), typeof(int));

            // Assert
            Assert.AreEqual(typeof(int), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
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
            var fragment = Stub.Create<TransformFragment>(typeof(object), typeof(object));

            // Act
            target.Add(fragment);

            // Assert
            Assert.AreEqual(fragment, target.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_FragmentSourceMismatch_Throws()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<TransformFragment>(typeof(int), typeof(object));

            // Act
            target.Add(fragment);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_FragmentTargetMismatch_Throws()
        {
            // Arrange
            var target = CreateTarget();
            var fragment = Stub.Create<TransformFragment>(typeof(object), typeof(int));

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
            var fragment = Stub.Create<TransformFragment>(typeof(object), typeof(object));
            target.Add(fragment);
            
            // Act
            foreach (var item in (IEnumerable)target)
            {
                // Assert
                Assert.AreEqual(fragment, item);
            }
        }

        private static TransformArtifact CreateTarget()
        {
            return new TransformArtifact(typeof(object), typeof(object));
        }
    }
}