using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlWikiPageReader : IWikiPageReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;

        public YamlWikiPageReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        public WikiPage? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                // DeserializeFromContent YAML to object
                var yamlWikiPage = _yamlReaderHelper.DeserializeFromFile<YamlWikiPage>(modelObjectPath);

                // Validate the URL format
                var validationResults = _yamlReaderHelper.ValidateObject(yamlWikiPage);
                if (validationResults.Any())
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warn($"Validation warnings for URL '{modelObjectPath}': {errors}");
                }

                var moodleWikiPage = ConvertYamlWikiPageToMoodleWikiPage(yamlWikiPage);
                return moodleWikiPage;
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

        private WikiPage ConvertYamlWikiPageToMoodleWikiPage(YamlWikiPage yamlWikiPage)
        {
            return new WikiPage()
            {
                PageId = ValueKeeper.CurrentWikiPageId,
                Title = yamlWikiPage.Title,
                CachedContent = yamlWikiPage.CachedContent,
                ReadOnly = yamlWikiPage.ReadOnly
            };
        }
    }
}
