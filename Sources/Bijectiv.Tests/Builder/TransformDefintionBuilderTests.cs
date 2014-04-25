// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformDefintionBuilderTests.cs" company="Bijectiv">
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
//   Defines the TransformDefintionBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable AssignNullToNotNullAttribute
namespace Bijectiv.Tests.Builder
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="TransformDefinitionBuilder{TSource,TTarget}"/> class.
    /// </summary>
    [TestClass]
    public class TransformDefintionBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DefintionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new TransformDefinitionBuilder<TestClass1, TestClass2>(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefintionParameter_IsAssignedToDefintionProperty()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);

            // Act
            var target = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Assert
            Assert.AreEqual(defintion, target.Definition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceTypeMismatch_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);

            // Act
            new TransformDefinitionBuilder<BaseTestClass1, TestClass2>(defintion).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetTypeMismatch_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);

            // Act
            new TransformDefinitionBuilder<TestClass1, BaseTestClass2>(defintion).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Activate_DefaultParameters_AddsActivatorFragmentToDefintion()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.Activate();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(ActivateFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void DefaultFactory_DefaultParameters_AddsDefaultFactoryFragmentToDefintion()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.DefaultFactory();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(DefaultFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CustomFactory_FactoryParameterNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.CustomFactory(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AddsCustomFactoryFragmentToDefintion()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.CustomFactory(p => default(TestClass2));

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(CustomFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AssignsFactoryToCustomFactoryFragment()
        {
            // Arrange
            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = p => default(TestClass2);
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.CustomFactory(factory);

            // Assert
            Assert.AreEqual(factory, ((CustomFactoryFragment)defintion.Single()).Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoNone();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AssignsNullStrategyToFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoNone();

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NullSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoExact();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoExact();

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoExact();

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexSourceMemberStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixSource_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixSource("Prefix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexSourceMemberStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixTarget_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixTarget("Prefix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.None, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexSourceMemberStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixSource_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixSource("Suffix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexSourceMemberStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixTarget_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixTarget("Suffix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexSourceMemberStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoRegex_RegexParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoRegex(null, AutoOptions.None);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoRegex("^Foo[abc]Bar$", AutoOptions.IgnoreCase);

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(SourceMemberStrategyFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoRegex("^Foo[abc]Bar$", AutoOptions.IgnoreCase);

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexSourceMemberStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new TransformDefinition(TestClass1.T, TestClass2.T);
            var builder = new TransformDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            builder.AutoRegex("^Foo[abc]Bar$", AutoOptions.IgnoreCase);

            // Assert
            var fragment = (SourceMemberStrategyFragment)defintion.Single();
            var strategy = (NameRegexSourceMemberStrategy)fragment.Strategy;
            Assert.AreEqual(AutoOptions.IgnoreCase, strategy.Options);
            Assert.AreEqual("^Foo[abc]Bar$", strategy.PatternTemplate);
        }
    }
}