// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeMembersTaskTests.cs" company="Bijectiv">
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
//   Defines the InitializeMembersTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InitializeMembersTask"/> class.
    /// </summary>
    [TestClass]
    public class InitializeMembersTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_ReflectionGatewayParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InitializeMembersTask(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InitializeMembersTask(Stub.Create<IReflectionGateway>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ReflectionGatewayParameter_IsAssignedToReflectionGatewayProperty()
        {
            // Arrange
            var reflectionGateway = Stub.Create<IReflectionGateway>();

            // Act
            var target = new InitializeMembersTask(reflectionGateway);

            // Assert
            Assert.AreEqual(reflectionGateway, target.ReflectionGateway);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new InitializeMembersTask(Stub.Create<IReflectionGateway>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ScaffoldProcessedTargetMembers_IsCleared()
        {
            // Arrange
            var reflectionGatewayMock = new Mock<IReflectionGateway>(MockBehavior.Loose);
            reflectionGatewayMock
                .Setup(_ => _.GetFields(It.IsAny<Type>(), It.IsAny<ReflectionOptions>()))
                .Returns(new[] { Stub.Create<FieldInfo>() });
            reflectionGatewayMock
                .Setup(_ => _.GetProperties(It.IsAny<Type>(), It.IsAny<ReflectionOptions>()))
                .Returns(new[] { Stub.Create<PropertyInfo>() });

            var scaffold = CreateScaffold();
            scaffold.ProcessedTargetMembers.Add(Reflect<TestClass1>.Property(_ => _.Id));

            var target = new InitializeMembersTask(reflectionGatewayMock.Object);

            // Act
            target.Execute(scaffold);

            // Assert
            Assert.IsFalse(scaffold.ProcessedTargetMembers.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_SourceMembers_AreAdded()
        {
            // Arrange
            var reflectionGatewayMock = new Mock<IReflectionGateway>(MockBehavior.Loose);

            var member1 = Stub.Create<FieldInfo>();
            var member2 = Stub.Create<PropertyInfo>();

            reflectionGatewayMock
                .Setup(_ => _.GetFields(TestClass1.T, ReflectionOptions.CanRead))
                .Returns(new[] { member1 })
                .Verifiable();
            reflectionGatewayMock
                .Setup(_ => _.GetProperties(TestClass1.T, ReflectionOptions.CanRead))
                .Returns(new[] { member2 })
                .Verifiable();

            var scaffold = CreateScaffold();
            scaffold.SourceMembers.Add(Stub.Create<MemberInfo>());

            var target = new InitializeMembersTask(reflectionGatewayMock.Object);

            // Act
            target.Execute(scaffold);

            // Assert
            reflectionGatewayMock.Verify();
            Assert.AreEqual(2, scaffold.SourceMembers.Count());
            Assert.IsTrue(scaffold.SourceMembers.Contains(member1));
            Assert.IsTrue(scaffold.SourceMembers.Contains(member2));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_TargetMembers_AreAdded()
        {
            // Arrange
            var reflectionGatewayMock = new Mock<IReflectionGateway>(MockBehavior.Loose);

            var member1 = Stub.Create<FieldInfo>();
            var member2 = Stub.Create<PropertyInfo>();

            reflectionGatewayMock
                .Setup(_ => _.GetFields(TestClass2.T, ReflectionOptions.None))
                .Returns(new[] { member1 })
                .Verifiable();
            reflectionGatewayMock
                .Setup(_ => _.GetProperties(TestClass2.T, ReflectionOptions.None))
                .Returns(new[] { member2 })
                .Verifiable();

            var scaffold = CreateScaffold();
            scaffold.TargetMembers.Add(Stub.Create<MemberInfo>());

            var target = new InitializeMembersTask(reflectionGatewayMock.Object);

            // Act
            target.Execute(scaffold);

            // Assert
            reflectionGatewayMock.Verify();
            Assert.AreEqual(2, scaffold.TargetMembers.Count());
            Assert.IsTrue(scaffold.TargetMembers.Contains(member1));
            Assert.IsTrue(scaffold.TargetMembers.Contains(member2));
        }

        private static InjectionScaffold CreateScaffold()
        {
            return new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Stub.Create<Expression>(),
                Stub.Create<Expression>());
        }
    }
}