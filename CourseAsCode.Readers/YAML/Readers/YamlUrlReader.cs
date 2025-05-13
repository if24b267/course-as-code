using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlUrlReader : IUrlReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;

        public YamlUrlReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        // For backward compatibility
        public YamlUrlReader()
            : this(new YamlFileReaderHelper())
        {
        }

        public URL? Read(string modelObjectPath)
        {
            // TODO: maybe make this into a middleware
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                // DeserializeFromContent YAML to object
                var yamlUrl = _yamlReaderHelper.DeserializeFromFile<YamlUrl>(modelObjectPath);

                // Validate the URL format
                var validationResults = _yamlReaderHelper.ValidateObject(yamlUrl);
                if (validationResults.Any())
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warn($"Validation warnings for URL '{modelObjectPath}': {errors}");
                }

                var modelUrl = ConvertYamlUrlToModelUrl(yamlUrl);
                return modelUrl;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing URL from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading URL from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private URL? ConvertYamlUrlToModelUrl(YamlUrl yamlUrl)
        {
            // TODO: default values?
            return new URL()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlUrl.Name,
                ExternalUrl = yamlUrl.URL,
                Description = yamlUrl.Description,
            };
        }
    }
}
