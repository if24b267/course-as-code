using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.YAML
{
    public class DeserializerValidation(INodeDeserializer nodeDeserializer) : INodeDeserializer
    {
        private const int MAX_STRING_LENGTH = 10000;
        private static readonly Regex UrlRegex = new(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled);

        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
        {
            if (!nodeDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value, rootDeserializer))
            {
                return false;
            }

            var context = new ValidationContext(value);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(value, context, results, true))
            {
                return true;
            }

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
                        // Normalize path separators first (optional but helpful)
                        var normalizedPath = value.Replace('/', Path.DirectorySeparatorChar)
                            .Replace('\\', Path.DirectorySeparatorChar);

                        // Check for invalid characters in the ENTIRE path
                        var invalidPathChars = Path.GetInvalidPathChars();
                        if (normalizedPath.IndexOfAny(invalidPathChars) >= 0)
                        {
                            results.Add(new ValidationResult(
                                $"Path contains invalid characters: {value}",
                                new[] { "FilePath" }));
                        }

                        // If this looks like a file path (has extension or ends with a filename)
                        if (Path.HasExtension(normalizedPath) || !Directory.Exists(normalizedPath))
                        {
                            // Check filename portion separately
                            var fileName = Path.GetFileName(normalizedPath);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                var invalidFileChars = Path.GetInvalidFileNameChars();
                                if (fileName.IndexOfAny(invalidFileChars) >= 0)
                                {
                                    results.Add(new ValidationResult(
                                        $"Filename contains invalid characters: {fileName}",
                                        new[] { "FilePath" }));
                                }
                            }
                        }

                        // Windows-specific checks
                        if (OperatingSystem.IsWindows())
                        {
                            // Check reserved device names in the last segment
                            var lastSegment = normalizedPath.Split(Path.DirectorySeparatorChar)
                                .LastOrDefault(s => !string.IsNullOrEmpty(s));
                            if (IsReservedDeviceName(lastSegment))
                            {
                                results.Add(new ValidationResult(
                                    $"Path contains reserved device name: {lastSegment}",
                                    new[] { "FilePath" }));
                            }

                            // Validate drive letter if present
                            if (normalizedPath.Length >= 2 && normalizedPath[1] == ':')
                            {
                                if (!IsValidDriveLetter(normalizedPath))
                                {
                                    results.Add(new ValidationResult(
                                        $"Invalid drive letter in path: {value}",
                                        new[] { "FilePath" }));
                                }
                            }
                        }

                        // Final validation - get full path (handles relative paths correctly)
                        var fullPath = Path.GetFullPath(normalizedPath);
                    }
                    catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
                    {
                        results.Add(new ValidationResult(
                            $"Invalid path format: {value} ({ex.Message})",
                            new[] { "FilePath" }));
                    }
                }
            }

            return results;
        }

        // Helper methods for Windows-specific checks
        private static bool IsReservedDeviceName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var deviceNames = new[]
            {
                "CON", "PRN", "AUX", "NUL",
                "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
            };

            foreach (var device in deviceNames)
            {
                if (fileName.Equals(device, StringComparison.OrdinalIgnoreCase) ||
                    fileName.StartsWith(device + ".", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsValidDriveLetter(string path)
        {
            if (path.Length < 2)
            {
                return false;
            }

            if (path[1] != ':')
            {
                return false;
            }

            char drive = char.ToUpper(path[0]);
            return drive >= 'A' && drive <= 'Z';
        }
    }
}
