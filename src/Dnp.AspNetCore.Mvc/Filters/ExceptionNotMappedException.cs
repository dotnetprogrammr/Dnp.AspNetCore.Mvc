using System;

namespace Dnp.AspNetCore.Mvc
{
    /// <summary>
    /// The exception that is thrown when the transformation of an exception has been requested but it has not been
    /// mapped.
    /// </summary>
    public class ExceptionNotMappedException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ExceptionNotMappedException"/> class.
        /// </summary>
        /// <param name="exception">The exception that could not be mapped.</param>
        public ExceptionNotMappedException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Exception = exception;
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that was not mapped.
        /// </summary>
        public Exception Exception { get; }
    }
}
