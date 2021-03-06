﻿// --------------------------------------------------------------------------------------------------------------------
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

namespace Bijectiv.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

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
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionDefinitionBuilder<TestClass1, TestClass2>(null, Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_RegistryParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_DefintionParameter_IsAssignedToDefintionProperty()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Assert
            Assert.AreEqual(definition, testTarget.Definition);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_RegistryParameter_IsAssignedToRegistryProperty()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var registry = Stub.Create<IInstanceRegistry>();

            // Act
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registry);

            // Assert
            Assert.AreEqual(registry, testTarget.Registry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceTypeMismatch_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            new InjectionDefinitionBuilder<BaseTestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetTypeMismatch_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);

            // Act
            new InjectionDefinitionBuilder<TestClass1, BaseTestClass2>(definition, Stub.Create<IInstanceRegistry>()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Activate_DefaultParameters_AddsActivatorFragmentToDefintion()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.Activate();

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(ActivateFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void DefaultFactory_DefaultParameters_AddsDefaultFactoryFragmentToDefintion()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.DefaultFactory();

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(DefaultFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CustomFactory_FactoryParameterNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.CustomFactory(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AddsCustomFactoryFragmentToDefintion()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.CustomFactory(p => default(TestClass2));

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(CustomFactoryFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CustomFactory_ValidParameters_AssignsFactoryToCustomFactoryFragment()
        {
            // Arrange
            Func<CustomFactoryParameters<TestClass1>, TestClass2> factory = p => default(TestClass2);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.CustomFactory(factory);

            // Assert
            Assert.AreEqual(factory, ((CustomFactoryFragment)definition.Single()).Factory);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoNone();

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoNone_DefaultParameters_AssignsNullStrategyToFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoNone();

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NullAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoExact();

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoExact();

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoExact_DefaultParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoExact();

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixSource_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AutoPrefixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixSource("Prefix");

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixSource("Prefix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoPrefixTarget_PrefixParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AutoPrefixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixTarget("Prefix");

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoPrefixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoPrefixTarget("Prefix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.None, strategy.Options);
            Assert.AreEqual(
                "^Prefix" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixSource_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AutoSuffixSource(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixSource("Suffix");

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixSource_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixSource("Suffix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.MatchTarget, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoSuffixTarget_SuffixParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AutoSuffixTarget(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixTarget("Suffix");

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoSuffixTarget_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoSuffixTarget("Suffix");

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.None, strategy.Options);
            Assert.AreEqual(
                "^" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "Suffix$", 
                strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void AutoRegex_RegexParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.AutoRegex(null, AutoInjectionOptions.None);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AddsSourceMemberStrategyFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoRegex("^Foo[abc]Bar$", AutoInjectionOptions.IgnoreCase);

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(AutoInjectionFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_AssignsNameRegexSourceMemberStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoRegex("^Foo[abc]Bar$", AutoInjectionOptions.IgnoreCase);

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            Assert.IsInstanceOfType(fragment.Strategy, typeof(NameRegexAutoInjectionStrategy));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void AutoRegex_ValidParameters_CreatesExpectedStrategyStrategy()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.AutoRegex("^Foo[abc]Bar$", AutoInjectionOptions.IgnoreCase);

            // Assert
            var fragment = (AutoInjectionFragment)definition.Single();
            var strategy = (NameRegexAutoInjectionStrategy)fragment.Strategy;
            Assert.AreEqual(AutoInjectionOptions.IgnoreCase, strategy.Options);
            Assert.AreEqual("^Foo[abc]Bar$", strategy.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void NullSourceThrow_ExceptionFactoryParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.NullSourceThrow(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceThrow_ValidParameters_AddsNullSourceFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceThrow(context => new InvalidOperationException());

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        [InvalidOperationExceptionExpected]
        public void NullSourceThrow_ValidParameters_FragmentFactoryThrows()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceThrow(context => new InvalidOperationException());

            // Assert
            var fragment = (NullSourceFragment)definition.Single();
            ((Func<IInjectionContext, TestClass2>)fragment.Factory)(Stub.Create<IInjectionContext>());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceThrow_ValidParameters_FragmentFactoryReceivesContext()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());
            var actualContext = Stub.Create<IInjectionContext>();
            var call = new IInjectionContext[1];

            // Act
            testTarget.NullSourceThrow(context =>
            {
                call[0] = context;
                return new InvalidOperationException();
            });

            // Assert
            var fragment = (NullSourceFragment)definition.Single();

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
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceDefault();

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceDefault_ValidParameters_FragmentFactoryCreatesDefault()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceDefault();

            // Assert
            var fragment = (NullSourceFragment)definition.Single();
            var result = ((Func<IInjectionContext, TestClass2>)fragment.Factory)(Stub.Create<IInjectionContext>());
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void NullSourceCustom_FactoryParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.NullSourceCustom(null);

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceCustom_ValidParameters_AddsNullSourceFragment()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceCustom(context => new TestClass2());

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(NullSourceFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void NullSourceCustom_ValidParameters_FragmentFactoryCreatesDefault()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.NullSourceCustom(context => (TestClass2)context.Resolve(TestClass2.T));

            // Assert
            var contextMock = new Mock<IInjectionContext>(MockBehavior.Strict);
            var expected = new TestClass2();
            contextMock.Setup(_ => _.Resolve(TestClass2.T)).Returns(expected);

            var fragment = (NullSourceFragment)definition.Single();
            var result = ((Func<IInjectionContext, TestClass2>)fragment.Factory)(contextMock.Object);

            contextMock.VerifyAll();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MergeOnKey_SourceKeySelectorParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.MergeOnKey(null, t => t.Id);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void MergeOnKey_TargetKeySelectorParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.MergeOnKey(s => s.Id, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeOnKey_ValidParameters_RegistersTargetFinder()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            registryMock.Setup(_ => _.Register(typeof(TargetFinderRegistration), It.IsAny<TargetFinderRegistration>()));

            // Act
            testTarget.MergeOnKey(s => s.Id, t => t.Id);

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeOnKey_ValidParameters_TargetFinderRegistrationHasExpectedSourceElementAndTargetElementTypes()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            TargetFinderRegistration registration = null; 
            registryMock
                .Setup(_ => _.Register(It.IsAny<Type>(), It.IsAny<object>()))
                .Callback((Type t, object o) => registration = (TargetFinderRegistration)o);

            // Act
            testTarget.MergeOnKey(s => s.Id, t => t.Id);

            // Assert
            Assert.AreEqual(TestClass1.T, registration.SourceElement);
            Assert.AreEqual(TestClass2.T, registration.TargetElement);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeOnKey_ValidParameters_TargetFinderRegistrationHasExpectedFactory()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            TargetFinderRegistration registration = null;
            registryMock
                .Setup(_ => _.Register(It.IsAny<Type>(), It.IsAny<object>()))
                .Callback((Type t, object o) => registration = (TargetFinderRegistration)o);

            // Act
            testTarget.MergeOnKey(s => s.Id, t => t.Id);

            // Assert
            var finder = registration.TargetFinderFactory();
            Assert.IsInstanceOfType(finder, typeof(IdenticalKeyTargetFinder));

            var keyFinder = (IdenticalKeyTargetFinder)finder;
            Assert.AreEqual("1", keyFinder.SourceKeySelector(new TestClass1 { Id = "1" }));
            Assert.AreEqual("2", keyFinder.TargetKeySelector(new TestClass2 { Id = "2" }));
            Assert.AreEqual(EqualityComparer<object>.Default, keyFinder.Comparer);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void MergeOnKey_ValidParameters_ReturnsSelf()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            // Act
            var result = testTarget.MergeOnKey(s => s.Id, t => t.Id);

            // Assert
            Assert.AreEqual(result, testTarget);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void RegisterTrigger_TriggerParameterIsNull_Throws()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.RegisterTrigger(null, TriggeredBy.InjectionEnded);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterTrigger_ValidParameters_AddsInjectionTriggerFragment()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            Action<IInjectionParameters<TestClass1, TestClass2>> trigger = p => p.Naught(); 
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            // Act
            testTarget.RegisterTrigger(trigger, TriggeredBy.InjectionEnded);

            // Assert
            Assert.AreEqual(1, definition.Count());
            Assert.IsInstanceOfType(definition.Single(), typeof(InjectionTriggerFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterTrigger_ValidParameters_AssignsPropertiesToFragment()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            Action<IInjectionParameters<TestClass1, TestClass2>> trigger = p => p.Naught(); 
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, registryMock.Object);

            // Act
            testTarget.RegisterTrigger(trigger, TriggeredBy.InjectionEnded);

            // Assert
            var fragment = (InjectionTriggerFragment)definition.Single();
            Assert.AreEqual(TestClass1.T, fragment.Source);
            Assert.AreEqual(TestClass2.T, fragment.Target);
            Assert.AreEqual(trigger, fragment.Trigger);
            Assert.AreEqual(TriggeredBy.InjectionEnded, fragment.TriggeredBy);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void OnInjectionEnded_ActionParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(
                definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.OnInjectionEnded(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void OnInjectionEnded_ValidParameters_RegisteredTriggerWrapsAction()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTargetMock = new Mock<InjectionDefinitionBuilder<TestClass1, TestClass2>>(
                MockBehavior.Loose,
                definition,
                Stub.Create<IInstanceRegistry>()) { CallBase = true };

            Action<IInjectionParameters<TestClass1, TestClass2>> trigger = p => p.Naught();

            // Act
            testTargetMock.Object.OnInjectionEnded(trigger);
            
            // Assert
            var fragment = (InjectionTriggerFragment)definition.Single();
            Assert.AreEqual(TestClass1.T, fragment.Source);
            Assert.AreEqual(TestClass2.T, fragment.Target);
            Assert.AreEqual(trigger, fragment.Trigger);
            Assert.AreEqual(TriggeredBy.InjectionEnded, fragment.TriggeredBy);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void OnInjectionEnded_ValidParameters_ReturnsSelf()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(
                definition, Stub.Create<IInstanceRegistry>());

            // Act
            var result = testTarget.OnInjectionEnded(p => p.Naught());

            // Assert
            Assert.AreEqual(result, testTarget);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void OnCollectionItem_ActionParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(
                definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.OnCollectionItem(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void OnCollectionItem_ValidParameters_OnInjectionEndedWrapsIndexedAction()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTargetMock = new Mock<InjectionDefinitionBuilder<TestClass1, TestClass2>>(
                MockBehavior.Loose,
                definition,
                Stub.Create<IInstanceRegistry>()) { CallBase = true };

            Action<IInjectionParameters<TestClass1, TestClass2>> indexedAction = null;
            testTargetMock
                .Setup(_ => _.OnInjectionEnded(It.IsAny<Action<IInjectionParameters<TestClass1, TestClass2>>>()))
                .Callback((Action<IInjectionParameters<TestClass1, TestClass2>> a) => indexedAction = a);

            object index = 0;
            object called = false;

            // Act
            testTargetMock.Object.OnCollectionItem((i, p) =>
                {
                    index = i;
                    called = true;
                });
            indexedAction(
                new InjectionParameters<TestClass1, TestClass2>(
                    null, null, null, null, new EnumerableInjectionHint(5)));

            // Assert
            Assert.AreEqual(5, index);
            Assert.IsTrue((bool)called);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void OnCollectionItem_ValidParameters_IndexedActionNotCalledWhenHintIsNotEnumerableInjection()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTargetMock = new Mock<InjectionDefinitionBuilder<TestClass1, TestClass2>>(
                MockBehavior.Loose,
                definition,
                Stub.Create<IInstanceRegistry>()) { CallBase = true };

            Action<IInjectionParameters<TestClass1, TestClass2>> indexedAction = null;
            testTargetMock
                .Setup(_ => _.OnInjectionEnded(It.IsAny<Action<IInjectionParameters<TestClass1, TestClass2>>>()))
                .Callback((Action<IInjectionParameters<TestClass1, TestClass2>> a) => indexedAction = a);

            object called = false;

            // Act
            testTargetMock.Object.OnCollectionItem((i, p) =>
            {
                called = true;
            });
            indexedAction(new InjectionParameters<TestClass1, TestClass2>());

            // Assert
            Assert.IsFalse((bool)called);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void InjectMember_MemberParameterIsNull_Throws()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(
                definition, Stub.Create<IInstanceRegistry>());

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.InjectMember<string>(null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectMember_ValidParameters_AddsMemberFragmentToDefinition()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.InjectMember(t => t.Id);

            // Assert
            Assert.IsInstanceOfType(definition.Single(), typeof(MemberFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectMember_ValidParameters_AddedMemberFragmentHasExpectedProperties()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            testTarget.InjectMember(t => t.Id);

            // Assert
            var fragment = (MemberFragment)definition.Single();
            Assert.AreEqual(TestClass1.T, fragment.Source);
            Assert.AreEqual(TestClass2.T, fragment.Target);
            Assert.AreEqual(Reflect<TestClass2>.Property(_ => _.Id), fragment.Member);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void InjectMember_ValidParameters_ReturnsExpectedMemberDefinitionBuilder()
        {
            // Arrange
            var definition = new InjectionDefinition(TestClass1.T, TestClass2.T);
            var testTarget = new InjectionDefinitionBuilder<TestClass1, TestClass2>(definition, Stub.Create<IInstanceRegistry>());

            // Act
            dynamic result = testTarget.InjectMember(t => t.Id);

            // Assert
            Assert.AreEqual(testTarget, result.Builder);
            Assert.AreEqual(definition.Single(), result.Fragment);
        }
    }
}