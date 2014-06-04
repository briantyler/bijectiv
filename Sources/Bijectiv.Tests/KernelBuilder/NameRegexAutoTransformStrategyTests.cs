// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameRegexAutoTransformStrategyTests.cs" company="Bijectiv">
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
//   Defines the NameRegexAutoTransformStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.KernelBuilder
{
    using System.Collections.Generic;
    using System.Reflection;

    using Bijectiv.KernelBuilder;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="NameRegexAutoInjectionStrategy"/> class.
    /// </summary>
    [TestClass]
    public class NameRegexAutoTransformStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new NameRegexAutoInjectionStrategy(null, AutoInjectionOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsWhiteSpace_Throws()
        {
            // Arrange

            // Act
            new NameRegexAutoInjectionStrategy(new string(' ', 5), AutoInjectionOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsInvalidRegex_Throws()
        {
            // Arrange

            // Act
            new NameRegexAutoInjectionStrategy("*", AutoInjectionOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NameRegexAutoInjectionStrategy(
                NameRegexAutoInjectionStrategy.NameTemplateParameter, AutoInjectionOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_PatternTemplateParameter_IsAssignedToPatternTemplateProperty()
        {
            // Arrange

            // Act
            var target = new NameRegexAutoInjectionStrategy("Pattern", AutoInjectionOptions.None);

            // Assert
            Assert.AreEqual("Pattern", target.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_OptionsParameter_IsAssignedToOptionsProperty()
        {
            // Arrange

            // Act
            var target = new NameRegexAutoInjectionStrategy("Pattern", AutoInjectionOptions.MatchTarget);

            // Assert
            Assert.AreEqual(AutoInjectionOptions.MatchTarget, target.Options);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void TryGetSourceForTarget_SourceMembersIsNull_Throws()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Pattern", AutoInjectionOptions.None);
            MemberInfo sourceMember;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.TryGetSourceForTarget(null, Stub.Create<MemberInfo>(), out sourceMember);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void TryGetSourceForTarget_TargetMemberIsNull_Throws()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Pattern", AutoInjectionOptions.None);
            MemberInfo sourceMember;

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            target.TryGetSourceForTarget(Stub.Create<IEnumerable<MemberInfo>>(), null, out sourceMember);

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_NoMatch_ReturnsFalse()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Pattern", AutoInjectionOptions.None);
            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                new MemberInfo[0], CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatch_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Target", AutoInjectionOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatch_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Target", AutoInjectionOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatch_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Target", AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[] { CreateMemberInfo("Ignored") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatch_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("Target", AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[] { CreateMemberInfo("Ignored") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasNoMatchIgnoreCase_ReturnsFalse()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy("tARGET", AutoInjectionOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("target") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatchIgnoreCase_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                "tARGET", AutoInjectionOptions.IgnoreCase | AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[] { CreateMemberInfo("Ignored") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("target"), out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatchIgnoreCase_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                "tARGET", AutoInjectionOptions.IgnoreCase | AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[] { CreateMemberInfo("Ignored") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(sourceMembers, CreateMemberInfo("target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatchMemberSubstitution_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                NameRegexAutoInjectionStrategy.NameTemplateParameter, AutoInjectionOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatchMemberSubstitution_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                NameRegexAutoInjectionStrategy.NameTemplateParameter, AutoInjectionOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatchMemberSubstitution_ReturnsFirstMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                "X" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "Y", AutoInjectionOptions.None);
            var sourceMembers = new[]
            {
                CreateMemberInfo("Target"),
                CreateMemberInfo("xTARGETy"),
                CreateMemberInfo("XTargetY"),
                CreateMemberInfo("Target"),
                CreateMemberInfo("XTargetY")
            };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[2], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatchMemberSubstitution_ReturnsFirstMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                "X" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "Y", AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[]
            {
                CreateMemberInfo("NotMe"),
                CreateMemberInfo("xTARGETy"),
                CreateMemberInfo("XTargetY"),
                CreateMemberInfo("Target"),
                CreateMemberInfo("XTargetY")
            };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("XTargetY"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[3], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchTargetHasMatchMemberSubstitutionIgnoreCase_ReturnsFirstMatch()
        {
            // Arrange
            var target = new NameRegexAutoInjectionStrategy(
                "X" + NameRegexAutoInjectionStrategy.NameTemplateParameter + "Y", 
                AutoInjectionOptions.IgnoreCase | AutoInjectionOptions.MatchTarget);
            var sourceMembers = new[]
            {
                CreateMemberInfo("NotMe"),
                CreateMemberInfo("AndNotMe"),
                CreateMemberInfo("XTARGETY"),
                CreateMemberInfo("target")
            };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(sourceMembers, CreateMemberInfo("xTargety"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[3], sourceMember);
        }

        private static MemberInfo CreateMemberInfo(string name)
        {
            var stub = new Mock<MemberInfo>(MockBehavior.Loose);
            stub.SetupGet(_ => _.Name).Returns(name);
            return stub.Object;
        }
    }
}