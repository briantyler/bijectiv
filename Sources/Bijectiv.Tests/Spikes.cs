// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Spikes.cs" company="Bijectiv">
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
//   Defines the Spikes type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests
{
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [Ignore]
    [TestClass]
    public class Spikes
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void Spike_Activate_Activated()
        {
            // Arrange
            var builder = new TransformStoreBuilder();
            builder.Register<TestClass1, TestClass2>();
            var store = builder.Build();
            var transform = store.Resolve(TestClass1.T, TestClass2.T);

            // Act
            // var result = transform.Transform(new TestClass1(), new TransformContext(store));

            // Assert
            // Assert.IsInstanceOfType(result, TestClass2.T);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Spike_AutoTransform_AutoTransforms()
        {
            // Arrange
            var builder = new TransformStoreBuilder();
            builder.Register<AutoTransformTestClass1, AutoTransformTestClass1>().AutoExact();

            var @sealed = new SealedClass1();
            builder
                .Register<SealedClass1, SealedClass1>()
                .NullSourceCustom(ctx => @sealed);

            builder.Register<DerivedTestClass1, BaseTestClass1>();

            var @base = new DerivedTestClass1();
            var source = new AutoTransformTestClass1
            {
                PropertyInt = 33,
                FieldInt = 17,
                PropertySealed = null,
                FieldBase = @base,
                PropertyBase = @base,
            };

            var store = builder.Build();
            var transform = store.Resolve(typeof(AutoTransformTestClass1), typeof(AutoTransformTestClass1));

            // Act
            // var target = (AutoTransformTestClass1)transform.Transform(source, new TransformContext(store));

            // Assert
            // Assert.AreEqual(33, target.PropertyInt);
            // Assert.AreEqual(17, target.FieldInt);
            // Assert.AreEqual(@sealed, target.PropertySealed);
            // Assert.IsNotNull(target.PropertyBase);
            // Assert.IsNotNull(target.FieldBase);
            // Assert.AreSame(target.PropertyBase, target.FieldBase);
        }
    }
}