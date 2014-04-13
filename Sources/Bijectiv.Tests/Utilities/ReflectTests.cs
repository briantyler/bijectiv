// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectTests.cs" company="Bijectiv">
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
//   Defines the ReflectTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Tests.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="Reflect{T}"/> class.
    /// </summary>
    [TestClass]
    public class ReflectTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_NoParameters_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>.Constructor(() => new MultiConstructorTestClass());

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.IsFalse(ctor.GetParameters().Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_OneParameter_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>
                .Constructor(() => new MultiConstructorTestClass(Placeholder.Is<TestClass2>()));

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.AreEqual(typeof(TestClass2), ctor.GetParameters().Single().ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Constructor_TwoParameters_ReturnsConstructor()
        {
            // Arrange

            // Act
            var ctor = Reflect<MultiConstructorTestClass>
                .Constructor(() => new MultiConstructorTestClass(Placeholder.Is<TestClass1>(), Placeholder.Is<TestClass2>()));

            // Assert
            Assert.IsInstanceOfType(ctor, typeof(ConstructorInfo));
            Assert.AreEqual(typeof(MultiConstructorTestClass), ctor.DeclaringType);
            Assert.AreEqual(2, ctor.GetParameters().Length);
            Assert.AreEqual(typeof(TestClass1), ctor.GetParameters().ElementAt(0).ParameterType);
            Assert.AreEqual(typeof(TestClass2), ctor.GetParameters().ElementAt(1).ParameterType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Method_ActionVariant_ReturnsMethod()
        {
            // Arrange

            // Act
            var method = Reflect<Stack<TestClass1>>.Method(_ => _.Push(Placeholder.Is<TestClass1>()));

            // Assert
            Assert.IsInstanceOfType(method, typeof(MethodInfo));
            Assert.AreEqual("Push", method.Name);
            Assert.AreEqual(typeof(TestClass1), method.GetParameters().Single().ParameterType);
        }
    }
}