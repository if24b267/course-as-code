using System.ComponentModel.DataAnnotations;
using System.Globalization;

using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NodeDeserializers;

namespace CourseAsCode.Readers.Helper
{
    /// <summary>
    /// Implementation of YAML file reader and validator with dependency injection support
    /// </summary>
    public class YamlFileReaderHelper : IYamlFileReaderHelper
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string[] _dateFormats = ["dd-MM-yyyy HH:mm"];

        private readonly IDeserializer _deserializer;

        public YamlFileReaderHelper()
        {
            // Standard deserializer - validates only required fields
            _deserializer = new DeserializerBuilder()
                .WithTypeConverter(new CustomDateTimeConverter(
                    provider: CultureInfo.InvariantCulture,
                    formats: _dateFormats))
                .IgnoreUnmatchedProperties()
                .WithNodeDeserializer(
                    i => new DeserializerValidation(i),
                    s => s.InsteadOf<ObjectNodeDeserializer>())
                .Build();
        }

        /// <summary>
        /// DeserializeFromContent YAML content to the specified type (validates only required fields)
        /// </summary>
        public T DeserializeFromContent<T>(string yamlContent)
        {
            _logger.Debug($"Deserializing YAML content to {typeof(T).Name}");
            if (string.IsNullOrWhiteSpace(yamlContent))
            {
                throw new ArgumentException("YAML content cannot be null or empty", nameof(yamlContent));
            }

            try
            {
                return _deserializer.Deserialize<T>(yamlContent);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, $"Unexpected error deserializing YAML content: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// DeserializeFromContent YAML file to the specified type (validates only required fields)
        /// </summary>
        public T DeserializeFromFile<T>(string filePath)
        {
            _logger.Debug($"Deserializing YAML file \"{filePath}\" to {typeof(T).Name}");
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var yamlContent = File.ReadAllText(filePath);
            return DeserializeFromContent<T>(yamlContent);
        }

        /// <summary>
        /// Validates a deserialized object using the enhanced validation rules
        /// </summary>
        /// <param name="obj">The object to validate</param>
        /// <returns>A list of validation results, empty if validation passed</returns>
        public List<ValidationResult> ValidateObject(object obj)
        {
            return DeserializerValidation.ValidateObject(obj);
        }

        /// <summary>
        /// Validates a deserialized object and throws YamlException if validation fails
        /// </summary>
        /// <param name="obj">The object to validate</param>
        public void ValidateObjectAndThrow(object obj)
        {
            DeserializerValidation.ValidateObjectAndThrow(obj);
        }
    }
}
