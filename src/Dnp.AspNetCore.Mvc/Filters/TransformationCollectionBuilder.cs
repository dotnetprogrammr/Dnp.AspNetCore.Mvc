using System;

namespace Dnp.AspNetCore.Mvc
{
    public class ExceptionTransformationCollectionBuilder
    {
        internal ExceptionTransformationCollectionBuilder(ITransformationCollection transformations, int statusCode)
        {
            if (transformations == null)
            {
                throw new ArgumentNullException(nameof(transformations));
            }
            ;
            StatusCode = statusCode;
            Transformations = transformations;
        }

        /// <summary>
        /// Gets the collection of defined transformations.
        /// </summary>
        public ITransformationCollection Transformations { get; }

        internal int StatusCode { get; }

        /// <summary>
        /// Defines a new status code against which exceptions are to be mapped.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>A <see cref="MappedTransformationCollectionBuilder"/> instance.</returns>
        public MappedTransformationCollectionBuilder Return(int statusCode)
        {
            return new MappedTransformationCollectionBuilder(Transformations, statusCode);
        }

        /// <summary>
        /// Maps an exception of <typeparamref name="T"/> to the defined status code.
        /// </summary>
        /// <typeparam name="T">The exception.</typeparam>
        /// <returns>A <see cref="ExceptionTransformationCollectionBuilder"/> instance.</returns>
        public ExceptionTransformationCollectionBuilder Or<T>() where T : Exception
        {
            Transformations.AddMappingFor<T>(StatusCode);
            return new ExceptionTransformationCollectionBuilder(Transformations, StatusCode);
        }
    }

    public class MappedTransformationCollectionBuilder
    {
        internal MappedTransformationCollectionBuilder(ITransformationCollection transformations, int statusCode)
        {
            if (transformations == null)
            {
                throw new ArgumentNullException(nameof(transformations));
            }

            StatusCode = statusCode;
            Transformations = transformations;
        }

        internal int StatusCode { get; }

        internal ITransformationCollection Transformations { get; }

        /// <summary>
        /// Maps an exception of <typeparamref name="T"/> to the defined status code.
        /// </summary>
        /// <typeparam name="T">The exception.</typeparam>
        /// <returns>A <see cref="ExceptionTransformationCollectionBuilder"/> instance.</returns>
        public ExceptionTransformationCollectionBuilder For<T>() where T : Exception
        {
            Transformations.AddMappingFor<T>(StatusCode);
            return new ExceptionTransformationCollectionBuilder(Transformations, StatusCode);
        }
    }

    /// <summary>
    /// Provides a mechanism for defining transformations.
    /// </summary>
    public class TransformationCollectionBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationCollectionBuilder"/> class.
        /// </summary>
        public TransformationCollectionBuilder()
        {
            Transformations = new TransformationCollection();
        }

        internal TransformationCollection Transformations { get; }

        /// <summary>
        /// Defines a new status code against which exceptions are to be mapped.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>A <see cref="MappedTransformationCollectionBuilder"/> instance.</returns>
        public MappedTransformationCollectionBuilder Return(int statusCode)
        {
            return new MappedTransformationCollectionBuilder(Transformations, statusCode);
        }
    }
}
