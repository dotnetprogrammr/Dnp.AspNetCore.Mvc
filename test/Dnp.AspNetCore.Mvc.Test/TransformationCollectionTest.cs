using System;
using System.Collections.Generic;

using FakeItEasy;

using Xunit;

namespace Dnp.AspNetCore.Mvc.Test
{
    public class TransformationCollectionTest
    {
        [Fact]
        public void AddMappingFor_WithAnSeenException_UpdatesATransformationInTheCollection()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            transformationCollection.AddMappingFor<ArgumentNullException>(400);

            // Act
            transformationCollection.AddMappingFor<ArgumentNullException>(403);

            // Assert
            Assert.NotNull(transformationCollection.Transformations);
            Assert.Equal(1, transformationCollection.Transformations.Count);
            Assert.True(transformationCollection.Transformations.ContainsKey(typeof(ArgumentNullException)));
            Assert.Equal(403, transformationCollection.Transformations[typeof(ArgumentNullException)]);
        }

        [Fact]
        public void AddMappingFor_WithAnUnseenException_AddsANewTransformationIntoTheCollection()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();

            // Act
            transformationCollection.AddMappingFor<ArgumentNullException>(400);

            // Assert
            Assert.NotNull(transformationCollection.Transformations);
            Assert.Equal(1, transformationCollection.Transformations.Count);
            Assert.True(transformationCollection.Transformations.ContainsKey(typeof(ArgumentNullException)));
            Assert.Equal(400, transformationCollection.Transformations[typeof(ArgumentNullException)]);
        }

        [Fact]
        public void TransformException_WithMappedException_IgnoresTheDefaultStatusCode()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            transformationCollection.AddMappingFor<ArgumentNullException>(400);
            var ex = new ArgumentNullException();

            // Act
            var statusCode = transformationCollection.TransformException(ex, 500);

            // Assert
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public void TransformException_WithMappedException_ReturnsTheStatusCode()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            transformationCollection.AddMappingFor<ArgumentNullException>(400);
            var ex = new ArgumentNullException();

            // Act
            var statusCode = transformationCollection.TransformException(ex);

            // Assert
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public void TransformException_WithMappedExceptionAndNullDefault_ReturnsTheStatusCode()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            transformationCollection.AddMappingFor<ArgumentNullException>(400);
            var ex = new ArgumentNullException();

            // Act
            var statusCode = transformationCollection.TransformException(ex, null);

            // Assert
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public void TransformException_WithUnmappedException_ReturnsTheDefault()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            var ex = new ArgumentNullException();

            // Act
            var statusCode = transformationCollection.TransformException(ex, 500);

            // Assert
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public void TransformException_WithUnmappedException_Throws()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            var ex = new ArgumentNullException();

            // Act & Assert
            var exception = Assert.Throws<ExceptionNotMappedException>(() => transformationCollection.TransformException(ex));
            Assert.Same(ex, exception.Exception);
        }

        [Fact]
        public void TransformException_WithUnmappedExceptionAndNullDefault_Throws()
        {
            // Arrange
            var transformationCollection = new TransformationCollection();
            var ex = new ArgumentNullException();

            // Act & Assert
            var exception = Assert.Throws<ExceptionNotMappedException>(() => transformationCollection.TransformException(ex, null));
            Assert.Same(ex, exception.Exception);
        }

        [Fact]
        public void WhenInitialized_ShouldHaveAnEmptyTransformationCollection()
        {
            // Act
            var transformationCollection = new TransformationCollection();

            // Assert
            Assert.NotNull(transformationCollection.Transformations);
            Assert.Equal(0, transformationCollection.Transformations.Count);
        }
    }
}
