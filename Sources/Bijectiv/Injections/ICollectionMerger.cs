namespace Bijectiv.Injections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ICollectionMerger
    {
        IMergeResult Merge<TTarget>(
            IEnumerable source, 
            ICollection<TTarget> target, 
            IInjectionContext context);

        IMergeResult Merge<TSource, TTarget>(
            IEnumerable<TSource> source, 
            ICollection<TTarget> target, 
            IInjectionContext context);

        IMergeResult Merge<TSource, TTarget>(
            IEnumerable<TSource> source,
            IList<TTarget> target,
            IInjectionContext context);
    }
}