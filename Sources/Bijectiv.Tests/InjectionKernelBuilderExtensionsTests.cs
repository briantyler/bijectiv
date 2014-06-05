// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionKernelBuilderExtensionsTests.cs" company="Bijectiv">
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
//   Defines the InjectionKernelBuilderExtensionsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1720
namespace Bijectiv.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bijectiv.KernelBuilder;
    using Bijectiv.Stores;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="InjectionKernelBuilderExtensions"/> class.
    /// </summary>
    [TestClass]
    public class InjectionKernelBuilderExtensionsTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Register_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(InjectionKernelBuilder).Register<TestClass1, TestClass2>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_DefintionIsAddedToRegistry()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock.Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedDefintionIsEmpty()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.IsFalse(defintion.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedDefintionHasCorrectSource()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(TestClass1.T, defintion.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_AddedDefintionHasCorrectTarget()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.Register<TestClass1, TestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(TestClass2.T, defintion.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Register_ValidParameters_ResultIsInjectionDefinitionBuilder()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            var result = (InjectionDefinitionBuilder<TestClass1, TestClass2>)target.Register<TestClass1, TestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(defintion, result.Definition);
            Assert.AreEqual(registryMock.Object, result.Registry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void RegisterInherited_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(InjectionKernelBuilder)
                .RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_DefintionIsAddedToRegistry()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);
            registryMock.Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedDefintionHasCorrectSource()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
               .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(DerivedTestClass1.T, defintion.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedDefintionHasCorrectTarget()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(DerivedTestClass2.T, defintion.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_AddedDefintionHasContainsInheritsFragment()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = defintions.Single().Single();
            Assert.IsInstanceOfType(fragment, typeof(InheritsFragment));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_InheritsFragmentHasCorrectSourceBase()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = (InheritsFragment)defintions.Single().Single();
            Assert.AreEqual(BaseTestClass1.T, fragment.SourceBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_InheritsFragmentHasCorrectTargetBase()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var fragment = (InheritsFragment)defintions.Single().Single();
            Assert.AreEqual(BaseTestClass2.T, fragment.TargetBase);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterInherited_ValidParameters_ResultIsInjectionDefinitionBuilder()
        {
            // Arrange
            var defintions = new List<InjectionDefinition>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Loose);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(InjectionDefinition), It.IsAny<InjectionDefinition>()))
                .Callback((Type t, object o) => defintions.Add((InjectionDefinition)o));

            // Act
            var result = (InjectionDefinitionBuilder<DerivedTestClass1, DerivedTestClass2>)
                target.RegisterInherited<DerivedTestClass1, DerivedTestClass2, BaseTestClass1, BaseTestClass2>();

            // Assert
            var defintion = defintions.Single();
            Assert.AreEqual(defintion, result.Definition);
            Assert.AreEqual(registryMock.Object, result.Registry);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void RegisterEnumerable_ThisParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(InjectionKernelBuilder).RegisterEnumerable<IEnumerable<Placeholder>, Collection<Placeholder>>();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterEnumerable_ValidParameters_EnumerableFactoryRegistrationIsAddedToRegistry()
        {
            // Arrange
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock.Setup(_ => _.Register(
                typeof(EnumerableRegistration), It.IsAny<EnumerableRegistration>()));

            // Act
            target.RegisterEnumerable<IEnumerable<Placeholder>, Collection<Placeholder>>();

            // Assert
            registryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterEnumerable_ValidParameters_RegistrationHasCorrectInterfaceType()
        {
            // Arrange
            var registrations = new List<EnumerableRegistration>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(EnumerableRegistration), It.IsAny<EnumerableRegistration>()))
                .Callback((Type t, object o) => registrations.Add((EnumerableRegistration)o));

            // Act
            target.RegisterEnumerable<IEnumerable<Placeholder>, Collection<Placeholder>>();

            // Assert
            var registration = registrations.Single();
            Assert.AreEqual(typeof(IEnumerable<>), registration.InterfaceType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void RegisterEnumerable_ValidParameters_RegistrationHasCorrectConcreteType()
        {
            // Arrange
            var registrations = new List<EnumerableRegistration>();
            var registryMock = new Mock<IInstanceRegistry>(MockBehavior.Strict);
            var target = new InjectionKernelBuilder(registryMock.Object);

            registryMock
                .Setup(_ => _.Register(typeof(EnumerableRegistration), It.IsAny<EnumerableRegistration>()))
                .Callback((Type t, object o) => registrations.Add((EnumerableRegistration)o));

            // Act
            target.RegisterEnumerable<IEnumerable<Placeholder>, Collection<Placeholder>>();

            // Assert
            var registration = registrations.Single();
            Assert.AreEqual(typeof(Collection<>), registration.ConcreteType);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Build_ThisParameterIsNull_Throws()
        {
            // Arrange
            
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            default(InjectionKernelBuilder).Build();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Build_ValidParameters_BuildsUsingBuildConfigurator()
        {
            // Arrange
            var expected = Stub.Create<IInjectionKernel>();
            var storeFactories = new[] { Stub.Create<IInjectionStoreFactory>() };
            var instanceFactories = new[] { Stub.Create<IInstanceFactory>() };
            
            BuildConfigurator.Instance.StoreFactories.Clear();
            BuildConfigurator.Instance.InstanceFactories.Clear();
            // ReSharper disable ImplicitlyCapturedClosure
            BuildConfigurator.Instance.StoreFactories.Add(() => storeFactories[0]);
            BuildConfigurator.Instance.InstanceFactories.Add(() => instanceFactories[0]);
            //// ReSharper restore ImplicitlyCapturedClosure

            var builderMock = new Mock<InjectionKernelBuilder>(MockBehavior.Strict);
            builderMock
                .Setup(_ => _.Build(
                    It.IsAny<IEnumerable<IInjectionStoreFactory>>(),
                    It.IsAny<IEnumerable<IInstanceFactory>>()))
                .Callback(
                    (IEnumerable<IInjectionStoreFactory> sfx, IEnumerable<IInstanceFactory> ifx) =>
                    {
                        storeFactories.AssertSequenceEqual(sfx);
                        instanceFactories.AssertSequenceEqual(ifx);
                    })
                .Returns(expected);

            // Act
            var result = builderMock.Object.Build();

            // Assert
            builderMock.VerifyAll();
            Assert.AreEqual(expected, result);
        }
    }
}