using System.ComponentModel.DataAnnotations;
using CourseAsCode.Readers.YAML;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.Tests.YAML
{
    [TestFixture]
    public class DeserializerValidationTests
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
            var customDeserializer = new CustomFailingDeserializer();
            var validator = new DeserializerValidation(customDeserializer);

            // Act
            var result = validator.Deserialize(_parser, typeof(object),
                 _nestedObjectDeserializer, out var value, _rootDeserializer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(value, Is.Null);
            });
        }

        // Custom deserializer that always returns false
        private class CustomFailingDeserializer : INodeDeserializer
        {
            public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
            {
                value = null;
                return false;
            }
        }

        [Test]
        public void Deserialize_ValidObject_ReturnsTrue()
        {
            // Arrange
            var validObject = new TestClassWithoutValidation { Name = "Test" };
            var customDeserializer = new CustomValidObjectDeserializer(validObject);
            var validator = new DeserializerValidation(customDeserializer);

            // Act
            var result = validator.Deserialize(_parser, typeof(TestClassWithoutValidation),
                 _nestedObjectDeserializer, out var value, _rootDeserializer);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(value, Is.Not.Null);
                Assert.That(value, Is.TypeOf<TestClassWithoutValidation>());
                var typedValue = (TestClassWithoutValidation)value;
                Assert.That(typedValue.Name, Is.EqualTo("Test"));
            });
        }

        // Custom deserializer that returns a valid object for testing
        private class CustomValidObjectDeserializer : INodeDeserializer
        {
            private readonly object _objectToReturn;

            public CustomValidObjectDeserializer(object objectToReturn)
            {
                _objectToReturn = objectToReturn;
            }

            public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
            {
                value = _objectToReturn;
                return true;
            }
        }

        [Test]
        public void Deserialize_InvalidObject_ThrowsYamlException()
        {
            // For this test, we'll create a real implementation rather than a mock
            // since mocking with out parameters is tricky
            var customDeserializer = new CustomInvalidObjectDeserializer();
            var validator = new DeserializerValidation(customDeserializer);

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 validator.Deserialize(_parser, typeof(TestClassWithRequiredProperty),
                      _nestedObjectDeserializer, out var value, _rootDeserializer));
        }

        // Custom deserializer that returns an invalid object for testing validation
        private class CustomInvalidObjectDeserializer : INodeDeserializer
        {
            public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
            {
                // Always return an invalid object (missing required property)
                value = new TestClassWithRequiredProperty();
                return true;
            }
        }

        private class TestClassWithoutValidation
        {
            public string? Name { get; set; }
        }

        private class TestClassWithRequiredProperty
        {
            [Required]
            public string? RequiredProperty { get; set; }
        }
    }
}
