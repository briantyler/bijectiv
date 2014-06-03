namespace Bijectiv
{
    using Bijectiv.Builder;

    public interface IInjectionKernel
    {
        IInjectionStore Store { get; }

        IInstanceRegistry Registry { get; }
    }
}