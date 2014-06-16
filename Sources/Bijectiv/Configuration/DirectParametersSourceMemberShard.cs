namespace Bijectiv.Configuration
{
    using System;

    public class DirectParametersSourceMemberShard
    {
        private readonly object @delegate;

        public DirectParametersSourceMemberShard(object @delegate)
        {
            this.@delegate = @delegate;
        }
    }
}