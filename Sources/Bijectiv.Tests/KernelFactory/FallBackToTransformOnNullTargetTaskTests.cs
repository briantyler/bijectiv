// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FallBackToTransformOnNullTargetTaskTests.cs" company="Bijectiv">
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
//   Defines the FallBackToTransformOnNullTargetTaskTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="FallBackToTransformOnNullTargetTaskTests"/> class.
    /// </summary>
    [TestClass]
    public class FallBackToTransformOnNullTargetTaskTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new FallBackToTransformOnNullTargetTask().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Execute_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new FallBackToTransformOnNullTargetTask();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Execute(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_TargetIsNotNull_DoesNothing()
        {
            // Arrange
            var scaffold = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Constant(new TestClass1(), typeof(object)),
                Expression.Constant(Stub.Create<IInjectionContext>()));

            scaffold.TargetAsObject = scaffold.GetVariable("targetAsObject", typeof(object));
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(new object())));

            var target = new FallBackToTransformOnNullTargetTask();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(null, LegendaryLabels.End)));
            Expression
                .Lambda<Action>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile()();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_TargetIsNullAndSourceIsNull_FallsBackToTransform()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var targetInstance = new TestClass2();
            var transformMock = repository.Create<ITransform>();
            var storeMock = repository.Create<IInjectionStore>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(TestClass1.T, TestClass2.T)).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(null, contextMock.Object, null)).Returns(targetInstance);

            var scaffold = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Constant(null, typeof(object)),
                Expression.Constant(contextMock.Object));

            scaffold.TargetAsObject = scaffold.GetVariable("targetAsObject", typeof(object));
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(null)));

            var target = new FallBackToTransformOnNullTargetTask();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(null, LegendaryLabels.End)));
            scaffold.Expressions.Add(scaffold.TargetAsObject);

            var result = Expression
                .Lambda<Func<object>>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile()();

            repository.VerifyAll();
            Assert.AreEqual(targetInstance, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_TargetIsNullAndSourceIsNotNull_FallsBackToTransform()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new DerivedTestClass1();
            var targetInstance = new TestClass2();
            var transformMock = repository.Create<ITransform>();
            var storeMock = repository.Create<IInjectionStore>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(DerivedTestClass1.T, TestClass2.T)).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(sourceInstance, contextMock.Object, null)).Returns(targetInstance);

            var scaffold = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(BaseTestClass1.T, TestClass2.T),
                Expression.Constant(sourceInstance, typeof(object)),
                Expression.Constant(contextMock.Object));

            scaffold.TargetAsObject = scaffold.GetVariable("targetAsObject", typeof(object));
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(null)));

            var target = new FallBackToTransformOnNullTargetTask();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(null, LegendaryLabels.End)));
            scaffold.Expressions.Add(scaffold.TargetAsObject);

            var result = Expression
                .Lambda<Func<object>>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile()();

            repository.VerifyAll();
            Assert.AreEqual(targetInstance, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Execute_TargetIsNull_FallsBackWithReplacePostMergeAction()
        {
            // Arrange
            var repository = new MockRepository(MockBehavior.Strict);

            var sourceInstance = new DerivedTestClass1();
            var targetInstance = new TestClass2();
            var transformMock = repository.Create<ITransform>();
            var storeMock = repository.Create<IInjectionStore>();
            var contextMock = repository.Create<IInjectionContext>();

            contextMock.SetupGet(_ => _.InjectionStore).Returns(storeMock.Object);
            storeMock.Setup(_ => _.Resolve<ITransform>(DerivedTestClass1.T, TestClass2.T)).Returns(transformMock.Object);
            transformMock.Setup(_ => _.Transform(sourceInstance, contextMock.Object, null)).Returns(targetInstance);

            var scaffold = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(BaseTestClass1.T, TestClass2.T),
                Expression.Constant(sourceInstance, typeof(object)),
                Expression.Constant(contextMock.Object));

            scaffold.TargetAsObject = scaffold.GetVariable("targetAsObject", typeof(object));
            scaffold.Expressions.Add(Expression.Assign(scaffold.TargetAsObject, Expression.Constant(null)));

            var target = new FallBackToTransformOnNullTargetTask();

            // Act
            target.Execute(scaffold);

            // Assert
            scaffold.Expressions.Add(Expression.Label(scaffold.GetLabel(null, LegendaryLabels.End)));
            scaffold.Expressions.Add(scaffold.GetVariable("PostMergeAction", typeof(PostMergeAction)));

            var result = Expression
                .Lambda<Func<PostMergeAction>>(Expression.Block(scaffold.Variables, scaffold.Expressions))
                .Compile()();

            repository.VerifyAll();
            Assert.AreEqual(PostMergeAction.Replace, result);
        }
    }
}