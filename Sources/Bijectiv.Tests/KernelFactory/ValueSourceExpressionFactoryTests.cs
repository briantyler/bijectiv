// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueSourceExpressionFactoryTests.cs" company="Bijectiv">
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
//   Defines the ValueSourceExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="ValueSourceExpressionFactory"/> class.
    /// </summary>
    [TestClass]
    public class ValueSourceExpressionFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new ValueSourceExpressionFactory().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new ValueSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null, Stub.Create<MemberFragment>(), Stub.Create<ValueSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new ValueSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(Stub.Create<InjectionScaffold>(), null, Stub.Create<ValueSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new ValueSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsShardValueAsConstantExpression()
        {
            // Arrange
            var target = new ValueSourceExpressionFactory();

            var mockShard = new Mock<ValueSourceMemberShard>(MockBehavior.Strict);
            var value = new object();
            mockShard.SetupGet(_ => _.Value).Returns(value);

            // Act
            var result = target.Create(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), mockShard.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(value, ((ConstantExpression)result).Value);
        }
    }
}