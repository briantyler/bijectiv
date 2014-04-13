namespace Bijectiv.Utilities
{
    public static class Placeholder
    {
        public static T Is<T>()
        {
            return default(T);
        }
    }
}