using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Interfaces;

/// <summary>
/// Interface for YAML file reader and validator
/// </summary>
public interface IYamlFileReaderHelper
{
	/// <summary>
	/// DeserializeFromContent YAML content string to specified type
	/// </summary>
	/// <typeparam name="T">Type to deserialize to</typeparam>
	/// <param name="yamlContent">YAML content</param>
	/// <returns>Deserialized object</returns>
	T DeserializeFromContent<T>(string yamlContent);

	/// <summary>
	/// DeserializeFromContent YAML file to specified type
	/// </summary>
	/// <typeparam name="T">Type to deserialize to</typeparam>
	/// <param name="filePath">Path to YAML file</param>
	/// <returns>Deserialized object</returns>
	T DeserializeFromFile<T>(string filePath);

	/// <summary>
	/// Validates a deserialized object using enhanced validation rules
	/// </summary>
	/// <param name="obj">Object to validate</param>
	/// <returns>List of validation results, empty if valid</returns>
	List<ValidationResult> ValidateObject(object obj);

	/// <summary>
	/// Validates a deserialized object and throws if validation fails
	/// </summary>
	/// <param name="obj">Object to validate</param>
	/// <exception cref="YamlException">Thrown when validation fails</exception>
	void ValidateObjectAndThrow(object obj);
}