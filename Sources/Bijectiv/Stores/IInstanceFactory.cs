namespace Bijectiv.Stores
{
    using System;

    using Bijectiv.Builder;

    public interface IInstanceFactory
    {
        Tuple<Type, object> Create(IInstanceRegistry registry); 
    }
}