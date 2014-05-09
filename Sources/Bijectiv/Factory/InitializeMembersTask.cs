// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeMembersTask.cs" company="Bijectiv">
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
//   Defines the InitializeMembersTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// The task that initializes the scaffold members.
    /// </summary>
    public class InitializeMembersTask : IInjectionTask
    {
        /// <summary>
        /// The reflection gateway.
        /// </summary>
        private readonly IReflectionGateway reflectionGateway;

        /// <summary>
        /// Initialises a new instance of the <see cref="InitializeMembersTask"/> class.
        /// </summary>
        /// <param name="reflectionGateway">
        /// The reflection gateway.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InitializeMembersTask([NotNull] IReflectionGateway reflectionGateway)
        {
            if (reflectionGateway == null)
            {
                throw new ArgumentNullException("reflectionGateway");
            }

            this.reflectionGateway = reflectionGateway;
        }

        /// <summary>
        /// Gets the reflection gateway.
        /// </summary>
        public IReflectionGateway ReflectionGateway
        {
            get { return this.reflectionGateway; }
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="ITransform"/> is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute([NotNull] InjectionScaffold scaffold)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }
            
            scaffold.SourceMembers.Clear();
            scaffold.TargetMembers.Clear();
            scaffold.ProcessedTargetMembers.Clear();

            scaffold.SourceMembers.AddRange(
                this.ReflectionGateway.GetFields(scaffold.Definition.Source, ReflectionOptions.CanRead));
            scaffold.SourceMembers.AddRange(
                this.ReflectionGateway.GetProperties(scaffold.Definition.Source, ReflectionOptions.CanRead));

            scaffold.TargetMembers.AddRange(
                this.ReflectionGateway.GetFields(scaffold.Definition.Target, ReflectionOptions.None));
            scaffold.TargetMembers.AddRange(
                this.ReflectionGateway.GetProperties(scaffold.Definition.Target, ReflectionOptions.None));
        }
    }
}