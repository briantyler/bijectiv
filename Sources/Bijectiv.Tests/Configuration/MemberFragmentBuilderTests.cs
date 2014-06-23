// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberFragmentBuilderTests.cs" company="Bijectiv">
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
//   Defines the MemberFragmentBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Configuration
{
    using System.Reflection;

    using Bijectiv.Configuration;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class tests the <see cref="MemberFragmentBuilder{TSource,TTarget,TMember}"/> class.
    /// </summary>
    [TestClass]
    public class MemberFragmentBuilderTests
    {
        private static readonly MemberInfo Member = Reflect<TestClass2>.FieldOrProperty(_ => _.Id);

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_BuilderParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(null, CreateFragment()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_FragmentParameterIsNull_Throws()
        {
            // Arrange

            // Act
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(), 
                CreateFragment()).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_BuilderParameter_IsAssignedToBuilderProperty()
        {
            // Arrange
            var builder = Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>();

            // Act
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                builder,
                CreateFragment());

            // Assert
            Assert.AreEqual(builder, target.Builder);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_FragmentParameter_IsAssignedToFragmentProperty()
        {
            // Arrange
            var fragment = CreateFragment();

            // Act
            var target = new MemberFragmentBuilder<TestClass1, TestClass2, string>(
                Stub.Create<IInjectionDefinitionBuilder<TestClass1, TestClass2>>(),
                fragment);

            // Assert
            Assert.AreEqual(fragment, target.Fragment);
        }

        private static MemberFragment CreateFragment(MemberInfo member = null)
        {
            return new MemberFragment(TestClass1.T, TestClass2.T, member ?? Member);
        }
    }
}