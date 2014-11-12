// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumInjectionTests.cs" company="Bijectiv">
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
//   Defines the EnumInjectionTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Kernel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bijectiv.Kernel;
    using Bijectiv.TestUtilities;
    using Bijectiv.TestUtilities.TestTypes;
    using Bijectiv.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="EnumInjection"/> class.
    /// </summary>
    [TestClass]
    public class EnumInjectionTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_SourceParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumInjection(null, typeof(TestEnum1)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_TargetParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new EnumInjection(typeof(TestEnum1), null).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_NeitherParameterIsAnEnum_Throws()
        {
            // Arrange

            // Act
            new EnumInjection(typeof(int), typeof(int)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_SourceParameterIsInvalid_Throws()
        {
            // Arrange

            // Act
            new EnumInjection(typeof(object), typeof(TestEnum1)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_TargetParameterIsInvalid_Throws()
        {
            // Arrange

            // Act
            new EnumInjection(typeof(TestEnum1), typeof(object)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_EnumParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumInjection(typeof(TestEnum1), typeof(TestEnum2)).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_NullableEnumParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new EnumInjection(typeof(TestEnum1?), typeof(TestEnum2?)).Naught();

            // Assert
        }
        
        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ConvertibleSourceParameter_InstanceCreated()
        {
            // Arrange

            // Act
            TypeClasses.EnumConvertibleTypes.ForEach(item => new EnumInjection(item, typeof(TestEnum1)));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ConvertibleTargetParameter_InstanceCreated()
        {
            // Arrange

            // Act
            TypeClasses.EnumConvertibleTypes.ForEach(item => new EnumInjection(typeof(TestEnum1), item));

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_SourceParameter_IsAssignedToSourceProperty()
        {
            // Arrange

            // Act
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Assert
            Assert.AreEqual(typeof(TestEnum1), testTarget.Source);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_TargetParameter_IsAssignedToTargetProperty()
        {
            // Arrange

            // Act
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Assert
            Assert.AreEqual(typeof(TestEnum2), testTarget.Target);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_MapIsEmpty()
        {
            // Arrange

            // Act
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Assert
            Assert.IsFalse(testTarget.Map.Any());
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_SourceValueParameterIsNotInstanceOfSourceType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Act
            testTarget.Add(1, TestEnum2.Value1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_SourceValueParameterIsNullForNonNullableSourceType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2?));

            // Act
            testTarget.Add(null, TestEnum2.Value1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_NullableSourceValueParameterIsNotInstanceOfTargetType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(TestEnum2?));

            // Act
            testTarget.Add(1, TestEnum2.Value1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_TargetValueParameterIsNotInstanceOfTargetType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Act
            testTarget.Add(TestEnum1.Value1, 1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_TargetValueParameterIsNullForNonNullableTargetType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(TestEnum2));

            // Act
            testTarget.Add(TestEnum1.Value1, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void Add_NullableTargetValueParameterIsNotInstanceOfTargetType_Throws()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(TestEnum2?));

            // Act
            testTarget.Add(TestEnum1.Value1, 1);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParameters_AddsMap()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Act
            testTarget.Add(TestEnum1.Value2, TestEnum2.Value3);

            // Assert
            new[] { new KeyValuePair<object, object>(TestEnum1.Value2, TestEnum2.Value3) }
                .AssertSetEqual(testTarget.Map);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_ValidParametersNullTarget_AddsMap()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2?));

            // Act
            testTarget.Add(TestEnum1.Value2, null);

            // Assert
            new[] { new KeyValuePair<object, object>(TestEnum1.Value2, null) }
                .AssertSetEqual(testTarget.Map);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Add_DuplicteValue_UpdatesMap()
        {
            // Arrange
            // ReSharper disable once UseObjectOrCollectionInitializer
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Act
            testTarget.Add(TestEnum1.Value2, TestEnum2.Value3);
            testTarget.Add(TestEnum1.Value2, TestEnum2.Value1);

            // Assert
            new[] { new KeyValuePair<object, object>(TestEnum1.Value2, TestEnum2.Value1) }
                .AssertSetEqual(testTarget.Map);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_ContextParameterIsNull_Throws()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum1));

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            testTarget.Transform(TestEnum1.Value1, null, null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceIntIsContainedInMapTargetEnum_ReturnsMapTargetValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(int), typeof(TestEnum2)) { { 321, TestEnum2.Value3 } };

            // Act
            var result = testTarget.Transform(321, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value3, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceEnumTargetEnum_ReturnsTargetValuetWithMatchingName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2));

            // Act
            var result = testTarget.Transform(TestEnum1.Value1, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value1, result);
        }

        //---
        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceIntIsContainedInMapNullableTargetEnum_ReturnsMapTargetValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(int), typeof(TestEnum2?)) { { 321, TestEnum2.Value3 } };

            // Act
            var result = testTarget.Transform(321, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value3, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceEnumNullableTargetEnum_ReturnsTargetValuetWithMatchingName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(TestEnum2?));

            // Act
            var result = testTarget.Transform(TestEnum1.Value1, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value1, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceStringNullableTargetEnum_ReturnsTargetValuetWithMatchingName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(string), typeof(TestEnum2?));

            // Act
            var result = testTarget.Transform("Value1", Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value1, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceEnumTargetString_ReturnsSourceValueName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(string));

            // Act
            var result = testTarget.Transform(TestEnum1.Value1, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual("Value1", result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceEnumTargetNumeric_ReturnsSourceValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1), typeof(int));
            var contextMock = new Mock<IInjectionContext>();
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = testTarget.Transform(TestEnum1.Value2, contextMock.Object, null);

            // Assert
            Assert.AreEqual((int)TestEnum1.Value2, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceStringTargetEnum_ReturnsTargetValueWithMatchingName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(string), typeof(TestEnum1));

            // Act
            var result = testTarget.Transform("Value2", Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum1.Value2, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceNumericTargetEnum_ReturnsTargetValueWithMatchingValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(int), typeof(TestEnum1));

            // Act
            var result = testTarget.Transform((int)TestEnum1.Value2, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum1.Value2, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_NullableSourceIsContainedInMap_ReturnsMapTargetValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(int?), typeof(TestEnum2)) { { null, TestEnum2.Value3 } };

            // Act
            var result = testTarget.Transform(null, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value3, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_NullableSourceEnumTargetEnum_ReturnsTargetValuetWithMatchingName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(TestEnum2));

            // Act
            var result = testTarget.Transform((TestEnum1?)TestEnum1.Value1, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum2.Value1, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_NullableSourceEnumTargetString_ReturnsNullableSourceValueName()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(string));

            // Act
            var result = testTarget.Transform((TestEnum1?)TestEnum1.Value1, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual("Value1", result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_NullableSourceEnumTargetNumeric_ReturnsNullableSourceValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(TestEnum1?), typeof(int));
            var contextMock = new Mock<IInjectionContext>();
            contextMock.SetupGet(_ => _.Culture).Returns(CultureInfo.InvariantCulture);

            // Act
            var result = testTarget.Transform((TestEnum1?)TestEnum1.Value2, contextMock.Object, null);

            // Assert
            Assert.AreEqual((int)TestEnum1.Value2, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_NullableSourceNumericTargetEnum_ReturnsTargetValueWithMatchingValue()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(int?), typeof(TestEnum1));

            // Act
            var result = testTarget.Transform((int?)TestEnum1.Value2, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.AreEqual(TestEnum1.Value2, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Transform_SourceNullStringNullableTargetEnum_ReturnsNull()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(string), typeof(TestEnum1?));

            // Act
            var result = testTarget.Transform(null, Stub.Create<IInjectionContext>(), null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void Transform_NullableSourceNotNullableTargetEnum_Throws()
        {
            // Arrange
            var testTarget = new EnumInjection(typeof(string), typeof(TestEnum1));

            // Act
            testTarget.Transform(null, Stub.Create<IInjectionContext>(), null);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Merge_ValidPrameters_DelegatesToTransform()
        {
            // Arrange
            var testTargetMock = new Mock<EnumInjection>(MockBehavior.Strict, typeof(int), typeof(TestEnum1));
            var context = Stub.Create<IInjectionContext>();
            testTargetMock
                .Setup(_ => _.Transform(123, context, null))
                .Returns(TestEnum1.Value2);

            // Act
            var result = testTargetMock.Object.Merge(123, TestEnum1.Value1, context, null);

            // Assert
            testTargetMock.VerifyAll();
            Assert.AreEqual(TestEnum1.Value2, result.Target);
            Assert.AreEqual(PostMergeAction.Replace, result.Action);
        }
    }
}