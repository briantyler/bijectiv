namespace Bijectiv.Tests.Tools
{
    public static class It
    {
        public static TInstance IsAny<TInstance>()
        {
            return default(TInstance);
        }
    }
}