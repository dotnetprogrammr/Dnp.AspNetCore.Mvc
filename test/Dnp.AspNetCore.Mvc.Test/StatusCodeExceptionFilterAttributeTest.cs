using System;
using System.Linq;

using Dnp.AspNetCore.Mvc.Filters;

using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Routing;

using Xunit;

namespace Dnp.AspNetCore.Mvc.Test
{
    public class StatusCodeExceptionFilterAttributeTest
    {
        [Fact]
        public void OnException_WhenHandlingAnException_ShouldPopulateTheResult()
        {
            // Arrange
            var transformations = new TransformationCollectionBuilder().Return(400).For<InvalidOperationException>().Transformations;

            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var exceptionContext = new ExceptionContext(actionContext, Enumerable.Empty<IFilterMetadata>().ToList());
            exceptionContext.Exception = new InvalidOperationException();

            var exceptionFilter = new StatusCodeExceptionFilterAttribute(transformations);

            // Act
            exceptionFilter.OnException(exceptionContext);

            // Assert
            Assert.NotNull(exceptionContext.Exception);
            Assert.NotNull(exceptionContext.Result);
            Assert.IsAssignableFrom<HttpStatusCodeResult>(exceptionContext.Result);
            var typedResult = exceptionContext.Result as HttpStatusCodeResult;
            Assert.NotNull(typedResult);
            Assert.Equal(400, typedResult.StatusCode);
        }

        [Fact]
        public void OnException_WhenNotHandlingTheException_ShouldNotPopulateTheResult()
        {
            // Arrange
            var transformations = new TransformationCollectionBuilder().Return(400).For<InvalidOperationException>().Transformations;

            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var exceptionContext = new ExceptionContext(actionContext, Enumerable.Empty<IFilterMetadata>().ToList());
            exceptionContext.Exception = new ArgumentException();

            var exceptionFilter = new StatusCodeExceptionFilterAttribute(transformations);

            // Act
            exceptionFilter.OnException(exceptionContext);

            // Assert
            Assert.NotNull(exceptionContext.Exception);
            Assert.Null(exceptionContext.Result);
        }

        [Fact]
        public void OnException_WhenTheExceptionContextIsNull_TheExceptionShouldNotBeHandled()
        {
            // Arrange
            var transformations = new TransformationCollectionBuilder().Return(400).For<InvalidOperationException>().Transformations;

            var exceptionFilter = new StatusCodeExceptionFilterAttribute(transformations);

            // Act
            exceptionFilter.OnException(null);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnException_WhenTheExceptionOnTheExceptionContextIsNull_TheExceptionShouldNotBeHandled()
        {
            // Arrange
            var transformations = new TransformationCollectionBuilder().Return(400).For<InvalidOperationException>().Transformations;

            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var exceptionContext = new ExceptionContext(actionContext, Enumerable.Empty<IFilterMetadata>().ToList());

            var exceptionFilter = new StatusCodeExceptionFilterAttribute(transformations);

            // Act
            exceptionFilter.OnException(null);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void StatusCodeExceptionFilterAttributeOnConstruction_WhenPassedNullTransformations_ExceptionThrown()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => { new StatusCodeExceptionFilterAttribute(null); });

            Assert.NotNull(exception);
            Assert.Equal("transformations", exception.ParamName);
        }
    }
}
