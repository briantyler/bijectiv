// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourceMemberTransformSubtask.cs" company="Bijectiv">
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
//   A subtask that transforms a source shard into a member.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq.Expressions;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A subtask that transforms a source object provided by a <typeparamref name="TShard"/> into a target instance 
    /// member.
    /// </summary>
    /// <typeparam name="TShard">
    /// The type of shard that provides the source object.
    /// </typeparam>
    public class SourceMemberTransformSubtask<TShard> : SingleInstanceShardCategorySubtask<TShard>
        where TShard : SourceMemberShard
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SourceMemberTransformSubtask{TShard}"/> class.
        /// </summary>
        /// <param name="sourceExpressionFactory">
        /// The source expression factory.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public SourceMemberTransformSubtask(
            [NotNull] ISourceExpressionFactory<TShard> sourceExpressionFactory,
            [NotNull] IInjectionHelper injectionHelper)
            : this()
        {
            if (sourceExpressionFactory == null)
            {
                throw new ArgumentNullException("sourceExpressionFactory");
            }

            this.SourceExpressionFactory = sourceExpressionFactory;
            this.InjectionHelper = injectionHelper;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SourceMemberTransformSubtask{TShard}"/> class.
        /// </summary>
        protected SourceMemberTransformSubtask()
            : base(LegendaryShards.Source)
        {
        }

        /// <summary>
        /// Gets the source expression factory.
        /// </summary>
        public virtual ISourceExpressionFactory<TShard> SourceExpressionFactory { get; private set; }

        public IInjectionHelper InjectionHelper { get; private set; }

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
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
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
            this.InjectionHelper.AddTransformExpressionToScaffold(scaffold, fragment.Member, sourceExpression);
        }

        /// <summary>
        /// Determines whether a shard can be processed.
        /// </summary>
        /// <param name="shard">
        /// The shard to check.
        /// </param>
        /// <returns>
        /// A value indicating whether the shard can be processed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        protected internal override bool CanProcess([NotNull] TShard shard)
        {
            if (shard == null)
            {
                throw new ArgumentNullException("shard");
            }

            return shard.Inject;
        }
    }
}