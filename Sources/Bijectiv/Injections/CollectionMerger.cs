namespace Bijectiv.Injections
{
    using System.Collections;
    using System.Collections.Generic;

    public class CollectionMerger : ICollectionMerger
    {
        public IMergeResult Merge<TTargetElement>(
            IEnumerable source, ICollection<TTargetElement> target, IInjectionContext context)
        {
            throw new System.NotImplementedException();
        }

        public virtual IMergeResult Merge<TSourceElement, TTargetElement>(
            IEnumerable<TSourceElement> source, ICollection<TTargetElement> target, IInjectionContext context)
        {
            throw new System.NotImplementedException();
        }

        public IMergeResult Merge<TSourceElement, TTargetElement>(
            IEnumerable<TSourceElement> source, IList<TTargetElement> target, IInjectionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}