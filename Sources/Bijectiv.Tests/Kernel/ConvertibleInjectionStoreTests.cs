// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertibleInjectionStoreTests.cs" company="Bijectiv">
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
//   Defines the ConvertibleInjectionStoreTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Kernel
{
    using System;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="ConvertibleInjectionStore"/> class.
    /// </summary>
    [TestClass]
    public class ConvertibleInjectionStoreTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ConvertibleInjectionStore().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_SourceParameterIsNull_Throws()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            // Act
            target.Resolve<IInjection>(null, typeof(int));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Resolve_TargetParameterIsNull_Throws()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            // Act
            target.Resolve<IInjection>(typeof(int), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameterIsNotConvertibleTarget_ReturnsNull()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();
            
            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                // Act
                var result = target.Resolve<IInjection>(sourceType, typeof(object));

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_SourceParameterIsNotConvertibleSource_ReturnsNull()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
            {
                // Act
                var result = target.Resolve<IInjection>(typeof(object), targetType);

                // Assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_ValidParameters_ReturnsConvertibleInjection()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve<IInjection>(sourceType, targetType);

                    // Assert
                    Assert.IsInstanceOfType(result, typeof(ConvertibleInjection));
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TransformTypeParameter_ReturnsConvertibleInjection()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve<ITransform>(sourceType, targetType);

                    // Assert
                    Assert.IsInstanceOfType(result, typeof(ConvertibleInjection));
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_MergeTypeParameter_ReturnsConvertibleInjection()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve<IMerge>(sourceType, targetType);

                    // Assert
                    Assert.IsInstanceOfType(result, typeof(ConvertibleInjection));
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_Iconvertible_IsAssignedToConvertibleInjectionSourceProperty()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve<IInjection>(sourceType, targetType);

                    // Assert
                    Assert.AreEqual(typeof(IConvertible), result.Source);
                }
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Resolve_TargetParameter_IsAssignedToConvertibleInjectionSourceProperty()
        {
            // Arrange
            var target = new ConvertibleInjectionStore();

            foreach (var sourceType in TypeClasses.ConvertibleSourceTypes)
            {
                foreach (var targetType in TypeClasses.ConvertibleTargetTypes)
                {
                    // Act
                    var result = target.Resolve<IInjection>(sourceType, targetType);

                    // Assert
                    Assert.AreEqual(targetType, result.Target);
                }
            }
        }
    }
}