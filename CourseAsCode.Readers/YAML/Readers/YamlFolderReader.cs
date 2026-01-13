using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlFolderReader : IFolderReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;
        private readonly IFileReader _yamlFileReader;

        public YamlFolderReader(IYamlFileReaderHelper yamlReaderHelper, IFileReader yamlFileReader)
        {
            _yamlReaderHelper = yamlReaderHelper;
            _yamlFileReader = yamlFileReader;
        }

        public Folder? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                var yamlFolder = _yamlReaderHelper.DeserializeFromFile<YamlFolder>(modelObjectPath);

                List<MoodleFile> files = [];

                foreach (var yamlFolderYamlFilePath in yamlFolder.YamlFilePaths)
                {
                    var fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelObjectPath), yamlFolderYamlFilePath));
                    var moodleFile = _yamlFileReader.Read(fullPath);
                    files.Add(moodleFile);
                }

                foreach (MoodleFile moodleFile in files)
                {
                    moodleFile.SystemDefaults.Component = Component.Folder;
                    moodleFile.SystemDefaults.FileArea = FileArea.CourseContent;
                    moodleFile.SystemDefaults.ItemId = ValueKeeper.CurrentFileItemId;
                    ValueKeeper.CurrentFileItemId++;
                }

                var moodleFolder = ConvertYamlFolderToMoodleFolder(yamlFolder, files);
                return moodleFolder;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing Folder from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading Folder from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private Folder ConvertYamlFolderToMoodleFolder(YamlFolder yamlFolder, List<MoodleFile> files)
        {
            return new Folder()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Description = yamlFolder.Description,
                DescriptionFormat = yamlFolder.DescriptionFormat,
                Name = yamlFolder.Name,
                VisibleToStudents = yamlFolder.VisibleToStudents,
                DisplayDescription = yamlFolder.DisplayDescription,
                Files = files,
            };
        }
    }
}
