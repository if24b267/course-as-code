using CourseAsCode.Readers.YAML;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.Tests.YAML
{
    [TestFixture]
    public class EnhancedDeserializerValidationTests
    {
        private INodeDeserializer _baseDeserializer;
        private DeserializerValidation _validator;
        private IParser _parser;
        private Func<IParser, Type, object?> _nestedObjectDeserializer;
        private ObjectDeserializer _rootDeserializer;

        [SetUp]
        public void Setup()
        {
            _baseDeserializer = Substitute.For<INodeDeserializer>();
            _validator = new DeserializerValidation(_baseDeserializer);
            _parser = Substitute.For<IParser>();
            _nestedObjectDeserializer = Substitute.For<Func<IParser, Type, object?>>();
            _rootDeserializer = Substitute.For<ObjectDeserializer>();
        }

        [Test]
        public void Deserialize_BaseDeserializerReturnsFalse_ReturnsFalse()
        {
            // Arrange
            _baseDeserializer.Deserialize(_parser, typeof(object),
                 _nestedObjectDeserializer, out object? value, _rootDeserializer)
                 .Returns(x => { x[3] = null; return false; });

            // Act
            var result = _validator.Deserialize(_parser, typeof(object),
                 _nestedObjectDeserializer, out var actualValue, _rootDeserializer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(actualValue, Is.Null);
            });
        }
        
        [Test]
        public void Deserialize_ValidatesAfterDeserializationNotDuring()
        {
            // Arrange - this object would fail validation with an invalid URL
            var invalidObject = new TestClass { Name = "Test", URL = "not-a-valid-url" };
            _baseDeserializer.Deserialize(_parser, typeof(TestClass),
                 _nestedObjectDeserializer, out object? value, _rootDeserializer)
                 .Returns(x => { x[3] = invalidObject; return true; });

            // Act - should not throw exception during deserialization
            var result = _validator.Deserialize(_parser, typeof(TestClass),
                 _nestedObjectDeserializer, out var actualValue, _rootDeserializer);

            // Assert - deserialization succeeds even with invalid data
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(actualValue, Is.Not.Null);
                Assert.That(actualValue, Is.SameAs(invalidObject));
            });
            
            // Separate validation should fail
            var validationResults = DeserializerValidation.ValidateObject(invalidObject);
            Assert.That(validationResults, Is.Not.Empty);
        }

        [Test]
        public void Deserialize_ValidObject_ReturnsTrue()
        {
            // Arrange
            var validObject = new TestClass { Name = "Test", URL = "https://example.com" };
            _baseDeserializer.Deserialize(_parser, typeof(TestClass),
                 _nestedObjectDeserializer, out object? value, _rootDeserializer)
                 .Returns(x => { x[3] = validObject; return true; });

            // Act
            var result = _validator.Deserialize(_parser, typeof(TestClass),
                 _nestedObjectDeserializer, out var actualValue, _rootDeserializer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(actualValue, Is.Not.Null);
                Assert.That(actualValue, Is.SameAs(validObject));
            });
        }

        [Test]
        public void ValidateObject_WithValidRequiredProperty_ReturnsNoErrors()
        {
            // Arrange
            var validObject = new TestClassWithRequiredProperty
            {
                RequiredProperty = "Valid Value"
            };

            // Act
            var results = DeserializerValidation.ValidateObject(validObject).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void ValidateObject_WithMissingRequiredProperty_ReturnsErrors()
        {
            // Arrange
            var invalidObject = new TestClassWithRequiredProperty();

            // Act
            var results = DeserializerValidation.ValidateObject(invalidObject).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Empty);
                Assert.That(results[0].ErrorMessage, Does.Contain("RequiredProperty"));
            });
        }

        [Test]
        public void ValidateObject_WithInvalidUrl_ReturnsErrors()
        {
            // Arrange
            var invalidObject = new TestClass { Name = "Test", URL = "not-a-valid-url" };

            // Act
            var results = DeserializerValidation.ValidateObject(invalidObject).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Empty);
                Assert.That(results[0].ErrorMessage, Does.Contain("invalid URL format"));
            });
        }

        [Test]
        public void ValidateObject_WithValidUrl_ReturnsNoErrors()
        {
            // Arrange
            var validObject = new TestClass { Name = "Test", URL = "https://example.com" };

            // Act
            var results = DeserializerValidation.ValidateObject(validObject).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void ValidateObject_WithInvalidFilePath_ReturnsErrors()
        {
            // Arrange - using invalid characters in path
            var invalidObject = new TestFileClass { FilePath = "invalid*path?with:chars" };

            // Act
            var results = DeserializerValidation.ValidateObject(invalidObject).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Empty);
                Assert.That(results[0].ErrorMessage, Does.Contain("invalid characters"));
            });
        }

        [Test]
        public void ValidateObject_WithValidFilePath_ReturnsNoErrors()
        {
            // Arrange
            var validObject = new TestFileClass { FilePath = "path/to/valid/file.txt" };

            // Act
            var results = DeserializerValidation.ValidateObject(validObject).ToList();

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void ValidateObject_StringTooLong_ReturnsErrors()
        {
            // Arrange - string exceeding max length
            var longString = new string('a', 10001);
            var invalidObject = new TestClass { Name = longString, URL = "https://example.com" };

            // Act
            var results = DeserializerValidation.ValidateObject(invalidObject).ToList();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Empty);
                Assert.That(results[0].ErrorMessage, Does.Contain("exceeds the maximum allowed length"));
            });
        }

        private class TestClass
        {
            public string Name { get; set; } = "";
            public string URL { get; set; } = "";
        }

        private class TestFileClass
        {
            public string FilePath { get; set; } = "";
        }

        private class TestClassWithRequiredProperty
        {
            [Required]
            public string? RequiredProperty { get; set; }
        }
    }
}