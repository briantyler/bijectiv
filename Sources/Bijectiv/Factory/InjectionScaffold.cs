// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionScaffold.cs" company="Bijectiv">
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
//   Defines the InjectionScaffold type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Builder;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a <see cref="InjectionDefinition"/> as it is being worked into a <see cref="IInjection"/>.
    /// </summary>
    public class InjectionScaffold
    {
        /// <summary>
        /// The temporary variables that are required by the <see cref="IInjection"/>.
        /// </summary>
        private readonly List<ParameterExpression> variables = new List<ParameterExpression>();

        /// <summary>
        /// The sequence of expressions that when compiled will comprise the <see cref="IInjection"/>.
        /// </summary>
        private readonly List<Expression> expressions = new List<Expression>();

        /// <summary>
        /// The fragments that can contribute to the <see cref="IInjection"/>.
        /// </summary>
        private readonly List<InjectionFragment> candidateFragments = new List<InjectionFragment>();

        /// <summary>
        /// The fragments that have already been processed.
        /// </summary>
        private readonly HashSet<InjectionFragment> processedFragments = new HashSet<InjectionFragment>();

        /// <summary>
        /// The source members.
        /// </summary>
        private readonly List<MemberInfo> sourceMembers = new List<MemberInfo>();

        /// <summary>
        /// The target members.
        /// </summary>
        private readonly List<MemberInfo> targetMembers = new List<MemberInfo>();

        /// <summary>
        /// The target members that have already been processed.
        /// </summary>
        private readonly HashSet<MemberInfo> processedTargetMembers = new HashSet<MemberInfo>();

        /// <summary>
        /// The return label.
        /// </summary>
        private readonly LabelTarget returnLabel = Expression.Label("return");

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionScaffold"/> class.
        /// </summary>
        /// <param name="instanceRegistry">
        /// The instance registry consisting of all known instances.
        /// </param>
        /// <param name="definition">
        /// The definition from which the <see cref="IInjection"/> is being constructed.
        /// </param>
        /// <param name="sourceAsObject">
        /// An expression that provides the source as an <see cref="object"/>.
        /// </param>
        /// <param name="injectionContext">
        /// An expression that provides the <see cref="IInjectionContext"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public InjectionScaffold(
            [NotNull] IInstanceRegistry instanceRegistry,
            [NotNull] InjectionDefinition definition,
            [NotNull] Expression sourceAsObject,
            [NotNull] Expression injectionContext)
        {
            if (instanceRegistry == null)
            {
                throw new ArgumentNullException("instanceRegistry");
            }

            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            if (sourceAsObject == null)
            {
                throw new ArgumentNullException("sourceAsObject");
            }

            if (injectionContext == null)
            {
                throw new ArgumentNullException("injectionContext");
            }

            this.InstanceRegistry = instanceRegistry;
            this.Definition = definition;
            this.InjectionContext = injectionContext;
            this.SourceAsObject = sourceAsObject;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InjectionScaffold"/> class.
        /// </summary>
        protected InjectionScaffold()
        {
        }

        /// <summary>
        /// Gets the instance registry consisting of all known instances.
        /// </summary>
        public virtual IInstanceRegistry InstanceRegistry { get; private set; }

        /// <summary>
        /// Gets the definition from which the <see cref="IInjection"/> is being created.
        /// </summary>
        public virtual InjectionDefinition Definition { get; private set; }

        /// <summary>
        /// Gets an expression that provides the source as an <see cref="object"/>.
        /// </summary>
        public virtual Expression SourceAsObject { get; private set; }

        /// <summary>
        /// Gets an expression that provides the <see cref="IInjectionContext"/>.
        /// </summary>
        public virtual Expression InjectionContext { get; private set; }

        /// <summary>
        /// Gets or sets an expression that provides the source as its mapped type.
        /// </summary>
        public virtual Expression Source { get; set; }

        /// <summary>
        /// Gets or sets an expression that provides the target as an <see cref="object"/>.
        /// </summary>
        public virtual Expression TargetAsObject { get; set; }

        /// <summary>
        /// Gets or sets an expression that provides the target as its mapped type.
        /// </summary>
        public virtual Expression Target { get; set; }

        /// <summary>
        /// Gets the return label.
        /// </summary>
        public virtual LabelTarget ReturnLabel
        {
            get { return this.returnLabel; }
        }

        /// <summary>
        /// Gets the fragments that have not been processed.
        /// </summary>
        public virtual IEnumerable<InjectionFragment> UnprocessedFragments
        {
            get
            {
                return this.CandidateFragments
                    .Where(candidate => !this.ProcessedFragments.Contains(candidate))
                    .ToArray();
            }
        }

        /// <summary>
        /// Gets the fragments that can contribute to the <see cref="IInjection"/>.
        /// </summary>
        public virtual IList<InjectionFragment> CandidateFragments 
        { 
            get { return this.candidateFragments; }
        }

        /// <summary>
        /// Gets the fragments that have already been processed.
        /// </summary>
        public virtual ISet<InjectionFragment> ProcessedFragments
        {
            get { return this.processedFragments; }
        }

        /// <summary>
        /// Gets the source members.
        /// </summary>
        public virtual IList<MemberInfo> SourceMembers
        {
            get { return this.sourceMembers; }
        }

        /// <summary>
        /// Gets the target members.
        /// </summary>
        public virtual IList<MemberInfo> TargetMembers
        {
            get { return this.targetMembers; }
        }

        /// <summary>
        /// Gets the target members that have already been processed.
        /// </summary>
        public virtual ISet<MemberInfo> ProcessedTargetMembers
        {
            get { return this.processedTargetMembers; }
        }

        /// <summary>
        /// Gets the target members that have not been processed.
        /// </summary>
        public virtual IEnumerable<MemberInfo> UnprocessedTargetMembers
        {
            get
            {
                return this.TargetMembers
                    .Where(candidate => !this.ProcessedTargetMembers.Contains(candidate))
                    .ToArray();
            }
        }

        /// <summary>
        /// Gets the temporary variables that are required by the <see cref="IInjection"/>.
        /// </summary>
        public virtual IList<ParameterExpression> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Gets the sequence of expressions that when compiled will comprise the <see cref="IInjection"/>.
        /// </summary>
        public virtual IList<Expression> Expressions
        {
            get { return this.expressions; }
        }
    }
}