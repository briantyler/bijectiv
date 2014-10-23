// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformFactoryTests.cs" company="Bijectiv">
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
//   Defines the TransformFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class TransformFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformFactory(Stub.Create<IEnumerable<IInjectionTask>>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TaskCollectionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new TransformFactory(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TasksParameter_IsAssignedToTasksProperty()
        {
            // ReSharper disable PossibleMultipleEnumeration
            // Arrange
            var tasks = Stub.Create<IEnumerable<IInjectionTask>>();

            // Act
            var testTarget = new TransformFactory(tasks);

            // Assert
            Assert.AreEqual(tasks, testTarget.Tasks);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_DefinitionParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new TransformFactory(Stub.Create<IEnumerable<IInjectionTask>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Create(Stub.Create<IInstanceRegistry>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_InstanceRegistryParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new TransformFactory(Stub.Create<IEnumerable<IInjectionTask>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Create(null, new InjectionDefinition(TestClass1.T, TestClass2.T));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsDelegateTransform()
        {
            // Arrange
            var taskMock = new Mock<IInjectionTask>();
            taskMock
                .Setup(_ => _.Execute(It.IsAny<InjectionScaffold>()))
                .Callback((InjectionScaffold s) => s.Expressions.Add(Expression.Constant(new object())));

            var taskCollection = new List<IInjectionTask> { taskMock.Object };
            var testTarget = new TransformFactory(taskCollection);

            // Act
            var result = testTarget.Create(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T));

            // Assert
            Assert.IsInstanceOfType(result, typeof(DelegateTransform));
            Assert.AreEqual(TestClass1.T, result.Source);
            Assert.AreEqual(TestClass2.T, result.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_CreatesExpectedTransformDelegate()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = new TestClass1();
            var target = new TestClass2();
            var injectionContext = Stub.Create<IInjectionContext>();

            Expression<Action<IInjectionContext>> validateContext = o => Assert.AreEqual(injectionContext, o);
            Expression<Action<object>> validateSource = o => Assert.AreEqual(source, o);

            var validateParametersTaskMock = repository.Create<IInjectionTask>();
            validateParametersTaskMock
                .Setup(_ => _.Execute(It.IsAny<InjectionScaffold>()))
                .Callback(
                    (InjectionScaffold s) =>
                    {
                        var validateContextExpression =
                            new ParameterExpressionVisitor(validateContext.Parameters[0], s.InjectionContext)
                            .Visit(validateContext.Body);
                        var validateSourceExpression =
                            new ParameterExpressionVisitor(validateSource.Parameters[0], s.SourceAsObject)
                            .Visit(validateSource.Body);

                        s.Expressions.Add(validateContextExpression);
                        s.Expressions.Add(validateSourceExpression);
                    });

            var returnTaskMock = new Mock<IInjectionTask>();
            returnTaskMock
                .Setup(_ => _.Execute(It.IsAny<InjectionScaffold>()))
                .Callback(
                    (InjectionScaffold s) =>
                    {
                        var variable = Expression.Variable(typeof(TestClass2));
                        var assign = Expression.Assign(variable, Expression.Constant(target));
                        s.Variables.Add(variable);
                        s.Expressions.Add(assign);
                        s.Expressions.Add(variable);
                    });

            var taskCollection = new List<IInjectionTask>
            {
                validateParametersTaskMock.Object, 
                returnTaskMock.Object
            };
            var testTarget = new TransformFactory(taskCollection);

            // Act
            var injection = testTarget.Create(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T));

            var result = injection.Transform(source, injectionContext, null);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(target, result);
        }
    }
}