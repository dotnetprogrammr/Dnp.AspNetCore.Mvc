using System;
using System.Collections.Immutable;

namespace Dnp.AspNetCore.Mvc
{
    /// <summary>
    /// A <see cref="ITransformationCollection"/> implementation for mapping exception instances to HTTP status codes. 
    /// </summary>
    internal sealed class TransformationCollection : ITransformationCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationCollection"/> class.
        /// </summary>
        public TransformationCollection()
        {
            Transformations = ImmutableDictionary<Type, int>.Empty;
        }

        /// <summary>
        /// Gets the <see cref="ImmutableDictionary"/> of mappings.
        /// </summary>
        internal ImmutableDictionary<Type, int> Transformations { get; private set; }

        /// <summary>
        /// Defines a mapping from an exception of <typeparamref name="T"/> to <paramref name="statusCode"/>.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="statusCode">The status code.</param>
        public void AddMappingFor<T>(int statusCode) where T : Exception
        {
            var exceptionType = typeof(T);
            if (Transformations.ContainsKey(exceptionType))
            {
                Transformations = Transformations.SetItem(exceptionType, statusCode);
            }
            Transformations = Transformations.Add(exceptionType, statusCode);
        }

        /// <summary>
        /// Attemps to find the mapped status code for an exception instance.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="defaultStatusCode">The status code to return if not mapping has been specified.</param>
        /// <returns>
        /// If the exception has been mapped then the status code defined for the mapping; otherwise,
        /// <paramref name="defaultStatusCode"/> if given.
        /// </returns>
        public int TransformException(Exception ex, int? defaultStatusCode = null)
        {
            var matchedStatusCode = FindStatusCodeForException(ex);
            if (matchedStatusCode.HasValue)
            {
                return matchedStatusCode.Value;
            }
            if (defaultStatusCode.HasValue)
            {
                return defaultStatusCode.Value;
            }
            throw new ExceptionNotMappedException(ex);
        }

        private int? FindStatusCodeForException(Exception ex)
        {
            var exceptionType = ex.GetType();
            if (!Transformations.ContainsKey(exceptionType))
            {
                return null;
            }
            return Transformations[exceptionType];
        }
    }
}