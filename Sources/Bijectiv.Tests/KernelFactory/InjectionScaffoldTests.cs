// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionScaffoldTests.cs" company="Bijectiv">
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
//   Defines the InjectionScaffoldTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.KernelFactory
{
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.KernelFactory;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="InjectionScaffold"/> class.
    /// </summary>
    [TestClass]
    public class InjectionScaffoldTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ProtectedDefaultConstructor_InstanceCreated()
        {
            // Arrange

            // Act
            Stub.Create<InjectionScaffold>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InstanceRegistryParameterNull_Throws()
        {
            // Arrange

            // Act
            new InjectionScaffold(
                null,
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DefinitionParameterNull_Throws()
        {
            // Arrange

            // Act
            new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                null,
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceAsObjectParameterNull_Throws()
        {
            // Arrange

            // Act
            new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                null,
                Expression.Parameter(typeof(IInjectionContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_InjectionContextParameterNull_Throws()
        {
            // Arrange

            // Act
            new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                null)
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InstanceRegistryParameter_IsAssignedToInstanceRegistryProperty()
        {
            // Arrange
            var parameter = Stub.Create<IInstanceRegistry>();

            // Act
            var testTarget = new InjectionScaffold(
                parameter,
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)));

            // Assert
            Assert.AreEqual(parameter, testTarget.InstanceRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefinitionParameter_IsAssignedToDefinitionProperty()
        {
            // Arrange
            var parameter = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            var testTarget = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                parameter,
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)));

            // Assert
            Assert.AreEqual(parameter, testTarget.Definition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceAsObjectParameter_IsAssignedToSourceAsObjectProperty()
        {
            // Arrange
            var parameter = Expression.Parameter(TestClass1.T);

            // Act
            var testTarget = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                parameter,
                Expression.Parameter(typeof(IInjectionContext)));

            // Assert
            Assert.AreEqual(parameter, testTarget.SourceAsObject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_InjectionContextParameter_IsAssignedToInjectionContextProperty()
        {
            // Arrange
            var parameter = Expression.Parameter(typeof(IInjectionContext));

            // Act
            var testTarget = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                parameter);

            // Assert
            Assert.AreEqual(parameter, testTarget.InjectionContext);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CandidateFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.CandidateFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ProcessedFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.ProcessedFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_UnprocessedFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.UnprocessedFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_VariablesProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.Variables.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ExpressionsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.Expressions.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceMembersProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.SourceMembers.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetMembersProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.TargetMembers.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ProcessedMembersProperty_IsEmpty()
        {
            // Arrange

            // Act
            var testTarget = CreateTestTarget();

            // Assert
            Assert.IsFalse(testTarget.ProcessedTargetMembers.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void UnprocessedFragmentsProperty_DefaultParameters_FiltersCandidateFragmentsByProcessedFragments()
        {
            // Arrange
            var testTarget = CreateTestTarget();
            var fragment1 = Stub.Create<InjectionFragment>(TestClass1.T, TestClass2.T);
            var fragment2 = Stub.Create<InjectionFragment>(TestClass1.T, TestClass2.T);
            var fragment3 = Stub.Create<InjectionFragment>(TestClass1.T, TestClass2.T);

            testTarget.CandidateFragments.AddRange(new[] { fragment1, fragment2, fragment3 });
            testTarget.ProcessedFragments.AddRange(new[] { fragment1, fragment3 });

            // Act
            var result = testTarget.UnprocessedFragments;

            // Assert
            Assert.AreEqual(fragment2, result.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void UnprocessedTargetMembers_DefaultParameters_FiltersTargetMembersByProcessedTargetMembers()
        {
            // Arrange
            var testTarget = new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, typeof(MemberInfoHierarchy6)),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)));

            var member1 = Reflect<MemberInfoHierarchy1>.Property(_ => _.Id);
            var member2 = Reflect<MemberInfoHierarchy3>.Property(_ => _.Id);
            var member3 = Reflect<MemberInfoHierarchy6>.Property(_ => _.Id);

            testTarget.TargetMembers.AddRange(new[] { member1, member2, member3 });
            testTarget.ProcessedTargetMembers.AddRange(new[] { member1, member3 });

            // Act
            var result = testTarget.UnprocessedTargetMembers;

            // Assert
            Assert.AreEqual(member2, result.Single());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAddLabel_LabelDoesNotExist_GetsLabel()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.GetLabel(null, LegendaryLabels.End);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAddLabel_LabelDoesNotExistWithScope_GetsLabel()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.GetLabel(new object(), LegendaryLabels.End);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetOrAddLabel_LabelExistsWithScope_GetsLabel()
        {
            var testTarget = CreateTestTarget();
            var scope = new object();
            var label = testTarget.GetLabel(scope, LegendaryLabels.End);

            // Act
            var result = testTarget.GetLabel(scope, LegendaryLabels.End);

            // Assert
            Assert.AreEqual(label, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetVariable_NameParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            testTarget.GetVariable(null, TestClass1.T);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void GetVariable_TypeParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            testTarget.GetVariable("bijectiv", null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetVariable_VariableDoesNotExist_VariableCreated()
        {
            // Arrange
            var testTarget = CreateTestTarget();

            // Act
            var result = testTarget.GetVariable("bijectiv", TestClass1.T);

            // Assert
            Assert.AreEqual("bijectiv", result.Name);
            Assert.AreEqual(TestClass1.T, result.Type);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetVariable_VariableExists_VariableReturned()
        {
            // Arrange
            var testTarget = CreateTestTarget();
            var parameter = Expression.Parameter(TestClass1.T, "bijectiv");
            testTarget.Variables.Add(parameter);

            // Act
            var result = testTarget.GetVariable("bijectiv", TestClass1.T);

            // Assert
            Assert.AreEqual(parameter, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void GetVariable_VariableExistsButWithDifferentType_Throws()
        {
            // Arrange
            var testTarget = CreateTestTarget();
            var parameter = Expression.Parameter(BaseTestClass1.T, "bijectiv");
            testTarget.Variables.Add(parameter);

            // Act
            testTarget.GetVariable("bijectiv", DerivedTestClass1.T);

            // Assert
        }

        private static InjectionScaffold CreateTestTarget()
        {
            return new InjectionScaffold(
                Stub.Create<IInstanceRegistry>(),
                new InjectionDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(IInjectionContext)));
        }
    }
}