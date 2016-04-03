using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace Dnp.AspNetCore.Mvc.Filters
{
    public class StatusCodeExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ITransformationCollection transformations;

        public StatusCodeExceptionFilterAttribute(ITransformationCollection transformations)
        {
            if (transformations == null)
            {
                throw new ArgumentNullException(nameof(transformations));
            }

            this.transformations = transformations;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context?.Exception == null)
            {
                return;
            }

            try
            {
                var statusCode = this.transformations.TransformException(context.Exception);
                context.Result = new HttpStatusCodeResult(statusCode);
            }
            catch (ExceptionNotMappedException)
            {
            }
        }
    }
}
