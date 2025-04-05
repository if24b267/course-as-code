using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.YAML;

public class DeserializerValidation(INodeDeserializer nodeDeserializer) : INodeDeserializer
{
	private const int MAX_STRING_LENGTH = 10000;
	private static readonly Regex UrlRegex = new(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled);

	public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value,
		ObjectDeserializer rootDeserializer)
	{
		if (!nodeDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value, rootDeserializer))
			return false;
		var context = new ValidationContext(value);
		var results = new List<ValidationResult>();
		if (Validator.TryValidateObject(value, context, results, true))
			return true;
		var message = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
		throw new YamlException(message);
	}

	/// <summary>
	/// Validates a deserialized object with both standard validation annotations and enhanced checks
	/// </summary>
	/// <param name="obj">The object to validate</param>
	/// <returns>A list of validation results, empty if validation passed</returns>
	public static List<ValidationResult> ValidateObject(object obj)
	{
		var results = new List<ValidationResult>();

		// Standard validation (same as DeserializerValidation)
		var context = new ValidationContext(obj);
		Validator.TryValidateObject(obj, context, results, true);

		// Additional validations specific to EnhancedDeserializerValidation
		if (obj != null)
		{
			results.AddRange(ValidateStringPropertyLengths(obj));
			results.AddRange(ValidateUrls(obj));
			results.AddRange(ValidateFilePaths(obj));
		}

		return results;
	}

	/// <summary>
	/// Validates an object and throws a YamlException if validation fails
	/// </summary>
	/// <param name="obj">The object to validate</param>
	/// <exception cref="YamlException">Thrown if validation fails</exception>
	public static void ValidateObjectAndThrow(object obj)
	{
		var results = ValidateObject(obj);
		if (results.Count > 0)
		{
			var message = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
			throw new YamlException(message);
		}
	}

	private static IEnumerable<ValidationResult> ValidateStringPropertyLengths(object obj)
	{
		var results = new List<ValidationResult>();
		var properties = obj.GetType().GetProperties()
			 .Where(p => p.PropertyType == typeof(string));

		foreach (var property in properties)
		{
			var value = property.GetValue(obj) as string;
			if (!string.IsNullOrEmpty(value) && value.Length > MAX_STRING_LENGTH)
			{
				results.Add(new ValidationResult(
					 $"The {property.Name} field exceeds the maximum allowed length of {MAX_STRING_LENGTH} characters.",
					 new[] { property.Name }));
			}
		}

		return results;
	}

	private static IEnumerable<ValidationResult> ValidateUrls(object obj)
	{
		var results = new List<ValidationResult>();
		var property = obj.GetType().GetProperty("URL");

		if (property != null && property.PropertyType == typeof(string))
		{
			var value = property.GetValue(obj) as string;
			if (!string.IsNullOrEmpty(value) && !UrlRegex.IsMatch(value))
			{
				results.Add(new ValidationResult(
					 $"The URL field contains an invalid URL format: {value}.",
					 new[] { "URL" }));
			}
		}

		return results;
	}

	private static IEnumerable<ValidationResult> ValidateFilePaths(object obj)
	{
		var results = new List<ValidationResult>();
		var property = obj.GetType().GetProperty("FilePath");

		if (property != null && property.PropertyType == typeof(string))
		{
			var value = property.GetValue(obj) as string;
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					// Check if the path has invalid characters
					var invalidChars = Path.GetInvalidPathChars();

					if (value.IndexOfAny(invalidChars) >= 0)
					{
						results.Add(new ValidationResult(
							$"The FilePath field contains invalid characters: {value}.",
							new[] { "FilePath" }));
					}
					var fullPath = Path.GetFullPath(value);
				}
				catch (Exception)
				{
					results.Add(new ValidationResult(
						$"The FilePath field contains an invalid path format: {value}.",
						new[] { "FilePath" }));
				}
			}
		}

		return results;
	}
}