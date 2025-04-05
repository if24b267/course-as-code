using CourseAsCode.Readers.YAML;
using System.Globalization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NodeDeserializers;

namespace CourseAsCode.Readers.Helper;

public class YamlFileReaderHelper
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private static readonly string[] DATE_FORMATS = ["dd-MM-yyyy HH:mm"];
	private static readonly IDeserializer DESERIALIZER = new DeserializerBuilder()
		.WithTypeConverter(new DateTimeConverter(
			provider: CultureInfo.InvariantCulture,
			formats: DATE_FORMATS))
		.IgnoreUnmatchedProperties()
		.WithNodeDeserializer(i => new DeserializerValidation(i),
			s => s.InsteadOf<ObjectNodeDeserializer>())
		.Build();

	// Keep static methods for backwards compatibility and transition to DI
	public static T DeserializeFromContent<T>(string yamlContent)
	{
		_logger.Debug($"Deserializing YAML content to {typeof(T).Name}");
		if (string.IsNullOrWhiteSpace(yamlContent))
		{
			throw new ArgumentException("YAML content cannot be null or empty", nameof(yamlContent));
		}
		return DESERIALIZER.Deserialize<T>(yamlContent);
	}

	public static T DeserializeFromFile<T>(string filePath)
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

	// Instance methods for DI
	public T Deserialize<T>(string yamlContent)
	{
		return DeserializeFromContent<T>(yamlContent);
	}

	public T DeserializeFile<T>(string filePath)
	{
		return DeserializeFromFile<T>(filePath);
	}
}