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
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var result = transform.Transform(new TestClass1(), CreateContext(kernel), null);

            // Assert
            Assert.IsInstanceOfType(result, TestClass2.T);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoTransform_AutoTransforms()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder
                .Register<AutoInjectionTestClass1, AutoInjectionTestClass1>()
                .AutoExact();

            var @sealed = new SealedClass1();
            builder
                .Register<SealedClass1, SealedClass1>()
                .NullSourceCustom(ctx => @sealed);

            builder.Register<DerivedTestClass1, BaseTestClass1>();

            var @base = new DerivedTestClass1();
            var source = new AutoInjectionTestClass1
            {
                PropertyInt = 33,
                FieldInt = 17,
                PropertySealed = null,
                FieldBase = @base,
                PropertyBase = @base,
            };

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(AutoInjectionTestClass1), typeof(AutoInjectionTestClass1));

            // Act
            var target = (AutoInjectionTestClass1)transform.Transform(source, CreateContext(kernel), null);

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
            builder.Register<AutoInjectionTestClass1, AutoInjectionTestClass1>().AutoExact();

            var @sealed = new SealedClass1();
            builder
                .Register<SealedClass1, SealedClass1>()
                .NullSourceCustom(ctx => @sealed);

            builder.Register<BaseTestClass1, BaseTestClass1>();
            builder.Register<DerivedTestClass1, BaseTestClass1>();

            var @base1 = new BaseTestClass1();
            var @base2 = new DerivedTestClass1();
            var source1 = new AutoInjectionTestClass1
            {
                PropertyInt = 33,
                FieldInt = 17,
                PropertySealed = null,
                FieldBase = @base1,
                PropertyBase = @base2,
            };

            var source2 = new AutoInjectionTestClass1
            {
                PropertyInt = 12,
                FieldInt = 82,
                PropertySealed = new SealedClass1(),
                FieldBase = @base2,
                PropertyBase = @base1,
            };

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(IEnumerable<AutoInjectionTestClass1>), typeof(AutoInjectionTestClass1[]));

            // Act
            var target = (AutoInjectionTestClass1[])transform.Transform(new[] { source1, source2 }, CreateContext(kernel), null);

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
            var builder = new InjectionKernelBuilder();
            builder
                .Register<object, decimal>()
                .NullSourceDefault();

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(IEnumerable), typeof(decimal[]));
            
            // Act
            var result = (decimal[])transform
                .Transform(new object[] { "1", 5, null, "19.753", 8.8 }, CreateContext(kernel), null);

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
            var result = merge.Merge(source, target, CreateContext(kernel), null);

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

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoMerge_AutoMergesCollectionAgainstKey()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder
                .Register<KeyedClass1, KeyedClass1>()
                .AutoExact()
                .MergeOnKey(s => s.Key, t => t.Key)
                .OnCollectionItem((i, p) => p.Target.Index = i);

            var kernel = builder.Build();
            var merge = kernel.Store.Resolve<IMerge>(typeof(IEnumerable<KeyedClass1>), typeof(IEnumerable<KeyedClass1>));

            var source = new[]
            {
                new KeyedClass1 { Key = 1, Value = "1s" },
                new KeyedClass1 { Key = 2, Value = "2s" },
                new KeyedClass1 { Key = 3, Value = "3s" },
                new KeyedClass1 { Key = 4, Value = "4s" }
            };

            var t1 = new KeyedClass1 { Key = 1, Value = "1t" };
            var t4 = new KeyedClass1 { Key = 4, Value = "4t" };
            var t3 = new KeyedClass1 { Key = 3, Value = "3t" };
            var target = new List<KeyedClass1> 
            {
                t3,
                new KeyedClass1 { Key = 9, Value = "9t" },
                t4,
                new KeyedClass1 { Key = 8, Value = "8t" },
                t1,
            };

            // Act
            merge.Merge(source, target, CreateContext(kernel), null);

            // Assert
            Assert.AreEqual(4, target.Count());
            Assert.AreEqual(t1, target.ElementAt(0));
            Assert.AreEqual(1, target.ElementAt(0).Key);
            Assert.AreEqual("1s", target.ElementAt(0).Value);
            Assert.AreEqual(0, target.ElementAt(0).Index);
            Assert.AreEqual(2, target.ElementAt(1).Key);
            Assert.AreEqual("2s", target.ElementAt(1).Value);
            Assert.AreEqual(1, target.ElementAt(1).Index);
            Assert.AreEqual(t3, target.ElementAt(2));
            Assert.AreEqual(3, target.ElementAt(2).Key);
            Assert.AreEqual("3s", target.ElementAt(2).Value);
            Assert.AreEqual(2, target.ElementAt(2).Index);
            Assert.AreEqual(t4, target.ElementAt(3));
            Assert.AreEqual(4, target.ElementAt(3).Key);
            Assert.AreEqual("4s", target.ElementAt(3).Value);
            Assert.AreEqual(3, target.ElementAt(3).Index);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoMerge_AutoMergesIntoNull()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            builder
                .Register<KeyedClass1, KeyedClass1>()
                .AutoExact()
                .MergeOnKey(s => s.Key, t => t.Key);

            var kernel = builder.Build();
            var merge = kernel.Store.Resolve<IMerge>(typeof(IEnumerable<KeyedClass1>), typeof(IEnumerable<KeyedClass1>));

            var source = new[]
            {
                new KeyedClass1 { Key = 1, Value = "1s" },
                new KeyedClass1 { Key = 2, Value = "2s" },
                new KeyedClass1 { Key = 3, Value = "3s" },
                new KeyedClass1 { Key = 4, Value = "4s" }
            };

            // Act
            var result = merge.Merge(source, null, CreateContext(kernel), null);

            // Assert
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
            var target = ((IEnumerable<KeyedClass1>)result.Target).ToArray();
            Assert.AreEqual(4, target.Count());
            Assert.AreEqual(1, target.ElementAt(0).Key);
            Assert.AreEqual("1s", target.ElementAt(0).Value);
            Assert.AreEqual(2, target.ElementAt(1).Key);
            Assert.AreEqual("2s", target.ElementAt(1).Value);
            Assert.AreEqual(3, target.ElementAt(2).Key);
            Assert.AreEqual("3s", target.ElementAt(2).Value);
            Assert.AreEqual(4, target.ElementAt(3).Key);
            Assert.AreEqual("4s", target.ElementAt(3).Value);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Spike_AutoTransformWithExplicitMemberTransforms_Transforms()
        {
            // Arrange
            var builder = new InjectionKernelBuilder();
            var @sealed = new SealedClass1();

            builder
                .Register<AutoInjectionTestClass1, AutoInjectionTestClass1>()
                .AutoExact()
                .InjectMember(t => t.FieldInt).Ignore()
                .InjectMember(t => t.PropertyInt).Condition(p => p.Source.PropertyInt == 33).InjectValue("123")
                .InjectMember(t => t.PropertySealed).InjectValue("sealed");

            builder
                .Register<string, SealedClass1>()
                .CustomFactory(ctx => @sealed);

            builder.Register<DerivedTestClass1, BaseTestClass1>();

            var @base = new DerivedTestClass1();
            var source = new AutoInjectionTestClass1
            {
                PropertyInt = 33,
                FieldInt = 17,
                PropertySealed = null,
                FieldBase = @base,
                PropertyBase = @base,
            };

            var kernel = builder.Build();

            var transform = kernel.Store.Resolve<ITransform>(typeof(AutoInjectionTestClass1), typeof(AutoInjectionTestClass1));

            // Act
            var target = (AutoInjectionTestClass1)transform.Transform(source, CreateContext(kernel), null);

            // Assert
            Assert.AreEqual(123, target.PropertyInt);
            Assert.AreEqual(default(int), target.FieldInt);
            Assert.AreEqual(@sealed, target.PropertySealed);
            Assert.IsNotNull(target.PropertyBase);
            Assert.IsNotNull(target.FieldBase);
            Assert.AreSame(target.PropertyBase, target.FieldBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MemberInfoCollection_InterfaceSpike()
        {
            var intRegularMember = Reflect<IBaseTestClass3>.Property(_ => _.Id);
            var intVirtualMember = Reflect<IBaseTestClass3>.Property(_ => _.Value);
            var intNewMember = Reflect<IBaseTestClass3>.Property(_ => _.Measure);

            var regularMember = Reflect<DerivedTestClass3>.Property(_ => _.Id);
            var virtualMember = Reflect<DerivedTestClass3>.Property(_ => _.Value);
            var newMember = Reflect<DerivedTestClass3>.Property(_ => _.Measure);

            var target = new EquivalentMemberInfoCollection(DerivedTestClass3.T)
            {
                intRegularMember,
                intVirtualMember,
                intNewMember
            };

            Assert.IsTrue(target.Contains(regularMember), "Regular Member");
            Assert.IsTrue(target.Contains(virtualMember), "Virtual Member");
            Assert.IsFalse(target.Contains(newMember), "New Member");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MemberInfoCollection_BaseSpike()
        {
            var intRegularMember = Reflect<BaseTestClass3>.Property(_ => _.Id);
            var intVirtualMember = Reflect<BaseTestClass3>.Property(_ => _.Value);
            var intNewMember = Reflect<BaseTestClass3>.Property(_ => _.Measure);

            var regularMember = Reflect<DerivedTestClass3>.Property(_ => _.Id);
            var virtualMember = Reflect<DerivedTestClass3>.Property(_ => _.Value);
            var newMember = Reflect<DerivedTestClass3>.Property(_ => _.Measure);

            var target = new EquivalentMemberInfoCollection(DerivedTestClass3.T)
            {
                intRegularMember,
                intVirtualMember,
                intNewMember
            };

            Assert.IsTrue(target.Contains(regularMember), "Regular Member");
            Assert.IsTrue(target.Contains(virtualMember), "Virtual Member");
            Assert.IsFalse(target.Contains(newMember), "New Member");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MemberInfoCollection_Spike()
        {
            var intRegularMember = Reflect<DerivedTestClass3>.Property(_ => _.Id);
            var intVirtualMember = Reflect<DerivedTestClass3>.Property(_ => _.Value);
            var intNewMember = Reflect<DerivedTestClass3>.Property(_ => _.Measure);

            var regularMember = Reflect<DerivedTestClass3>.Property(_ => _.Id);
            var virtualMember = Reflect<DerivedTestClass3>.Property(_ => _.Value);
            var newMember = Reflect<DerivedTestClass3>.Property(_ => _.Measure);

            var target = new EquivalentMemberInfoCollection(DerivedTestClass3.T)
            {
                intRegularMember,
                intVirtualMember,
                intNewMember
            };

            Assert.IsTrue(target.Contains(regularMember), "Regular Member");
            Assert.IsTrue(target.Contains(virtualMember), "Virtual Member");
            Assert.IsTrue(target.Contains(newMember), "New Member");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MemberInfoCollection_Hierarchy_Spike()
        {
            var memberI1 = Reflect<IMemberInfoHierarchy1>.Property(_ => _.Id);
            var memberI2 = Reflect<IMemberInfoHierarchy2>.Property(_ => _.Id);
            var memberI3 = Reflect<IMemberInfoHierarchy3>.Property(_ => _.Id);
            var memberI4 = Reflect<IMemberInfoHierarchy4>.Property(_ => _.Id);
            var memberI5 = Reflect<IMemberInfoHierarchy5>.Property(_ => _.Id);
            var memberI6 = Reflect<IMemberInfoHierarchy6>.Property(_ => _.Id);

            var member1 = Reflect<MemberInfoHierarchy1>.Property(_ => _.Id);
            var member2 = Reflect<MemberInfoHierarchy2>.Property(_ => _.Id);
            var member3 = Reflect<MemberInfoHierarchy3>.Property(_ => _.Id);
            var member4 = Reflect<MemberInfoHierarchy4>.Property(_ => _.Id);
            var member5 = Reflect<MemberInfoHierarchy5>.Property(_ => _.Id);
            var member6 = Reflect<MemberInfoHierarchy6>.Property(_ => _.Id);

            var target = new EquivalentMemberInfoCollection(typeof(MemberInfoHierarchy6)) { memberI1 };

            Assert.IsTrue(target.Contains(member1), "Member 1");
            Assert.IsFalse(target.Contains(member2), "Member 2");
            Assert.IsFalse(target.Contains(member3), "Member 3");
            Assert.IsFalse(target.Contains(member4), "Member 4");
            Assert.IsFalse(target.Contains(member5), "Member 5");
            Assert.IsFalse(target.Contains(member6), "Member 6");

            target.Clear();
            target.Add(memberI2);

            Assert.IsFalse(target.Contains(member1), "Member 1");
            Assert.IsTrue(target.Contains(member2), "Member 2");
            Assert.IsFalse(target.Contains(member3), "Member 3");
            Assert.IsFalse(target.Contains(member4), "Member 4");
            Assert.IsFalse(target.Contains(member5), "Member 5");
            Assert.IsFalse(target.Contains(member6), "Member 6");

            target.Clear();
            target.Add(memberI3);

            Assert.IsFalse(target.Contains(member1), "Member 1");
            Assert.IsFalse(target.Contains(member2), "Member 2");
            Assert.IsTrue(target.Contains(member3), "Member 3");
            Assert.IsTrue(target.Contains(member4), "Member 4");
            Assert.IsFalse(target.Contains(member5), "Member 5");
            Assert.IsFalse(target.Contains(member6), "Member 6");

            target.Clear();
            target.Add(memberI4);

            Assert.IsFalse(target.Contains(member1), "Member 1");
            Assert.IsFalse(target.Contains(member2), "Member 2");
            Assert.IsTrue(target.Contains(member3), "Member 3");
            Assert.IsTrue(target.Contains(member4), "Member 4");
            Assert.IsFalse(target.Contains(member5), "Member 5");
            Assert.IsFalse(target.Contains(member6), "Member 6");

            target.Clear();
            target.Add(memberI5);

            Assert.IsFalse(target.Contains(member1), "Member 1");
            Assert.IsFalse(target.Contains(member2), "Member 2");
            Assert.IsFalse(target.Contains(member3), "Member 3");
            Assert.IsFalse(target.Contains(member4), "Member 4");
            Assert.IsTrue(target.Contains(member5), "Member 5");
            Assert.IsTrue(target.Contains(member6), "Member 6");

            target.Add(memberI6);

            Assert.IsFalse(target.Contains(member1), "Member 1");
            Assert.IsFalse(target.Contains(member2), "Member 2");
            Assert.IsFalse(target.Contains(member3), "Member 3");
            Assert.IsFalse(target.Contains(member4), "Member 4");
            Assert.IsTrue(target.Contains(member5), "Member 5");
            Assert.IsTrue(target.Contains(member6), "Member 6");
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