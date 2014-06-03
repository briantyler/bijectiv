namespace Bijectiv.Stores
{
    using System;

    public interface IInstanceFactory
    {
        Tuple<Type, object> Create(IInstanceRegistry registry); 
    }
}