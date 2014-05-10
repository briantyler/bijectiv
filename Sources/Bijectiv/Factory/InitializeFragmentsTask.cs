// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeFragmentsTask.cs" company="Bijectiv">
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
//   Defines the InitializeFragmentsTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The task that initializes the scaffold fragments.
    /// </summary>
    public class InitializeFragmentsTask : IInjectionTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            scaffold.CandidateFragments.Clear();
            scaffold.ProcessedFragments.Clear();

            scaffold.CandidateFragments.AddRange(scaffold.Definition.Reverse());

            var inheritsFragments = scaffold.CandidateFragments.OfType<InheritsFragment>().ToArray();
            while (inheritsFragments.Any())
            {
                scaffold.ProcessedFragments.AddRange(inheritsFragments);

                var baseDefintion = scaffold.DefinitionRegistry
                    .Reverse()
                    .FirstOrDefault(candidate =>
                        candidate.Source == inheritsFragments[0].SourceBase
                        && candidate.Target == inheritsFragments[0].TargetBase);

                if (baseDefintion == null)
                {
                    break;
                }

                scaffold.CandidateFragments.AddRange(baseDefintion.Where(candidate => candidate.Inherited).Reverse());
                inheritsFragments = baseDefintion.OfType<InheritsFragment>().ToArray();
            }
        }
    }
}