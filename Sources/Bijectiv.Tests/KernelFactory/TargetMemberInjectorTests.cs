// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetMemberInjectorTests.cs" company="Bijectiv">
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
//   Defines the TargetMemberInjectorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="TargetMemberInjector"/> class.
    /// </summary>
    [TestClass]
    public class TargetMemberInjectorTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefaultParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TargetMemberInjector().Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_ScaffoldIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddTransformExpressionToScaffold(null, Stub.Create<MemberInfo>(), Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_MemberIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddTransformExpressionToScaffold(Stub.Create<InjectionScaffold>(), null, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddTransformExpressionToScaffold_ExpressionIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddTransformExpressionToScaffold(Stub.Create<InjectionScaffold>(), Stub.Create<MemberInfo>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddTransformExpressionToScaffold_MemberIsReadOnly_Merges()
        {
            // Arrange
            var targetMock = new Mock<TargetMemberTransformInjector> { CallBase = true };
            var member = typeof(HermitClass)
                .GetMember("ReadOnlyId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single();
            var scaffold = Stub.Create<InjectionScaffold>();
            var sourceExpression = Expression.Empty();

            // Act
            targetMock.Object.AddTransformExpressionToScaffold(scaffold, member, sourceExpression);

            // Assert
            targetMock.Verify(_ => _.AddMergeExpressionToScaffold(scaffold, member, sourceExpression));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_ScaffoldIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddMergeExpressionToScaffold(null, Stub.Create<MemberInfo>(), Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_MemberIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddMergeExpressionToScaffold(Stub.Create<InjectionScaffold>(), null, Expression.Empty());

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AddMergeExpressionToScaffold_ExpressionIsNull_Throws()
        {
            // Arrange
            var target = new TargetMemberInjector();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.AddMergeExpressionToScaffold(Stub.Create<InjectionScaffold>(), Stub.Create<MemberInfo>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AddMergeExpressionToScaffold_MemberIsReadOnly_Merges()
        {
            // Arrange
            var targetMock = new Mock<TargetMemberMergeInjector> { CallBase = true };
            var member = typeof(HermitClass)
                .GetMember("WriteOnlyId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single();
            var scaffold = Stub.Create<InjectionScaffold>();
            var sourceExpression = Expression.Empty();

            // Act
            targetMock.Object.AddMergeExpressionToScaffold(scaffold, member, sourceExpression);

            // Assert
            targetMock.Verify(_ => _.AddTransformExpressionToScaffold(scaffold, member, sourceExpression));
        }

        internal class TargetMemberTransformInjector : TargetMemberInjector
        {
            public override void AddMergeExpressionToScaffold(InjectionScaffold scaffold, MemberInfo member, Expression sourceExpression)
            {
            }
        }

        internal class TargetMemberMergeInjector : TargetMemberInjector
        {
            public override void AddTransformExpressionToScaffold(InjectionScaffold scaffold, MemberInfo member, Expression sourceExpression)
            {
            }
        }
    }
}