namespace Bijectiv.KernelFactory
{
    using System;

    using Bijectiv.Configuration;

    using JetBrains.Annotations;

    public interface IInjectionSubTask<in TFragment> where TFragment : InjectionFragment
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="scaffold">
        /// The scaffold on which the <see cref="IInjection"/> is being built.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        void Execute([NotNull] InjectionScaffold scaffold, [NotNull] TFragment fragment);
    }
}