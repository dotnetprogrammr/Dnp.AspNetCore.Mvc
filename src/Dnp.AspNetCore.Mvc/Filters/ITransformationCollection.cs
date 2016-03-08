namespace Dnp.AspNetCore.Mvc
{
    using System;

    public interface ITransformationCollection
    {
        void AddMappingFor<T>(int statusCode) where T : Exception;

        int TransformException(Exception ex, int? defaultStatusCode = null);
    }
}