namespace Bijectiv.KernelFactory
{
    using System.Linq.Expressions;

    using JetBrains.Annotations;

    public interface ILabelCache
    {
        LabelTarget GetLabel([NotNull] object scope, LabelCategory category);
    }
}