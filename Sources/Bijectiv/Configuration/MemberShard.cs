namespace Bijectiv.Configuration
{
    using System;

    public abstract class MemberShard
    {
        public abstract Guid ShardCategory { get; }
    }
}