// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullAutoTransformStrategyTests.cs" company="Bijectiv">
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
//   Defines the NullAutoTransformStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="NullAutoInjectionStrategy"/> class.
    /// </summary>
    [TestClass]
    public class NullAutoTransformStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NullAutoInjectionStrategy().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_AnyParameters_ReturnsTrue()
        {
            // Arrange
            var target = new NullAutoInjectionStrategy();
            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(null, null, out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_AnyParameters_SetsSourceMemberToNull()
        {
            // Arrange
            var target = new NullAutoInjectionStrategy();
            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(null, null, out sourceMember);

            // Assert
            Assert.IsNull(sourceMember);
        }
    }
}