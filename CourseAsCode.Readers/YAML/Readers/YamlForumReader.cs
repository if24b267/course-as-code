using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlForumReader : IForumReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;

        public YamlForumReader(IYamlFileReaderHelper yamlReaderHelper)
        {
            _yamlReaderHelper = yamlReaderHelper ?? throw new ArgumentNullException(nameof(yamlReaderHelper));
        }

        public Forum? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                var yamlForum = _yamlReaderHelper.DeserializeFromFile<YamlForum>(modelObjectPath);
                if (yamlForum == null)
                {
                    _logger.Error($"Could not deserialize forum from {modelObjectPath}");
                    return null;
                }

                return new Forum
                {
                    ActivityId = ValueKeeper.CurrentActivityId,
                    ModuleId = ValueKeeper.CurrentModuleId,
                    ModuleName = "forum",
                    Name = yamlForum.Name ?? "Forum",
                    ForumType = yamlForum.ForumType ?? "general",
                    Intro = yamlForum.Intro ?? "",
                    IntroFormat = (FormattingFormat)(yamlForum.IntroFormat ?? 1),
                    DueDate = yamlForum.DueDate ?? 0,
                    CutoffDate = yamlForum.CutoffDate ?? 0,
                    Assessed = yamlForum.Assessed ?? false,
                    AssessTimeStart = yamlForum.AssessTimeStart ?? 0,
                    AssessTimeFinish = yamlForum.AssessTimeFinish ?? 0,
                    Scale = yamlForum.Scale ?? 0,
                    MaxBytes = yamlForum.MaxBytes ?? 512000,
                    MaxAttachments = yamlForum.MaxAttachments ?? 1,
                    ForceSubscribe = yamlForum.ForceSubscribe ?? true,
                    TrackingType = yamlForum.TrackingType ?? 1,
                    RssType = yamlForum.RssType ?? 0,
                    RssArticles = yamlForum.RssArticles ?? 0,
                    TimeModified = DateTime.UtcNow,
                    WarnAfter = yamlForum.WarnAfter ?? 0,
                    BlockAfter = yamlForum.BlockAfter ?? 0,
                    BlockPeriod = yamlForum.BlockPeriod ?? 0,
                    CompletionDiscussions = yamlForum.CompletionDiscussions ?? 0,
                    CompletionReplies = yamlForum.CompletionReplies ?? 0,
                    CompletionPosts = yamlForum.CompletionPosts ?? 0,
                    DisplayWordCount = yamlForum.DisplayWordCount ?? false,
                    LockDiscussionAfter = yamlForum.LockDiscussionAfter ?? 0,
                    GradeForum = yamlForum.GradeForum ?? false,
                    Discussions = new(),
                    Subscriptions = new(),
                    Digests = new(),
                    ReadPosts = new(),
                    TrackedPrefs = new(),
                    PostsTags = new(),
                    Grades = new()
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error reading forum from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }
    }
}
