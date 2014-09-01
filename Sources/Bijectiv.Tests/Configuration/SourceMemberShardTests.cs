﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberShardTests.cs" company="Bijectiv">
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
//   Defines the SourceMemberShardTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="SourceMemberShard"/> class.
    /// </summary>
    [TestClass]
    public class SourceMemberShardTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            Stub.Create<SourceMemberShard>(TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id), true);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectSourceParameter_IsAssignedToInjectSourceProperty()
        {
            // Arrange

            // Act
            var target = new Mock<SourceMemberShard>(
                TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id), true)
                {
                    CallBase = true
                }
                .Object;

            // Assert
            Assert.IsTrue(target.InjectSource);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_ShardCategoryPropertyIs()
        {
            // Arrange

            // Act
            var target = new Mock<SourceMemberShard>(
                TestClass1.T, TestClass2.T, Reflect<TestClass2>.Property(_ => _.Id), true)
                {
                    CallBase = true
                }
                .Object;

            // Assert
            Assert.AreEqual(LegendaryShards.Source, target.ShardCategory);
        }
    }
}