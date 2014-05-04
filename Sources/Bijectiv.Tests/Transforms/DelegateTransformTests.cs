// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateTransformTests.cs" company="Bijectiv">
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
//   Defines the DelegateTransformTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Transforms
{
    using System;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Transforms;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="DelegateTransform"/> class.
    /// </summary>
    [TestClass]
    public class DelegateTransformTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateTransform(
                typeof(TestClass1), 
                typeof(TestClass2),
                (source, context) => Placeholder.Of<TestClass2>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new DelegateTransform(
                null,
                typeof(TestClass2),
                (source, context) => Placeholder.Of<TestClass2>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new DelegateTransform(
                typeof(TestClass1),
                null,
                (source, context) => Placeholder.Of<TestClass2>())
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DelgateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new DelegateTransform(
                typeof(TestClass1),
                typeof(TestClass2),
                null)
                .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourceParameterIsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = new DelegateTransform(
                typeof(TestClass1),
                typeof(TestClass2),
                (source, context) => Placeholder.Of<TestClass2>());

            // Assert
            Assert.AreEqual(typeof(TestClass1), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new DelegateTransform(
                typeof(TestClass1),
                typeof(TestClass2),
                (source, context) => Placeholder.Of<TestClass2>());

            // Assert
            Assert.AreEqual(typeof(TestClass2), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DelegateParameterIsAssignedToDelegateProperty()
        {
            // Arrange
            Func<object, ITransformContext, object> @delegate = (s, c) => Placeholder.Of<TestClass2>();

            // Act
            var target = new DelegateTransform(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Assert
            Assert.AreEqual(@delegate, target.Delegate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_TransformContextParameterIsNull_Throws()
        {
            // Arrange
            Func<object, ITransformContext, object> @delegate = (s, c) => true;

            var target = new DelegateTransform(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Transform(Stub.Create<object>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidParameters_InvokesDelegate()
        {
            // Arrange
            var called = new[] { false };
            Func<object, ITransformContext, object> @delegate = (s, c) => called[0] = true;

            var target = new DelegateTransform(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Transform(Stub.Create<object>(), Stub.Create<ITransformContext>());

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidParameters_PassesSourceToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var source = Stub.Create<object>();
            Func<object, ITransformContext, object> @delegate = (s, c) =>
            {
                Assert.AreEqual(source, s);
                return called[0] = true;
            };

            var target = new DelegateTransform(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Transform(source, Stub.Create<ITransformContext>());

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_ValidParameters_PassesContextToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var context = Stub.Create<ITransformContext>();
            Func<object, ITransformContext, object> @delegate = (s, c) =>
            {
                Assert.AreEqual(context, c);
                return called[0] = true;
            };

            var target = new DelegateTransform(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Transform(Stub.Create<object>(), context);

            // Assert
            Assert.IsTrue(called[0]);
        }
    }
}