using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlGradingInfosReader : IYamlGradingInfosReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;

        public YamlGradingInfosReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        public List<YamlGradingInfo>? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                // DeserializeFromContent YAML to object
                var yamlGradingInfos = _yamlReaderHelper.DeserializeFromFile<YamlGradingInfos>(modelObjectPath);

                // Validate the URL format
                var validationResults = _yamlReaderHelper.ValidateObject(yamlGradingInfos);
                if (validationResults.Any())
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warn($"Validation warnings for GradingInfos '{modelObjectPath}': {errors}");
                }

                foreach (var yamlGradingInfo in yamlGradingInfos.GradingInfos)
                {
                    yamlGradingInfo.PathToActivity =
                        ConvertGradingInfosRelativePathToAbsolutePath(yamlGradingInfo.PathToActivity, modelObjectPath);
                }

                return yamlGradingInfos.GradingInfos;
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

        private string ConvertGradingInfosRelativePathToAbsolutePath(string activityPath, string filePath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(filePath, activityPath));
            return fullPath;
        }
    }
}
