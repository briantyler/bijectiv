// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateLabelSubtaskTests.cs" company="Bijectiv">
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
//   Defines the CreateLabelSubtaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="CreateLabelSubtask"/> class.
    /// </summary>
    [TestClass]
    public class CreateLabelSubtaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new CreateLabelSubtask(new Guid("AAA72466-8C99-4778-8787-F48730D57E69")).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CaategoryParameter_IsAssignedToLabelProperty()
        {
            // Arrange
            var category = new Guid("AAA72466-8C99-4778-8787-F48730D57E69");

            // Act
            var target = new CreateLabelSubtask(category);

            // Assert
            Assert.AreEqual(category, target.Category);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new CreateLabelSubtask(Guid.Empty);

            // Act
            target.Execute(null, Stub.Create<InjectionFragment>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new CreateLabelSubtask(Guid.Empty);

            // Act
            target.Execute(Stub.Create<InjectionScaffold>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_ValidParameters_AppropriateLabelIsAddedToScaffoldExpressions()
        {
            // Arrange
            var fragment = Stub.Create<InjectionFragment>();
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);

            var expressions = new List<Expression>();
            scaffoldMock.SetupGet(_ => _.Expressions).Returns(expressions);

            var category = new Guid("77BC093A-78D2-4CC6-8FA0-9B664D9F989E");
            var labelTarget = Expression.Label();
            scaffoldMock.Setup(_ => _.GetLabel(fragment, category)).Returns(labelTarget);

            var target = new CreateLabelSubtask(category);

            // Act
            target.Execute(scaffoldMock.Object, fragment);

            // Assert
            var label = (LabelExpression)expressions.Single();
            Assert.AreEqual(labelTarget, label.Target);
        }
    }
}