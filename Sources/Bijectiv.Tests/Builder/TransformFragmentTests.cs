﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformFragmentTests.cs" company="Bijectiv">
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
//   Defines the TransformFragmentTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Builder
{
    using System;
    using System.Reflection;

    using Bijectiv.Builder;
    using Bijectiv.Tests.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformFragment"/> class.
    /// </summary>
    [TestClass]
    public class TransformFragmentTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            Abstract.Instance<TransformFragment>(typeof(object), typeof(object));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            try
            {
                Abstract.Instance<TransformFragment>(null, typeof(object));
            }   
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            try
            {
                Abstract.Instance<TransformFragment>(typeof(object), null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            
            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = Abstract.Instance<TransformFragment>(typeof(int), typeof(object));

            // Assert
            Assert.AreEqual(typeof(int), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = Abstract.Instance<TransformFragment>(typeof(object), typeof(int));

            // Assert
            Assert.AreEqual(typeof(int), target.Target);
        }
    }
}