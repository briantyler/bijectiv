namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;

    public interface IEnumerableFactory
    {
        /// <summary>
        /// Registers an enumerable monad interface to a collection monad concrete type.
        /// </summary>
        /// <typeparam name="TInterface">
        /// The <see cref="IEnumerable{T}"/> interface.
        /// </typeparam>
        /// <typeparam name="TConcrete">
        /// The <see cref="ICollection{T}"/> concrete type.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown when either <typeparamref name="TInterface"/> or <typeparamref name="TConcrete"/> is not a monadic
        /// type.
        /// </exception>
        /// <example>
        ///     Use the <see cref="Placeholder"/> type to register a generic type:
        ///     <code>
        ///         factory.Register&lt;ISet&lt;Placeholder&gt;, HashSet&lt;Placeholder&gt;&gt;();
        ///     </code>
        /// </example>
        /// <remarks>
        /// It would be possible to constuct parameters that do not behave as expected, but you are very unlikely to 
        /// do this by accident.
        /// </remarks>
        void Register<TInterface, TConcrete>()
            where TInterface : IEnumerable<Placeholder>
            where TConcrete : ICollection<Placeholder>, TInterface, new();

        /// <summary>
        /// Resolves an instance of type <paramref name="enumerable"/>.
        /// </summary>
        /// <param name="enumerable">
        /// The (enumerable) type to resolve.
        /// </param>
        /// <returns>
        /// An instance of <paramref name="enumerable"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="enumerable"/> is neither a class nor generic.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no registration exists for creating instances of <paramref name="enumerable"/>.
        /// </exception>
        object Resolve(Type enumerable);
    }
}