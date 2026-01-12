using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

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

            try
            {
                // DeserializeFromContent YAML to object
                var yamlLesson = _yamlFileReaderHelper.DeserializeFromFile<YamlLesson>(modelObjectPath);

                var moodleLesson = ConvertYamlLessonToMoodleLesson(yamlLesson);
                ValueKeeper.CurrentModuleId++;
                return moodleLesson;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing Lesson from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading Lesson from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private Lesson? ConvertYamlLessonToMoodleLesson(YamlLesson yamlLesson)
        {
            var lesson = new Lesson()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlLesson.Name,
                ModuleId = ValueKeeper.CurrentModuleId,
                ModuleName = yamlLesson.ModuleName,
                CourseId = ValueKeeper.CourseId,
                Context = yamlLesson.Context,
                Intro = yamlLesson.Intro,
                IntroFormat = yamlLesson.IntroFormat,
                Grade = yamlLesson.Grade,
                Practice = yamlLesson.Practice,
                MaxAttempts = yamlLesson.MaxAttempts,
                Review = yamlLesson.Review,
                Retake = yamlLesson.Retake,
                ProgressBar = yamlLesson.ProgressBar,
                DisplayLeft = yamlLesson.DisplayLeft,
                UsePassword = yamlLesson.UsePassword,
                Password = yamlLesson.Password,
                TimeLimit = yamlLesson.TimeLimit,
                Available = yamlLesson.Available,
                Deadline = yamlLesson.Deadline,
                Conditions = ConvertYamlLessonConditionsToMoodleLessonConditions(yamlLesson.Conditions),
                Pages = ConvertYamlLessonPagesToMoodleLessonPages(yamlLesson.Pages),
            };
            return lesson;
        }

        private Lesson.LessonConditions ConvertYamlLessonConditionsToMoodleLessonConditions(
            YamlLessonCondition condition)
        {
            return new Lesson.LessonConditions()
            {
                Completed = condition.Completed,
                GradeBetterThan = condition.GradeBetterThan,
                TimeSpentSeconds = condition.TimeSpentSeconds,
            };
        }

        private List<Lesson.LessonPage> ConvertYamlLessonPagesToMoodleLessonPages(
            List<YamlLessonPage> yamlLessonPages)
        {
            List<Lesson.LessonPage> lessonPages = [];
            foreach (YamlLessonPage yamlLessonPage in yamlLessonPages)
            {
                lessonPages.Add(new Lesson.LessonPage()
                {
                    PageId = ValueKeeper.CurrentLessonPageId,
                    Title = yamlLessonPage.Title,
                    Content = yamlLessonPage.Content,
                    ContentFormat = yamlLessonPage.ContentFormat,
                });
                ValueKeeper.CurrentLessonPageId++;
            }

            return lessonPages;
        }
    }
}
