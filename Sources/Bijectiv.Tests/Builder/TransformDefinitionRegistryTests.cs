// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformDefinitionRegistryTests.cs" company="Bijectiv">
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
//   Defines the TransformDefinitionRegistryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable UseObjectOrCollectionInitializer
namespace Bijectiv.Tests.Builder
{
    using System.Collections;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformDefinitionRegistry"/> class.
    /// </summary>
    [TestClass]
    public class TransformDefinitionRegistryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformDefinitionRegistry().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_CollectionEmpty()
        {
            // Arrange
            
            // Act
            var target = new TransformDefinitionRegistry();
            
            // Assert
            Assert.IsFalse(target.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CollectionConstructor_ItemsAdded()
        {
            // Arrange

            // Act
            var target = new TransformDefinitionRegistry
            {
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object)),
            };

            // Assert
            Assert.AreEqual(3, target.Count());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumerator_DefaultParameters_EnumeratesThroughCollection()
        {
            // Arrange
            var definitions = new[]
            {
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object))
            };

            // Act
            var target = new TransformDefinitionRegistry
            {
                definitions[0],
                definitions[1],
                definitions[2],
            };

            // Assert
            Assert.IsTrue(definitions.SequenceEqual(target));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEnumeratorWeak_DefaultParameters_EnumeratesThroughCollection()
        {
            // Arrange
            var definitions = new[]
            {
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object)),
                new TransformDefinition(typeof(object), typeof(object))
            };

            // Act
            var target = new TransformDefinitionRegistry
            {
                definitions[0],
                definitions[1],
                definitions[2],
            };

            // Assert
            var index = 0;
            foreach (var obj in (IEnumerable)target)
            {
                Assert.AreEqual(definitions[index++], obj);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Add_DefinitionParameterIsNull_Throws()
        {
            // Arrange
            var target = new TransformDefinitionRegistry();

            // Act
            target.Add(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_DefinitionParameterIsAddedToCollection()
        {
            // Arrange
            var target = new TransformDefinitionRegistry();
            var definition = new TransformDefinition(typeof(object), typeof(object));

            // Act
            target.Add(definition);

            // Assert
            Assert.AreEqual(definition, target.Single());
        }
    }
}