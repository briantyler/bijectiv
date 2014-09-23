// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateSourceExpressionFactoryTests.cs" company="Bijectiv">
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
//   Defines the DelegateSourceExpressionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="DelegateSourceExpressionFactory"/> class.
    /// </summary>
    [TestClass]
    public class DelegateSourceExpressionFactoryTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new DelegateSourceExpressionFactory().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_ScaffoldParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(null, Stub.Create<MemberFragment>(), Stub.Create<DelegateSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_FragmentParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(Stub.Create<InjectionScaffold>(), null, Stub.Create<DelegateSourceMemberShard>());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Create_ShardParameterIsNull_Throws()
        {
            // Arrange
            var target = new DelegateSourceExpressionFactory();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.Create(Stub.Create<InjectionScaffold>(), Stub.Create<MemberFragment>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ShardHasStaticDelegate_ReturnsExpressionThatCallsDelegate()
        {
            // Arrange
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);
            
            var parameter = Expression.Parameter(typeof(IInjectionParameters<TestClass1, TestClass2>), "injectionParameters");
            scaffoldMock.SetupGet(_ => _.Variables).Returns(new[] { parameter });

            var fragment = Stub.Create<MemberFragment>();

            var shard = new DelegateSourceMemberShard(
                TestClass1.T,
                TestClass2.T,
                Reflect<TestClass2>.FieldOrProperty(_ => _.Id),
                new Func<InjectionParameters<TestClass1, TestClass2>, string>(p => string.Format("ID:{0}", p.Source.Id)),
                false);

            var target = new DelegateSourceExpressionFactory();

            // Act
            var expression = target.Create(scaffoldMock.Object, fragment, shard);

            // Assert
            var lambda = Expression
                .Lambda<Func<IInjectionParameters<TestClass1, TestClass2>, string>>(expression, parameter)
                .Compile();

            var result = lambda(new InjectionParameters<TestClass1, TestClass2>(
                new TestClass1 { Id = "7" },
                new TestClass2(),
                Stub.Create<IInjectionContext>(),
                null));

            Assert.AreEqual("ID:7", result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Create_ShardHasInstanceDelegate_ReturnsExpressionThatCallsDelegate()
        {
            // Arrange
            var scaffoldMock = new Mock<InjectionScaffold>(MockBehavior.Strict);

            var parameter = Expression.Parameter(typeof(IInjectionParameters<TestClass1, TestClass2>), "injectionParameters");
            scaffoldMock.SetupGet(_ => _.Variables).Returns(new[] { parameter });

            var fragment = Stub.Create<MemberFragment>();
            var helper = new DelegateHelper(8);
            var shard = new DelegateSourceMemberShard(
                TestClass1.T,
                TestClass2.T,
                Reflect<TestClass2>.FieldOrProperty(_ => _.Id),
                new Func<InjectionParameters<TestClass1, TestClass2>, string>(helper.Execute),
                false);

            var target = new DelegateSourceExpressionFactory();

            // Act
            var expression = target.Create(scaffoldMock.Object, fragment, shard);

            // Assert
            var lambda = Expression
                .Lambda<Func<IInjectionParameters<TestClass1, TestClass2>, string>>(expression, parameter)
                .Compile();

            var result = lambda(new InjectionParameters<TestClass1, TestClass2>(
                new TestClass1 { Id = "7" },
                new TestClass2(),
                Stub.Create<IInjectionContext>(),
                null));

            Assert.AreEqual("ID:7,8", result);
        }

        private class DelegateHelper
        {
            private readonly int i;

            public DelegateHelper(int i)
            {
                this.i = i;
            }

            public string Execute(InjectionParameters<TestClass1, TestClass2> parameters)
            {
                return string.Format("ID:{0},{1}", parameters.Source.Id, this.i);
            }
        }
    }
}