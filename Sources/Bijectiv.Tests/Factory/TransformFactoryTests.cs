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

namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.Tests.TestTools;
    using Bijectiv.Tests.TestTypes;
    using Bijectiv.Transforms;
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
            new TransformFactory(Stub.Create<IEnumerable<ITransformTask>>()).Naught();

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
        public void CreateInstance_TaskCollectionParameter_IsAssignedToTaskCollectionProperty()
        {
            // ReSharper disable PossibleMultipleEnumeration
            // Arrange
            var taskCollection = Stub.Create<IEnumerable<ITransformTask>>();

            // Act
            var target = new TransformFactory(taskCollection);

            // Assert
            Assert.AreEqual(taskCollection, target.TaskCollection);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_DefinitionParameterIsNull_Throws()
        {
            // Arrange
            var target = new TransformFactory(Stub.Create<IEnumerable<ITransformTask>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(Stub.Create<ITransformDefinitionRegistry>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_DefinitionRegistryParameterIsNull_Throws()
        {
            // Arrange
            var target = new TransformFactory(Stub.Create<IEnumerable<ITransformTask>>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null, new TransformDefinition(TestClass1.T, TestClass2.T));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_ReturnsDelegateTransform()
        {
            // Arrange
            var taskMock = new Mock<ITransformTask>();
            taskMock
                .Setup(_ => _.Execute(It.IsAny<TransformScaffold>()))
                .Callback((TransformScaffold s) => s.Expressions.Add(Expression.Constant(new object())));

            var taskCollection = new List<ITransformTask> { taskMock.Object };
            var target = new TransformFactory(taskCollection);

            // Act
            var result = target.Create(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T));

            // Assert
            Assert.IsInstanceOfType(result, typeof(DelegateTransform));
            Assert.AreEqual(TestClass1.T, result.Source);
            Assert.AreEqual(TestClass2.T, result.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ValidParameters_CreatesExpectedTransformDelegate()
        {
            // This test looks a bit unpleasant, but it hits all of the important characteristics of 
            // the target method:
            //  # Both parameters are accessed.
            //  # Tasks are executed in sequence.
            //  # Variables are referenced.
            //  # Expressions are compiled in sequence.

            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);
            var source = new TestClass1();
            var expectedTarget = new TestClass2();

            var transformContextMock = repository.Create<ITransformContext>();
            transformContextMock.Setup(_ => _.Resolve(TestClass2.T)).Returns(expectedTarget);

            var variable = Expression.Variable(TestClass1.T);
            var assignSourceTaskMock = repository.Create<ITransformTask>();
            assignSourceTaskMock
                .Setup(_ => _.Execute(It.IsAny<TransformScaffold>()))
                .Callback(
                    (TransformScaffold s) =>
                    {
                        s.Variables.Add(variable);
                        var assign = Expression.Assign(variable, Expression.Convert(s.SourceAsObject, TestClass1.T));
                        s.Expressions.Add(assign);
                    });

            var returnTaskMock = repository.Create<ITransformTask>();
            returnTaskMock
                .Setup(_ => _.Execute(It.IsAny<TransformScaffold>()))
                .Callback(
                    (TransformScaffold s) =>
                    {
                        var callResolve = Expression.Call(
                            s.TransformContext,
                            Reflect<ITransformContext>.Method(_ => _.Resolve(Placeholder.Of<Type>())),
                            Expression.Constant(s.Definition.Target));

                        var @return = Expression.Variable(typeof(object));
                        s.Variables.Add(@return);

                        var expression = Expression.IfThenElse(
                            Expression.Equal(variable, Expression.Constant(source)),
                            Expression.Assign(@return, callResolve),
                            Expression.Throw(Expression.Constant(new InvalidOperationException())));

                        s.Expressions.Add(expression);
                        s.Expressions.Add(@return);
                    });

            var taskCollection = new List<ITransformTask> { assignSourceTaskMock.Object, returnTaskMock.Object };
            var target = new TransformFactory(taskCollection);

            // Act
            var result = target.Create(
                Stub.Create<ITransformDefinitionRegistry>(), 
                new TransformDefinition(TestClass1.T, TestClass2.T));

            var actualTarget = result.Transform(source, transformContextMock.Object);

            // Assert
            repository.VerifyAll();
            Assert.AreEqual(expectedTarget, actualTarget);
        }
    }
}