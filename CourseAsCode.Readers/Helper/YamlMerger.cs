using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers.Helper
{
    public static class YamlMerger
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder().Build();
        private static readonly ISerializer _serializer = new SerializerBuilder().Build();

        /// <summary>
        /// Deep merges a source YAML string (which overrides) into a base YAML string (defaults).
        /// </summary>
        public static string MergeYamlStrings(string baseYaml, string overrideYaml)
        {
            if (string.IsNullOrWhiteSpace(baseYaml)) return overrideYaml;
            if (string.IsNullOrWhiteSpace(overrideYaml)) return baseYaml;

            var baseDict = _deserializer.Deserialize<Dictionary<object, object>>(baseYaml) ?? new Dictionary<object, object>();
            var overrideDict = _deserializer.Deserialize<Dictionary<object, object>>(overrideYaml) ?? new Dictionary<object, object>();

            var mergedDict = MergeDictionaries(baseDict, overrideDict);
            return _serializer.Serialize(mergedDict);
        }

        private static Dictionary<object, object> MergeDictionaries(Dictionary<object, object> baseDict, Dictionary<object, object> overrideDict)
        {
            var result = new Dictionary<object, object>();

            // Copy base properties
            foreach (var kvp in baseDict)
            {
                result[kvp.Key] = kvp.Value;
            }

            // Apply overrides
            foreach (var kvp in overrideDict)
            {
                if (result.ContainsKey(kvp.Key))
                {
                    if (result[kvp.Key] is Dictionary<object, object> baseChild 
                        && kvp.Value is Dictionary<object, object> overrideChild)
                    {
                        // Deep merge nested dictionaries
                        result[kvp.Key] = MergeDictionaries(baseChild, overrideChild);
                    }
                    else if (result[kvp.Key] is List<object> baseList 
                             && kvp.Value is List<object> overrideList)
                    {
                        // For lists/sequences, we fully fully overwrite them in cascading configuration
                        // (Alternatively, we could append, but overwriting is more common for overrides)
                        result[kvp.Key] = overrideList;
                    }
                    else
                    {
                        // Overwrite scalars
                        result[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    // Introduce new property
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }
    }
}
