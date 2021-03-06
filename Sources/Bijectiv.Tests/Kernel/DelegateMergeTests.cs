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

namespace Bijectiv.Tests.Kernel
{
    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="DelegateMerge"/> class.
    /// </summary>
    [TestClass]
    public class DelegateMergeTests
    {
        private static readonly DMerge Delegate =
            (source, target, context, hint, injection) => Stub.Create<IMergeResult>();

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
            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(typeof(TestClass1), testTarget.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_TargetParameterIsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(typeof(TestClass2), testTarget.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_DelegateParameterIsAssignedToDelegateProperty()
        {
            // Arrange

            // Act
            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Assert
            Assert.AreEqual(Delegate, testTarget.Delegate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Merge_InjectionContextParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), Delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), null, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_InvokesDelegate()
        {
            // Arrange
            var called = new[] { false };
            DMerge @delegate = (s, t, c, h, i) =>
            {
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesSourceToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var source = Stub.Create<object>();
            DMerge @delegate = (s, t, c, h, i) =>
            {
                Assert.AreEqual(source, s);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(source, Stub.Create<object>(), Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesTargetToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var target = Stub.Create<object>();
            DMerge @delegate = (s, t, c, h, i) =>
            {
                Assert.AreEqual(target, t);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), target, Stub.Create<IInjectionContext>(), null);

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
            DMerge @delegate = (s, t, c, h, i) =>
            {
                Assert.AreEqual(context, c);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), context, null);

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesHintToDelegate()
        {
            // Arrange
            var called = new[] { false };
            var hint = new object();
            DMerge @delegate = (s, t, c, h, i) =>
            {
                Assert.AreEqual(hint, h);
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>(), hint);

            // Assert
            Assert.IsTrue(called[0]);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_PassesSelfToDelegate()
        {
            // Arrange
            var called = new[] { false };
            IInjection injection = null;
            DMerge @delegate = (s, t, c, h, i) =>
            {
                injection = i;
                called[0] = true;
                return Stub.Create<IMergeResult>();
            };

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.IsTrue(called[0]);
            Assert.AreEqual(injection, testTarget);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidParameters_ReturnsDelegateResult()
        {
            // Arrange
            var expected = Stub.Create<IMergeResult>();
            DMerge @delegate = (s, t, c, h, i) => expected;

            var testTarget = new DelegateMerge(typeof(TestClass1), typeof(TestClass2), @delegate);

            // Act
            var result = testTarget.Merge(Stub.Create<object>(), Stub.Create<object>(), Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}