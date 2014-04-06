// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleTransformStoreTests.cs" company="Bijectiv">
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
//   Defines the ConvertibleTransformStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Stores
{
    using System;

    using Bijectiv.Stores;
    using Bijectiv.Tests.Tools;
    using Bijectiv.Tests.Transforms;
    using Bijectiv.Transforms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="ConvertibleTransformStore"/> class.
    /// </summary>
    [TestClass]
    public class ConvertibleTransformStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ConvertibleTransformStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            // Act
            target.Resolve(null, typeof(int));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            // Act
            target.Resolve(typeof(int), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameterIsNotConvertibleTarget_ReturnsNull()
        {
            // Arrange
            var target = new ConvertibleTransformStore();
            
            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                // Act
                var result = target.Resolve(sourceType, typeof(object));

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotConvertibleSource_ReturnsNull()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
            {
                // Act
                var result = target.Resolve(typeof(object), targetType);

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsConvertibleTransform()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve(sourceType, targetType);

                    // Assert
                    Assert.IsInstanceOfType(result, typeof(ConvertibleTransform));
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_Iconvertible_IsAssignedToConvertibleTransformSourceProperty()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve(sourceType, targetType);

                    // Assert
                    Assert.AreEqual(typeof(IConvertible), result.Source);
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameter_IsAssignedToConvertibleTransformSourceProperty()
        {
            // Arrange
            var target = new ConvertibleTransformStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve(sourceType, targetType);

                    // Assert
                    Assert.AreEqual(targetType, result.Target);
                }
            }
        }
    }
}