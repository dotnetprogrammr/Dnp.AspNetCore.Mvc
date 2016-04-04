using System;

namespace Dnp.AspNetCore.Mvc
{
    /// <summary>
    /// Defines an interface for mapping <see cref="Exception"/> instances to HTTP status codes.
    /// </summary>
    public interface ITransformationCollection
    {
        /// <summary>
        /// Defines a mapping from an exception of <typeparamref name="T"/> to <paramref name="statusCode"/>.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="statusCode">The status code.</param>

        void AddMappingFor<T>(int statusCode) where T : Exception;

        /// <summary>
        /// Attemps to find the mapped status code for an exception instance.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="defaultStatusCode">The status code to return if not mapping has been specified.</param>
        /// <returns>
        /// If the exception has been mapped then the status code defined for the mapping; otherwise,
        /// <paramref name="defaultStatusCode"/> if given.
        /// </returns>
        int TransformException(Exception ex, int? defaultStatusCode = null);
    }
}