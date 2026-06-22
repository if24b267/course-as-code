using System.ComponentModel.DataAnnotations;
using System.Globalization;

using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML;
using YamlDotNet.Serialization;
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

            try
            {
                var coursePath = ValueKeeper.CoursePath;
                var currentDir = Path.GetDirectoryName(Path.GetFullPath(filePath));
                
                if (!string.IsNullOrEmpty(coursePath) && currentDir != null)
                {
                    var fullCoursePath = Path.GetFullPath(coursePath);
                    var defaultContents = new List<string>();
                    
                    var typeName = typeof(T).Name;
                    var typeSuffix = typeName.StartsWith("Yaml", StringComparison.OrdinalIgnoreCase) 
                        ? typeName.Substring(4).ToLowerInvariant() 
                        : typeName.ToLowerInvariant();
                        
                    var dir = currentDir;
                    while (dir != null && dir.StartsWith(fullCoursePath, StringComparison.OrdinalIgnoreCase))
                    {
                        var defaultsDir = Path.Combine(dir, "defaults");
                        
                        var genericDefaultsPath = Path.Combine(defaultsDir, "default.yaml");
                        var specificDefaultsPath = Path.Combine(defaultsDir, $"default_{typeSuffix}.yaml");
                        
                        // Merge specific first (so it's added to the top of the list, evaluating generic right after)
                        // Actually, since we Insert(0), we should insert generic, then specific. So the base properties are evaluated first.
                        // Wait, if defaultContents is Insert(0), the LAST item inserted is the FIRST item in the list.
                        // The items at the START of the list are merged first. Wait.
                        // If we insert the higher-level (parent) folders first, they end up at index 0. 
                        // Let's just collect them in order from child to parent, and Insert(0) each.
                        // Inside a single folder: generic is parent to specific. 
                        // So for a given folder, we want generic to be merged BEFORE specific.
                        // Since we are iterating defaultContents from beginning to end:
                        // "mergedDefaults = YamlMerger.MergeYamlStrings(mergedDefaults, content)" -> right-side overwrites left-side.
                        // So if we have [ParentGeneric, ParentSpecific, ChildGeneric, ChildSpecific], it perfectly cascades!
                        // To achieve this, when walking UP the tree (child -> parent), we must Insert(0) Specific, THEN Insert(0) Generic!
                        
                        if (!string.Equals(specificDefaultsPath, Path.GetFullPath(filePath), StringComparison.OrdinalIgnoreCase) 
                            && File.Exists(specificDefaultsPath))
                        {
                            defaultContents.Insert(0, File.ReadAllText(specificDefaultsPath));
                        }
                        
                        if (!string.Equals(genericDefaultsPath, Path.GetFullPath(filePath), StringComparison.OrdinalIgnoreCase) 
                            && File.Exists(genericDefaultsPath))
                        {
                            defaultContents.Insert(0, File.ReadAllText(genericDefaultsPath));
                        }
                        
                        if (dir.Equals(fullCoursePath, StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }
                        
                        var parentDir = Directory.GetParent(dir);
                        if (parentDir == null) break;
                        dir = parentDir.FullName;
                    }
                    
                    // Collapse all defaults iteratively
                    var mergedDefaults = string.Empty;
                    foreach (var content in defaultContents)
                    {
                        mergedDefaults = string.IsNullOrEmpty(mergedDefaults) ? content : YamlMerger.MergeYamlStrings(mergedDefaults, content);
                    }
                    
                    // Final merge: specific file overrides the defaults
                    if (!string.IsNullOrEmpty(mergedDefaults))
                    {
                        yamlContent = YamlMerger.MergeYamlStrings(mergedDefaults, yamlContent);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, $"Failed to apply cascading defaults to {filePath}. Proceeding with original file contents.");
            }

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
