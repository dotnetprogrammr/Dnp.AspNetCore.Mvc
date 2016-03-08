using System;
using System.Collections.Immutable;

namespace Dnp.AspNetCore.Mvc
{
    internal sealed class TransformationCollection : ITransformationCollection
    {
        public TransformationCollection()
        {
            Transformations = ImmutableDictionary<Type, int>.Empty;
        }

        internal ImmutableDictionary<Type, int> Transformations { get; private set; }

        public void AddMappingFor<T>(int statusCode) where T : Exception
        {
            var exceptionType = typeof(T);
            if (Transformations.ContainsKey(exceptionType))
            {
                Transformations = Transformations.SetItem(exceptionType, statusCode);
            }
            Transformations = Transformations.Add(exceptionType, statusCode);
        }

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