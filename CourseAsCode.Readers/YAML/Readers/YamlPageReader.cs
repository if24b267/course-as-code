using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlPageReader : IYamlPageReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;
        public YamlPageReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        public Page? Read(string modelObjectPath)
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
                var yamlPage = _yamlReaderHelper.DeserializeFromFile<YamlPage>(modelObjectPath);

                var moodlePage = ConvertYamlPageToMoodlePage(yamlPage);
                ValueKeeper.CurrentModuleId++;
                return moodlePage;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing Page from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading Page from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private Page ConvertYamlPageToMoodlePage(YamlPage yamlPage)
        {
            var page = new Page
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Context = yamlPage.Context,
                Name = yamlPage.Name,
                Intro = yamlPage.Intro,
                IntroFormat = yamlPage.IntroFormat,
                Content = yamlPage.Content,
                ContentFormat = yamlPage.ContentFormat,
                ModuleName = yamlPage.ModuleName,
                ModuleId = ValueKeeper.CurrentModuleId,
            };
            return page;
        }
    }
}
