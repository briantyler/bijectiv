namespace Bijectiv
{
    public interface ITargetFinderStore
    {
        ITargetFinder<TSource, TTarget> Resolve<TSource, TTarget>();
    }
}