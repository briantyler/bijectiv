namespace Bijectiv
{
    public interface IInjectionKernel
    {
        IInjectionStore Store { get; }

        IInstanceRegistry Registry { get; }
    }
}