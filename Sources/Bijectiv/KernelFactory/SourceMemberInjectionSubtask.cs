﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberMergeSubtask.cs" company="Bijectiv">
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
//   Defines the SourceMemberMergeSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    public class SourceMemberInjectionSubtask<TShard> : SingleInstanceShardCategorySubtask<TShard>
        where TShard : SourceMemberShard
    {
        public SourceMemberInjectionSubtask(
            [NotNull] ISourceExpressionFactory<TShard> sourceExpressionFactory,
            [NotNull] IInjectionHelper injectionHelper,
            bool isMerge)
            : this()
        {
            if (sourceExpressionFactory == null)
            {
                throw new ArgumentNullException("sourceExpressionFactory");
            }

            if (injectionHelper == null)
            {
                throw new ArgumentNullException("injectionHelper");
            }

            this.SourceExpressionFactory = sourceExpressionFactory;
            this.InjectionHelper = injectionHelper;
            this.IsMerge = isMerge;
        }

        protected SourceMemberInjectionSubtask()
            : base(LegendaryShards.Source)
        {
        }

        public virtual ISourceExpressionFactory<TShard> SourceExpressionFactory { get; private set; }

        public virtual IInjectionHelper InjectionHelper { get; private set; }

        public bool IsMerge { get; set; }

        /// <summary>
        /// Processes a shard.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <param name="shard">
        /// The shard to process.
        /// </param>
        public override void ProcessShard(
            [NotNull] InjectionScaffold scaffold,
            [NotNull] MemberFragment fragment,
            [NotNull] TShard shard)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            var sourceExpression = this.SourceExpressionFactory.Create(scaffold, fragment, shard);
            if (this.IsMerge)
            {
                this.InjectionHelper.AddMergeExpressionToScaffold(scaffold, fragment.Member, sourceExpression);
            }
            else
            {
                this.InjectionHelper.AddTransformExpressionToScaffold(scaffold, fragment.Member, sourceExpression);
            }
        }

        protected internal override bool CanProcess(TShard shard)
        {
            return shard.Inject;
        }
    }
}