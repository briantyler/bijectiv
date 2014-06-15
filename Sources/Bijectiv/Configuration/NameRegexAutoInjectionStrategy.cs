// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameRegexAutoInjectionStrategy.cs" company="Bijectiv">
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
//   Defines the NameRegexAutoInjectionStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Bijectiv.Utilities;

    /// <summary>
    /// A <see cref="IAutoInjectionStrategy"/> that substitutes the name of the target member into a regular expression
    /// and outputs the first source member with a name that is a match, or <c>null</c> otherwise.
    /// </summary>
    public class NameRegexAutoInjectionStrategy : IAutoInjectionStrategy
    {
        /// <summary>
        /// The name template parameter.
        /// </summary>
        public static readonly string NameTemplateParameter = "{NAME}";

        /// <summary>
        /// The pattern template into which the name will be substituted.
        /// </summary>
        private readonly string patternTemplate;

        /// <summary>
        /// The auto injection options.
        /// </summary>
        private readonly AutoInjectionOptions options;

        /// <summary>
        /// Initialises a new instance of the <see cref="NameRegexAutoInjectionStrategy"/> class.
        /// </summary>
        /// <param name="patternTemplate">
        /// The pattern template into which the name will be substituted.
        /// </param>
        /// <param name="options">
        /// The auto injection options.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="patternTemplate"/> is null.
        /// </exception>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Text.RegularExpressions.Regex", 
            Justification = "This is the only reliable way to validate a regex.")]
        public NameRegexAutoInjectionStrategy(
            [NotNull] string patternTemplate, AutoInjectionOptions options)
        {
            if (patternTemplate == null)
            {
                throw new ArgumentNullException("patternTemplate");
            }

            if (string.IsNullOrWhiteSpace(patternTemplate))
            {
                throw new ArgumentException("Pattern template cannot be empty.", "patternTemplate");
            }

            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Regex(patternTemplate.Replace(NameTemplateParameter, "b113c714"));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(
                    string.Format("Pattern template '{0}' is not a valid regular expression.", patternTemplate),
                    ex);
            }
            
            this.patternTemplate = patternTemplate;
            this.options = options;
        }

        /// <summary>
        /// Gets the pattern template into which the name will be substituted.
        /// </summary>
        public string PatternTemplate
        {
            get { return this.patternTemplate; }
        }

        /// <summary>
        /// Gets the auto injection options.
        /// </summary>
        public AutoInjectionOptions Options
        {
            get { return this.options; }
        }

        /// <summary>
        /// Tries to gets the source <see cref="MemberInfo"/> that will be identified with 
        /// <paramref name="targetMember"/>.
        /// </summary>
        /// <param name="sourceMembers">
        /// The collection of all source members from which <paramref name="sourceMember"/> can be chosen.
        /// </param>
        /// <param name="targetMember">
        /// The target member with which to identify one of <paramref name="sourceMembers"/>.
        /// </param>
        /// <param name="sourceMember">
        /// The source member with which <paramref name="targetMember"/> is identified. If this parameter is 
        /// <c>null</c> then <paramref name="targetMember"/> is not identified with any source member under this 
        /// strategy.
        /// </param>
        /// <returns>
        /// A value indicating whether the strategy was successful.
        /// If the result is <c>true</c> then <paramref name="targetMember"/> will be identified with
        /// <paramref name="sourceMember"/>; <c>otherwise</c> further strategies will be considered.
        /// </returns>
        public bool TryGetSourceForTarget(
            [NotNull] IEnumerable<MemberInfo> sourceMembers,
            [NotNull] MemberInfo targetMember, 
            out MemberInfo sourceMember)
        {
            if (sourceMembers == null)
            {
                throw new ArgumentNullException("sourceMembers");
            }

            if (targetMember == null)
            {
                throw new ArgumentNullException("targetMember");
            }

            sourceMember = sourceMembers.FirstOrDefault(candidate => this.IsMatch(candidate, targetMember));
            return sourceMember != null;
        }

        /// <summary>
        /// Determines whether <paramref name="sourceMember"/> and <paramref name="targetMember"/> are a match.
        /// </summary>
        /// <param name="sourceMember">
        /// The source member.
        /// </param>
        /// <param name="targetMember">
        /// The target member.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="sourceMember"/> and <paramref name="targetMember"/> 
        /// are a match.
        /// </returns>
        private bool IsMatch(MemberInfo sourceMember, MemberInfo targetMember)
        {
            var pattern = this.PatternTemplate.Replace(
                NameTemplateParameter,
                this.Options.HasFlag(AutoInjectionOptions.MatchTarget) ? sourceMember.Name : targetMember.Name);

            var regexOptions = RegexOptions.CultureInvariant
                | (this.Options.HasFlag(AutoInjectionOptions.IgnoreCase) ? RegexOptions.IgnoreCase : RegexOptions.None);

            return Regex.IsMatch(
                this.Options.HasFlag(AutoInjectionOptions.MatchTarget) ? targetMember.Name : sourceMember.Name,
                pattern,
                regexOptions);
        }
    }
}