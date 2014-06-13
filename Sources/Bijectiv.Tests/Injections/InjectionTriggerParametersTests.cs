// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTriggerParametersTests.cs" company="Bijectiv">
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
//   Defines the InjectionTriggerParametersTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Injections
{
    using Bijectiv.Injections;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InjectionTriggerParameters{TSource,TTarget}"/> class.
    /// </summary>
    [TestClass]
    public class InjectionTriggerParametersTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionTriggerParameters<TestClass1, TestClass2>().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_AnyParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionTriggerParameters<TestClass1, TestClass2>(null, null, null, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange
            var sourceInstance = new TestClass1();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(sourceInstance, null, null, null);

            // Assert
            Assert.AreEqual(sourceInstance, target.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceAsObjectProperty()
        {
            // Arrange
            var sourceInstance = new TestClass1();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(sourceInstance, null, null, null);

            // Assert
            Assert.AreEqual(sourceInstance, target.SourceAsObject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange
            var targetInstance = new TestClass2();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(null, targetInstance, null, null);

            // Assert
            Assert.AreEqual(targetInstance, target.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetAsObjectProperty()
        {
            // Arrange
            var targetInstance = new TestClass2();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(null, targetInstance, null, null);

            // Assert
            Assert.AreEqual(targetInstance, target.TargetAsObject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ContextParameter_IsAssignedToContextProperty()
        {
            // Arrange
            var context = Stub.Create<IInjectionContext>();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(null, null, context, null);

            // Assert
            Assert.AreEqual(context, target.Context);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_HintParameter_IsAssignedToHintroperty()
        {
            // Arrange
            var hint = new object();

            // Act
            var target = new InjectionTriggerParameters<TestClass1, TestClass2>(null, null, null, hint);

            // Assert
            Assert.AreEqual(hint, target.Hint);
        }
    }
}