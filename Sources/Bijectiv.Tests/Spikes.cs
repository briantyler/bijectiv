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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [Ignore]
    [TestClass]
    public class Spikes
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_Activate_Activated()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder.Register<TestClass1, TestClass2>();
            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(TestClass1.T, TestClass2.T);

            // Act
            var result = transform.Transform(new TestClass1(), CreateContext(kernel));

            // Assert
            Assert.IsInstanceOfType(result, TestClass2.T);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoTransform_AutoTransforms()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
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

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(AutoTransformTestClass1), typeof(AutoTransformTestClass1));

            // Act
            var target = (AutoTransformTestClass1)transform.Transform(source, CreateContext(kernel));

            // Assert
            Assert.AreEqual(33, target.PropertyInt);
            Assert.AreEqual(17, target.FieldInt);
            Assert.AreEqual(@sealed, target.PropertySealed);
            Assert.IsNotNull(target.PropertyBase);
            Assert.IsNotNull(target.FieldBase);
            Assert.AreSame(target.PropertyBase, target.FieldBase);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoTransformCollection_AutoTransforms()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder.Register<AutoTransformTestClass1, AutoTransformTestClass1>().AutoExact();

            var @sealed = new SealedClass1();
            builder
                .Register<SealedClass1, SealedClass1>()
                .NullSourceCustom(ctx => @sealed);

            builder.Register<BaseTestClass1, BaseTestClass1>();
            builder.Register<DerivedTestClass1, BaseTestClass1>();

            var @base1 = new BaseTestClass1();
            var @base2 = new DerivedTestClass1();
            var source1 = new AutoTransformTestClass1
            {
                PropertyInt = 33,
                FieldInt = 17,
                PropertySealed = null,
                FieldBase = @base1,
                PropertyBase = @base2,
            };

            var source2 = new AutoTransformTestClass1
            {
                PropertyInt = 12,
                FieldInt = 82,
                PropertySealed = new SealedClass1(),
                FieldBase = @base2,
                PropertyBase = @base1,
            };

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(IEnumerable<AutoTransformTestClass1>), typeof(AutoTransformTestClass1[]));

            // Act
            var target = (AutoTransformTestClass1[])transform.Transform(new[] { source1, source2 }, CreateContext(kernel));

            // Assert
            Assert.AreEqual(2, target.Length);

            Assert.AreEqual(33, target[0].PropertyInt);
            Assert.AreEqual(17, target[0].FieldInt);
            Assert.AreEqual(@sealed, target[0].PropertySealed);
            Assert.IsNotNull(target[0].FieldBase);
            Assert.IsNotNull(target[0].PropertyBase);

            Assert.AreEqual(12, target[1].PropertyInt);
            Assert.AreEqual(82, target[1].FieldInt);
            Assert.IsNotNull(target[1].PropertySealed);
            Assert.IsNotNull(target[1].FieldBase);
            Assert.IsNotNull(target[1].PropertyBase);

            Assert.AreEqual(target[0].FieldBase, target[1].PropertyBase);
            Assert.AreEqual(target[0].PropertyBase, target[1].FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Spike_StringToDecimalArray_Transforms()
        {
            // Arrange
            var kernel = new InjectionKernelBuilder().Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(IEnumerable), typeof(decimal[]));
            
            // Act
            var result = (decimal[])transform
                .Transform(new object[] { "1", 5, null, "19.753", 8.8 }, CreateContext(kernel));

            // Assert
            new[] { 1, 5, default(decimal), 19.753m, 8.8m }.AssertSequenceEqual(result);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoMerge_AutoMerges()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder.Register<AutoMergeTestClass1, AutoMergeTestClass1>().AutoExact();
            builder.Register<AutoMergeTestClass2, AutoMergeTestClass2>().AutoExact();

            var mergeTarget1 = new AutoMergeTestClass1 { FieldInt = 10, PropertyString = "1t" };
            var mergeTarget2 = new AutoMergeTestClass1 { FieldInt = 20, PropertyString = "2t" };
            var source = new AutoMergeTestClass2
            {
                PropertyInt = 5,
                FieldString = "5s",
                FieldMerge = new AutoMergeTestClass1 { FieldInt = 1, PropertyString = "1s" },
                PropertyMerge = new AutoMergeTestClass1 { FieldInt = 2, PropertyString = "2s" },
            };
            var target = new AutoMergeTestClass2
            {
                PropertyInt = 7,
                FieldString = "7t",
                FieldMerge = mergeTarget1,
                PropertyMerge = mergeTarget2,
            };

            var kernel = builder.Build();

            var merge = kernel.Store.Resolve<IMerge>(typeof(AutoMergeTestClass2), typeof(AutoMergeTestClass2));

            // Act
            var result = merge.Merge(source, target, CreateContext(kernel));

            // Assert
            Assert.AreEqual(PostMergeAction.None, result.Action);
            Assert.AreEqual(target, result.Target);
            Assert.AreEqual(5, target.PropertyInt);
            Assert.AreEqual("5s", target.FieldString);
            Assert.AreSame(mergeTarget1, target.FieldMerge);
            Assert.AreSame(mergeTarget2, target.PropertyMerge);

            Assert.AreEqual(1, mergeTarget1.FieldInt);
            Assert.AreEqual("1s", mergeTarget1.PropertyString);
            Assert.AreEqual(2, mergeTarget2.FieldInt);
            Assert.AreEqual("2s", mergeTarget2.PropertyString);
        }

        private static InjectionContext CreateContext(IInjectionKernel kernel)
        {
            return new InjectionContext(
                CultureInfo.InvariantCulture, 
                type => { throw new Exception(); },
                kernel);
        }
    }
}