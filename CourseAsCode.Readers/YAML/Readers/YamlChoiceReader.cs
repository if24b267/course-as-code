using CourseAsCode.Model.Activities.Choice;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlChoiceReader : IChoiceReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;
        public YamlChoiceReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        public Choice? Read(string modelObjectPath)
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
                var yamlChoice = _yamlReaderHelper.DeserializeFromFile<YamlChoice>(modelObjectPath);

                var moodleChoice = ConvertYamlChoiceToMoodleChoice(yamlChoice);
                return moodleChoice;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing Choice from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading Choice from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private Choice ConvertYamlChoiceToMoodleChoice(YamlChoice yamlChoice)
        {
            return new Choice()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlChoice.Name,
                Context = yamlChoice.Context,
                Description = yamlChoice.Description,
                DescriptionFormat = yamlChoice.DescriptionFormat,
                Publish = yamlChoice.Publish,
                ShowResults = yamlChoice.ShowResults,
                Display = yamlChoice.Display,
                AllowUpdate = yamlChoice.AllowUpdate,
                AllowMultiple = yamlChoice.AllowMultiple,
                ShowUnanswered = yamlChoice.ShowUnanswered,
                LimitAnswers = yamlChoice.LimitAnswers,
                TimeOpen = yamlChoice.TimeOpen,
                TimeClose = yamlChoice.TimeClose,
                CompletionSubmit = yamlChoice.CompletionSubmit,
                ShowPreview = yamlChoice.ShowPreview,
                IncludeInactive = yamlChoice.IncludeInteractive,
                ShowAvailable = yamlChoice.ShowAvailable,
                Options = ConvertYamlChoiceOptionsToMoodleChoiceOptions(yamlChoice.Options),
            };
        }

        private List<Choice.ChoiceOption> ConvertYamlChoiceOptionsToMoodleChoiceOptions(List<YamlChoiceOption> yamlChoiceOptions)
        {
            List<Choice.ChoiceOption> options = [];

            foreach (YamlChoiceOption yamlChoiceOption in yamlChoiceOptions)
            {
                options.Add(new Choice.ChoiceOption()
                {
                    Id = ValueKeeper.CurrentChoiceOptionId,
                    Text = yamlChoiceOption.Text,
                    MaxAnswers = yamlChoiceOption.MaxAnswers,
                    TimeModified = DateTime.UtcNow,
                });
                ValueKeeper.CurrentChoiceOptionId++;
            }

            return options;
        }
    }
}
