namespace Bijectiv.TestUtilities.TestTypes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bijectiv.Utilities;

    public class GenericPlaceholderCollection<T> : Collection<T>, IPlaceholderEnumerable
    {
        public IEnumerator<Placeholder> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}