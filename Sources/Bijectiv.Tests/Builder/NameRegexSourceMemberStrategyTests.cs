// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameRegexSourceMemberStrategyTests.cs" company="Bijectiv">
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
//   Defines the NameRegexSourceMemberStrategyTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Builder
{
    using System.Collections.Generic;
    using System.Reflection;

    using Bijectiv.Builder;
    using Bijectiv.TestUtilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// This class tests the <see cref="NameRegexSourceMemberStrategy"/> class.
    /// </summary>
    [TestClass]
    public class NameRegexSourceMemberStrategyTests
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsNull_Throws()
        {
            // Arrange

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            new NameRegexSourceMemberStrategy(null, AutoOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsWhiteSpace_Throws()
        {
            // Arrange

            // Act
            new NameRegexSourceMemberStrategy(new string(' ', 5), AutoOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentExceptionExpected]
        public void CreateInstance_PatternTemplateParameterIsInvalidRegex_Throws()
        {
            // Arrange

            // Act
            new NameRegexSourceMemberStrategy("*", AutoOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_ValidParameters_InstanceCreated()
        {
            // Arrange

            // Act
            new NameRegexSourceMemberStrategy(
                NameRegexSourceMemberStrategy.NameTemplateParameter, AutoOptions.None).Naught();

            // Assert
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_PatternTemplateParameter_IsAssignedToPatternTemplateProperty()
        {
            // Arrange

            // Act
            var target = new NameRegexSourceMemberStrategy("Pattern", AutoOptions.None);

            // Assert
            Assert.AreEqual("Pattern", target.PatternTemplate);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CreateInstance_OptionsParameter_IsAssignedToOptionsProperty()
        {
            // Arrange

            // Act
            var target = new NameRegexSourceMemberStrategy("Pattern", AutoOptions.MatchSource);

            // Assert
            Assert.AreEqual(AutoOptions.MatchSource, target.Options);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [ArgumentNullExceptionExpected]
        public void TryGetSourceForTarget_SourceMembersIsNull_Throws()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("Pattern", AutoOptions.None);
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
            var target = new NameRegexSourceMemberStrategy("Pattern", AutoOptions.None);
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
            var target = new NameRegexSourceMemberStrategy("Pattern", AutoOptions.None);
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
            var target = new NameRegexSourceMemberStrategy("Target", AutoOptions.None);
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
        public void TryGetSourceForTarget_HasMatch_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("Target", AutoOptions.None);
            var sourceMembers = new[] { CreateMemberInfo("Ignored") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchSourceHasMatch_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("Target", AutoOptions.MatchSource);
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
        public void TryGetSourceForTarget_MatchSourceHasMatch_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("Target", AutoOptions.MatchSource);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasNoMatchIgnoreCase_ReturnsFalse()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("tARGET", AutoOptions.None);
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
        public void TryGetSourceForTarget_MatchSourceHasMatchIgnoreCase_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("tARGET", AutoOptions.IgnoreCase | AutoOptions.MatchSource);
            var sourceMembers = new[] { CreateMemberInfo("target") };

            MemberInfo sourceMember;

            // Act
            var result = target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchSourceHasMatchIgnoreCase_ReturnsMatch()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy("tARGET", AutoOptions.IgnoreCase | AutoOptions.MatchSource);
            var sourceMembers = new[] { CreateMemberInfo("Target") };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Ignored"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[0], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_HasMatchMemberSubstitution_ReturnsTrue()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy(
                NameRegexSourceMemberStrategy.NameTemplateParameter, AutoOptions.None);
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
            var target = new NameRegexSourceMemberStrategy(
                NameRegexSourceMemberStrategy.NameTemplateParameter, AutoOptions.None);
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
            var target = new NameRegexSourceMemberStrategy(
                "X" + NameRegexSourceMemberStrategy.NameTemplateParameter + "Y", AutoOptions.None);
            var sourceMembers = new[]
            {
                CreateMemberInfo("XTargetY"),
                CreateMemberInfo("Target"),
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
            Assert.AreEqual(sourceMembers[1], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchSourceHasMatchMemberSubstitution_ReturnsFirstMatch()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy(
                "X" + NameRegexSourceMemberStrategy.NameTemplateParameter + "Y", AutoOptions.MatchSource);
            var sourceMembers = new[]
            {
                CreateMemberInfo("Target"),
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
            Assert.AreEqual(sourceMembers[3], sourceMember);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TryGetSourceForTarget_MatchSourceHasMatchMemberSubstitutionIgnoreCase_ReturnsFirstMatch()
        {
            // Arrange
            var target = new NameRegexSourceMemberStrategy(
                "X" + NameRegexSourceMemberStrategy.NameTemplateParameter + "Y", 
                AutoOptions.IgnoreCase | AutoOptions.MatchSource);
            var sourceMembers = new[]
            {
                CreateMemberInfo("Target"),
                CreateMemberInfo("Target"),
                CreateMemberInfo("xTARGETy"),
                CreateMemberInfo("XTargetY")
            };

            MemberInfo sourceMember;

            // Act
            target.TryGetSourceForTarget(
                sourceMembers, CreateMemberInfo("Target"), out sourceMember);

            // Assert
            Assert.AreEqual(sourceMembers[2], sourceMember);
        }

        private static MemberInfo CreateMemberInfo(string name)
        {
            var stub = new Mock<MemberInfo>(MockBehavior.Loose);
            stub.SetupGet(_ => _.Name).Returns(name);
            return stub.Object;
        }
    }
}