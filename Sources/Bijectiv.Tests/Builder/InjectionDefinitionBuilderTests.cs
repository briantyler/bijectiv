// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionDefinitionBuilderTests.cs" company="Bijectiv">
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
//   Defines the InjectionDefinitionBuilderTests type.
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

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InjectionDefinitionBuilder{TSource,TTarget}"/> class.
    /// </summary>
    [TestClass]
    public class InjectionDefinitionBuilderTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_DefintionParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new InjectionDefinitionBuilder<TestClass1, TestClass2>(null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefintionParameter_IsAssignedToDefintionProperty()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Assert
            Assert.AreEqual(defintion, target.Definition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceTypeMismatch_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            new InjectionDefinitionBuilder<BaseTestClass1, TestClass2>(defintion).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetTypeMismatch_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            new InjectionDefinitionBuilder<TestClass1, BaseTestClass2>(defintion).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Activate_DefaultParameters_AddsActivatorFragmentToDefintion()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.Activate();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(ActivateFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void DefaultFactory_DefaultParameters_AddsDefaultFactoryFragmentToDefintion()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.DefaultFactory();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(DefaultFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CustomFactory_FactoryParameterNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.CustomFactory(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AddsCustomFactoryFragmentToDefintion()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.CustomFactory(p => default(TestClass2));

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(CustomFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AssignsFactoryToCustomFactoryFragment()
        {
            // Arrange
            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = p => default(TestClass2);
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.CustomFactory(factory);

            // Assert
            Assert.AreEqual(factory, ((CustomFactoryFragment)defintion.Single()).Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoNone();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AssignsNullStrategyToFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoNone();

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NullAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoExact();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoExact();

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoExact();

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixSource_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixSource("Prefix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexAutoTransformStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixTarget_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixTarget("Prefix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.None, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexAutoTransformStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixSource_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixSource("Suffix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixTarget_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixTarget("Suffix");

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoTransformStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoRegex_RegexParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoRegex(null, AutoTransformOptions.None);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoRegex("^Foo[abc]Bar$", AutoTransformOptions.IgnoreCase);

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(AutoTransformFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoRegex("^Foo[abc]Bar$", AutoTransformOptions.IgnoreCase);

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoTransformStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.AutoRegex("^Foo[abc]Bar$", AutoTransformOptions.IgnoreCase);

            // Assert
            var fragment = (AutoTransformFragment)defintion.Single();
            var strategy = (NameRegexAutoTransformStrategy)fragment.Strategy;
            Assert.AreEqual(AutoTransformOptions.IgnoreCase, strategy.Options);
            Assert.AreEqual("^Foo[abc]Bar$", strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void NullSourceThrow_ExceptionFactoryParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceThrow(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceThrow_ValidParameters_AddsNullSourceFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceThrow(context => new InvalidOperationException());

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void NullSourceThrow_ValidParameters_FragmentFactoryThrows()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceThrow(context => new InvalidOperationException());

            // Assert
            var fragment = (NullSourceFragment)defintion.Single();
            ((Func<IInjectionContext, TestClass2>)fragment.Factory)(Stub.Create<IInjectionContext>());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceThrow_ValidParameters_FragmentFactoryReceivesContext()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);
            var actualContext = Stub.Create<IInjectionContext>();
            var call = new IInjectionContext[1];

            // Act
            target.NullSourceThrow(context =>
            {
                call[0] = context;
                return new InvalidOperationException();
            });

            // Assert
            var fragment = (NullSourceFragment)defintion.Single();

            try
            {
                ((Func<IInjectionContext, TestClass2>)fragment.Factory)(actualContext);
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(actualContext, call[0]);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceDefault_ValidParameters_AddsNullSourceFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceDefault();

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceDefault_ValidParameters_FragmentFactoryCreatesDefault()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceDefault();

            // Assert
            var fragment = (NullSourceFragment)defintion.Single();
            var result = ((Func<IInjectionContext, TestClass2>)fragment.Factory)(Stub.Create<IInjectionContext>());
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void NullSourceCustom_FactoryParameterIsNull_Throws()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceCustom(null);

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceCustom_ValidParameters_AddsNullSourceFragment()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceCustom(context => new TestClass2());

            // Assert
            Assert.IsInstanceOfType(defintion.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceCustom_ValidParameters_FragmentFactoryCreatesDefault()
        {
            // Arrange
            var defintion = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var target = new InjectionDefinitionBuilder<TestClass1, TestClass2>(defintion);

            // Act
            target.NullSourceCustom(context => (TestClass2)context.Resolve(TestClass2.T));

            // Assert
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            var expected = new TestClass2();
            contextMock.Setup(_ => _.Resolve(TestClass2.T)).Returns(expected);

            var fragment = (NullSourceFragment)defintion.Single();
            var result = ((Func<IInjectionContext, TestClass2>)fragment.Factory)(contextMock.Object);

            contextMock.VerifyAll();
            Assert.AreEqual(expected, result);
        }
    }
}