using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlLessonReader : ILessonReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlFileReaderHelper;

        public YamlLessonReader(IYamlFileReaderHelper yamlFileReaderHelper)
        {
            _yamlFileReaderHelper = yamlFileReaderHelper;
        }

        public Lesson? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            var yamlFile = _yamlFileReaderHelper.DeserializeFromFile<YamlLesson>(modelObjectPath);
        }

        private Lesson? ConvertYamlLessonToMoodleLesson(YamlLesson yamlLesson)
        {

        }
    }
}
