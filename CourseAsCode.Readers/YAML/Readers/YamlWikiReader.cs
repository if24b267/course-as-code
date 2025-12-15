using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlWikiReader : IWikiReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;
        private readonly IWikiPageReader _wikiPageReader;

        public YamlWikiReader(IYamlFileReaderHelper yamlReaderHelper, IWikiPageReader wikiPageReader)
        {
            _yamlReaderHelper = yamlReaderHelper;
            _wikiPageReader = wikiPageReader;
        }

        public Wiki? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                // DeserializeFromContent YAML to object
                var yamlWiki = _yamlReaderHelper.DeserializeFromFile<YamlWiki>(modelObjectPath);

                // Validate the URL format
                var validationResults = _yamlReaderHelper.ValidateObject(yamlWiki);
                if (validationResults.Any())
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warn($"Validation warnings for URL '{modelObjectPath}': {errors}");
                }

                List<WikiSubwiki> subwikis = [];

                foreach (YamlWikiSubwiki yamlWikiSubwiki in yamlWiki.Subwikis)
                {
                    List<WikiPage> wikiPages = [];

                    foreach (var pagePath in yamlWikiSubwiki.PagePaths)
                    {
                        var fullPagePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelObjectPath), pagePath));
                        wikiPages.Add(_wikiPageReader.Read(fullPagePath));
                        ValueKeeper.CurrentWikiPageId++;
                    }

                    WikiSubwiki subwiki = new WikiSubwiki()
                    {
                        SubwikiId = ValueKeeper.CurrentSubwikiId,
                        GroupId = yamlWikiSubwiki.GroupId,
                        UserId = yamlWikiSubwiki.UserId,
                        Pages = wikiPages
                    };
                    ValueKeeper.CurrentSubwikiId++;
                    subwikis.Add(subwiki);
                }

                var moodleWiki = ConvertYamlWikiToMoodleWiki(yamlWiki, subwikis);
                return moodleWiki;
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

        private Wiki? ConvertYamlWikiToMoodleWiki(YamlWiki yamlWiki, List<WikiSubwiki> subwikis)
        {
            return new Wiki
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlWiki.Name,
                Description = yamlWiki.Description,
                Intro = yamlWiki.Intro,
                IntroFormat = yamlWiki.IntroFormat,
                WikiMode = yamlWiki.WikiMode,
                DefaultFormat = yamlWiki.DefaultFormat,
                ForceFormat = yamlWiki.ForceFormat,
                Subwikis = subwikis,
                ModuleId = ValueKeeper.CurrentModuleId,
                ModuleName = "wiki"
            };
        }
    }
}
