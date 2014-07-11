// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleInstanceShardCategoryMemberFragmentSubtask.cs" company="Bijectiv">
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
//   Defines the SingleInstanceShardCategoryMemberFragmentSubtask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Linq;

    using Bijectiv.Configuration;
    using Bijectiv.Utilities;

    /// <summary>
    /// A <see cref="MemberFragment"/> subtask that processes a shard of type <typeparamref name="TShard"/> if it is
    /// the first unprocessed shard in some category. All remaining shards in the category are marked as processed.
    /// </summary>
    /// <typeparam name="TShard">
    /// The type of shard to process.
    /// </typeparam>
    public abstract class SingleInstanceShardCategoryMemberFragmentSubtask<TShard> : IInjectionSubtask<MemberFragment> 
        where TShard : MemberShard
    {
        /// <summary>
        /// The shard category.
        /// </summary>
        private readonly Guid category;

        /// <summary>
        /// Initialises a new instance of the <see cref="SingleInstanceShardCategoryMemberFragmentSubtask{TShard}"/> class.
        /// </summary>
        /// <param name="category">
        /// The shard category.
        /// </param>
        protected SingleInstanceShardCategoryMemberFragmentSubtask(Guid category)
        {
            this.category = category;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <param name="fragment">
        /// The fragment for which this is a subtask.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public void Execute(InjectionScaffold scaffold, MemberFragment fragment)
        {
            if (scaffold == null)
            {
                throw new ArgumentNullException("scaffold");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var shards = fragment.Where(candidate => candidate.ShardCategory == this.category).ToArray();
            var shard = shards.FirstOrDefault() as TShard;
            if (shard == null)
            {
                return;
            }

            this.ProcessShard(scaffold, fragment, shard);

            fragment.ProcessedShards.AddRange(shards);
        }

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
        public abstract void ProcessShard(InjectionScaffold scaffold, MemberFragment fragment, TShard shard);
    }
}