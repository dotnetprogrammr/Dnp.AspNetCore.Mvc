using System;
using System.Collections.Generic;

using FakeItEasy;

using Xunit;

namespace Dnp.AspNetCore.Mvc.Test
{
    public class ExceptionTransformationCollectionBuilderTest
    {
        [Fact]
        public void Map_WhenInvoked_ShouldReturnAnInstanceWithTheSameStatusCode()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.Return(404);

            // Assert
            Assert.NotNull(newBuilder);
            Assert.Equal(404, newBuilder.StatusCode);
        }

        [Fact]
        public void Map_WhenInvoked_ShouldReturnAnInstanceWithTheSameITransformationCollection()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.Return(404);

            // Assert
            Assert.NotNull(newBuilder);
            Assert.Equal(fakeITransformCollection, newBuilder.Transformations);
        }

        [Fact]
        public void Or_WhenInvokedShouldReturnAnInstanceWithTheSameStatusCode()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.Or<Exception>();

            // Assert
            Assert.NotNull(newBuilder);
            Assert.Same(fakeITransformCollection, newBuilder.Transformations);
        }

        [Fact]
        public void Or_WhenInvoked_ShouldAddANewTransformationIntoTheCollection()
        {
            // Arrange
            var transformations = new Dictionary<Type, int>();

            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            A.CallTo(() => fakeITransformCollection.AddMappingFor<Exception>(A<int>._)).Invokes((int sc) =>
            {
                transformations.Add(typeof(Exception), sc);
            });

            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.Or<Exception>();

            // Assert
            A.CallTo(fakeITransformCollection).Where(call => call.Method.Name == "AddMappingFor").MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(newBuilder);
            Assert.Equal(1, transformations.Count);
            Assert.True(transformations.ContainsKey(typeof(Exception)));
            Assert.Equal(500, transformations[typeof(Exception)]);
        }

        [Fact]
        public void WhenInitialized_ShouldUseTheITransformationCollectionInstanceGiven()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();

            // Act
            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Assert
            Assert.Equal(fakeITransformCollection, builder.Transformations);
        }

        [Fact]
        public void WhenInitialized_ShouldNotModifyTheStatusCode()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();

            // Act
            var builder = new ExceptionTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Assert
            Assert.Equal(500, builder.StatusCode);
        }

        [Fact]
        public void WhenInitialized_WithANullITransformCollection_ExceptionThrown()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ExceptionTransformationCollectionBuilder(null, 500));
            Assert.Equal("transformations", exception.ParamName);
        }
    }

    public class MappedTransformationCollectionBuilderTest
    {
        [Fact]
        public void To_WhenInvoked_ShouldAddANewTransformationIntoTheCollection()
        {
            // Arrange
            var transformations = new Dictionary<Type, int>();

            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            A.CallTo(() => fakeITransformCollection.AddMappingFor<Exception>(A<int>._)).Invokes((int sc) =>
            {
                transformations.Add(typeof(Exception), sc);
            });

            var builder = new MappedTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.For<Exception>();

            // Assert
            A.CallTo(fakeITransformCollection).Where(call => call.Method.Name == "AddMappingFor").MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(newBuilder);
            Assert.Equal(1, transformations.Count);
            Assert.True(transformations.ContainsKey(typeof(Exception)));
            Assert.Equal(500, transformations[typeof(Exception)]);
        }

        [Fact]
        public void To_WhenInvoked_ShouldReturnAnInstanceWithTheSameStatusCode()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();
            var builder = new MappedTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Act
            var newBuilder = builder.For<Exception>();

            // Assert
            Assert.NotNull(newBuilder);
            Assert.Equal(500, newBuilder.StatusCode);
        }

        [Fact]
        public void WhenInitialized_ShouldNotModifyTheStatusCode()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();

            // Act
            var builder = new MappedTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Assert
            Assert.Equal(500, builder.StatusCode);
        }

        [Fact]
        public void WhenInitialized_ShouldUseTheITransformationCollectionInstanceGiven()
        {
            // Arrange
            var fakeITransformCollection = A.Fake<ITransformationCollection>();

            // Act
            var builder = new MappedTransformationCollectionBuilder(fakeITransformCollection, 500);

            // Assert
            Assert.Same(fakeITransformCollection, builder.Transformations);
        }

        [Fact]
        public void WhenInitialized_WithANullITransformCollection_ExceptionThrown()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new MappedTransformationCollectionBuilder(null, 500));
            Assert.Equal("transformations", exception.ParamName);
        }
    }

    public class TransformationCollectionBuilderTest
    {
        [Fact]
        public void Map_WhenInvoked_ReturnsAnInstanceWithTheSameITransformationCollection()
        {
            // Arrange
            var builder = new TransformationCollectionBuilder();

            // Act
            var newBuilder = builder.Return(404);

            // Assert
            Assert.NotNull(newBuilder);
            Assert.IsAssignableFrom(typeof(MappedTransformationCollectionBuilder), newBuilder);
            Assert.Same(builder.Transformations, newBuilder.Transformations);
        }

        [Fact]
        public void Map_WhenInvoked_ReturnsAnInstanceWithTheSameStatusCode()
        {
            // Arrange
            var builder = new TransformationCollectionBuilder();

            // Act
            var newBuilder = builder.Return(404);

            // Assert
            Assert.NotNull(newBuilder);
            Assert.IsAssignableFrom(typeof(MappedTransformationCollectionBuilder), newBuilder);
            Assert.Equal(404, newBuilder.StatusCode);
        }
    }
}
