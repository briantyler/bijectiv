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
    using System.Globalization;

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
            var builder = new InjectionStoreBuilder();
            builder.Register<TestClass1, TestClass2>();
            var store = builder.Build();

            var transform = store.Resolve<ITransform>(TestClass1.T, TestClass2.T);

            // Act
            var result = transform.Transform(new TestClass1(), CreateContext(store));

            // Assert
            Assert.IsInstanceOfType(result, TestClass2.T);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoTransform_AutoTransforms()
        {
            // Arrange
            var builder = new InjectionStoreBuilder();
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

            var transform = store.Resolve<ITransform>(typeof(AutoTransformTestClass1), typeof(AutoTransformTestClass1));

            // Act
            var target = (AutoTransformTestClass1)transform.Transform(source, CreateContext(store));

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
        public void Spike_AutoMerge_AutoMerges()
        {
            // Arrange
            var builder = new InjectionStoreBuilder();
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

            var store = builder.Build();

            var merge = store.Resolve<IMerge>(typeof(AutoMergeTestClass2), typeof(AutoMergeTestClass2));

            // Act
            var result = merge.Merge(source, target, CreateContext(store));

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

        private static InjectionContext CreateContext(IInjectionStore store)
        {
            return new InjectionContext(CultureInfo.InvariantCulture, type => { throw new Exception(); }, store);
        }
    }
}