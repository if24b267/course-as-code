using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlFeedbackReader : IFeedbackReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;
        public YamlFeedbackReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper;
        }

        public Feedback? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                var yamlFeedback = _yamlReaderHelper.DeserializeFromFile<YamlFeedback>(modelObjectPath);


                var moodleFeedback = ConvertYamlFeedbackToMoodleFeedback(yamlFeedback);
                return moodleFeedback;
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

        private Feedback ConvertYamlFeedbackToMoodleFeedback(YamlFeedback yamlFeedback)
        {
            return new Feedback()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlFeedback.Name,
                Description = yamlFeedback.Description,
                DescriptionFormat = yamlFeedback.DescriptionFormat,
                Anonymous = yamlFeedback.Anonymous,
                EmailNotification = yamlFeedback.EmailNotification,
                MultipleSubmit = yamlFeedback.MultipleSubmit,
                Autonumbering = yamlFeedback.Autonumbering,
                PublishStats = yamlFeedback.PublishStats,
                CompletionSubmit = yamlFeedback.CompletionSubmit,
                Items = ConvertYamlFeedbackItemsToMoodleFeedbackItems(yamlFeedback.Items),
            };
        }

        private List<Feedback.FeedbackItem> ConvertYamlFeedbackItemsToMoodleFeedbackItems(List<YamlFeedbackItem> yamlFeedbackItems)
        {
            var feedbackItems = new List<Feedback.FeedbackItem>();
            foreach (var yamlItem in yamlFeedbackItems)
            {
                var feedbackItem = new Feedback.FeedbackItem()
                {
                    Id = ValueKeeper.CurrentFeedbackItemId,
                    Template = yamlItem.Template,
                    Name = yamlItem.Name,
                    Label = yamlItem.Label,
                    Type = yamlItem.Type,
                    Presentation = yamlItem.Presentation,
                    HasValue = yamlItem.HasValue,
                    Position = yamlItem.Position,
                    Required = yamlItem.Required,
                    DependItem = yamlItem.DependItem,
                    DependValue = yamlItem.DependValue,
                    Options = yamlItem.Options,
                };
                feedbackItems.Add(feedbackItem);
                ValueKeeper.CurrentFeedbackItemId++;
            }

            return feedbackItems;
        }
    }
}
