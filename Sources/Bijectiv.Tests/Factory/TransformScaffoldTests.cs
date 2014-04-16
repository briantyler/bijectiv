// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformScaffoldTests.cs" company="Bijectiv">
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
//   Defines the TransformScaffoldTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Factory
{
    using System.Linq;
    using System.Linq.Expressions;

    using Bijectiv.Builder;
    using Bijectiv.Factory;
    using Bijectiv.Tests.TestTools;
    using Bijectiv.Tests.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformScaffold"/> class.
    /// </summary>
    [TestClass]
    public class TransformScaffoldTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DefinitionRegistryParameterNull_Throws()
        {
            // Arrange

            // Act
            new TransformScaffold(
                null,
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)))
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
            new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                null,
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)))
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
            new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                null,
                Expression.Parameter(typeof(ITransformContext)))
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TransformContextParameterNull_Throws()
        {
            // Arrange

            // Act
            new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                null)
            .Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefinitionRegistryParameter_IsAssignedToDefinitionRegistryProperty()
        {
            // Arrange
            var parameter = Stub.Create<ITransformDefinitionRegistry>();

            // Act
            var target = new TransformScaffold(
                parameter,
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)));

            // Assert
            Assert.AreEqual(parameter, target.DefinitionRegistry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefinitionParameter_IsAssignedToDefinitionProperty()
        {
            // Arrange
            var parameter = new TransformDefinition(TestClass1.T, TestClass2.T);

            // Act
            var target = new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                parameter,
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)));

            // Assert
            Assert.AreEqual(parameter, target.Definition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceAsObjectParameter_IsAssignedToSourceAsObjectProperty()
        {
            // Arrange
            var parameter = Expression.Parameter(TestClass1.T);

            // Act
            var target = new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                parameter,
                Expression.Parameter(typeof(ITransformContext)));

            // Assert
            Assert.AreEqual(parameter, target.SourceAsObject);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TransformContextParameter_IsAssignedToTransformContextProperty()
        {
            // Arrange
            var parameter = Expression.Parameter(typeof(ITransformContext));

            // Act
            var target = new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                parameter);

            // Assert
            Assert.AreEqual(parameter, target.TransformContext);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_CandidateFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var target = CreateTarget();

            // Assert
            Assert.IsFalse(target.CandidateFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ProcessedFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var target = CreateTarget();

            // Assert
            Assert.IsFalse(target.ProcessedFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_UnprocessedFragmentsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var target = CreateTarget();

            // Assert
            Assert.IsFalse(target.UnprocessedFragments.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_VariablesProperty_IsEmpty()
        {
            // Arrange

            // Act
            var target = CreateTarget();

            // Assert
            Assert.IsFalse(target.Variables.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ExpressionsProperty_IsEmpty()
        {
            // Arrange

            // Act
            var target = CreateTarget();

            // Assert
            Assert.IsFalse(target.Expressions.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void UnprocessedFragmentsProperty_DefaultParameters_FiltersCandidateFragmentsByProcessedFragments()
        {
            // Arrange
            var target = CreateTarget();
            var fragment1 = Stub.Create<TransformFragment>(TestClass1.T, TestClass2.T);
            var fragment2 = Stub.Create<TransformFragment>(TestClass1.T, TestClass2.T);
            var fragment3 = Stub.Create<TransformFragment>(TestClass1.T, TestClass2.T);

            target.CandidateFragments.AddRange(new[] { fragment1, fragment2, fragment3 });
            target.ProcessedFragments.AddRange(new[] { fragment1, fragment3 });

            // Act
            var result = target.UnprocessedFragments;

            // Assert
            Assert.AreEqual(fragment2, result.Single());
        }

        private static TransformScaffold CreateTarget()
        {
            return new TransformScaffold(
                Stub.Create<ITransformDefinitionRegistry>(),
                new TransformDefinition(TestClass1.T, TestClass2.T),
                Expression.Parameter(TestClass1.T),
                Expression.Parameter(typeof(ITransformContext)));
        }
    }
}