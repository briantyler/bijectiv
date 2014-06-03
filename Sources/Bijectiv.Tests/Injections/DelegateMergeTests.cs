﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateMergeTests.cs" company="Bijectiv">
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
//   Defines the DelegateMergeTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using System;

    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="DelegateMerge"/> class.
    /// </summary>
    [TestClass]
    public class DelegateMergeTests
    {
        private static readonly Func<object, object, IInjectionContext, IMergeResult> Delegate =
            (source, target, context) => Placeholder.Of<IMergeResult>();

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateMerge(null, typeof(TestClass2), Delegate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateMerge(typeof(TestClass1), null, Delegate).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DelgateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new DelegateMerge(typeof(TestClass1), typeof(TestClass2), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_SourceParameterIsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(typeof(TestClass1), target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(typeof(TestClass2), target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DelegateParameterIsAssignedToDelegateProperty()
        {
            // Arrange

            // Act
            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(Delegate, target.Delegate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Merge_InjectionContextParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Act
            target.Merge(Stub.Create<object>(), Stub.Create<object>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_InvokesDelegate()
        {
            // Arrange
            var called = new[] { false };
            Func<object, object, IInjectionContext, IMergeResult> @delegate = (s, t, c) =>
            {
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>());

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesSourceToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var sourceInstance = Stub.Create<object>();
            Func<object, object, IInjectionContext, IMergeResult> @delegate = (s, t, c) =>
            {
                Assert.AreEqual(sourceInstance, s);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Merge(sourceInstance, Stub.Create<object>(), Stub.Create<IInjectionContext>());

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesTargetToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var targetInstance = Stub.Create<object>();
            Func<object, object, IInjectionContext, IMergeResult> @delegate = (s, t, c) =>
            {
                Assert.AreEqual(targetInstance, t);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Merge(Stub.Create<object>(), targetInstance, Stub.Create<IInjectionContext>());

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesContextToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var context = Stub.Create<IInjectionContext>();
            Func<object, object, IInjectionContext, IMergeResult> @delegate = (s, t, c) =>
            {
                Assert.AreEqual(context, c);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            target.Merge(Stub.Create<object>(), Stub.Create<object>(), context);

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_ReturnsDelegateResult()
        {
            // Arrange
            var expected = Stub.Create<IMergeResult>();
            Func<object, object, IInjectionContext, IMergeResult> @delegate = (s, t, c) => expected;

            var target = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            var result = target.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>());

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}