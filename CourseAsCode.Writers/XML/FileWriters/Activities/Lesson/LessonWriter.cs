using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using LessonModel = CourseAsCode.Model.Activities.Resources.Lesson;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.Lesson
{
    public class LessonWriter : XmlWriterBase
    {
        private const string DEFAULT_LESSON_RELATIVE_PATH = "lesson.xml";
        private readonly IGeneralHelper _generalHelper;

        public LessonWriter(IXmlWriterFactory xmlWriterFactory, ILogger<LessonWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WriteLesson(string outputPath, LessonModel lesson)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_LESSON_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, lesson);
                    WriteLessonTag(writer, lesson);

                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_LESSON_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_LESSON_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, LessonModel lesson)
        {
            writer.WriteAttributeString("id", lesson.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", lesson.ModuleId.ToString());
            writer.WriteAttributeString("modulename", "lesson");
            writer.WriteAttributeString("contextid", lesson.ActivityId.ToString());
        }

        private void WriteLessonTag(XmlWriter writer, LessonModel lesson)
        {
            writer.WriteStartElement("lesson");
            writer.WriteAttributeString("id", lesson.ActivityId.ToString());

            var elements = new Dictionary<string, string>
            {
                { "course", lesson.CourseId.ToString() },
                { "name", lesson.Name },
                { "intro", lesson.Intro ?? "" },
                { "introformat", _generalHelper.EnumIntegerToString(lesson.IntroFormat) },
                { "practice", lesson.Practice ? "1" : "0" },
                { "modattempts", lesson.ModAttempts ? "1" : "0" },
                { "usepassword", lesson.UsePassword ? "1" : "0" },
                { "password", lesson.Password ?? "" },
                { "dependency", lesson.Dependency.ToString() },
                { "conditions", SerializeConditions(lesson.Conditions) },
                { "grade", lesson.Grade.ToString() },
                { "custom", lesson.Custom ? "1" : "0" },
                { "ongoing", "0" },
                { "usemaxgrade", "0" },
                { "maxanswers", lesson.MaxAnswers.ToString() },
                { "maxattempts", lesson.MaxAttempts.ToString() },
                { "review", lesson.Review ? "1" : "0" },
                { "nextpagedefault", lesson.NextPageDefault.ToString() },
                { "feedback", lesson.Feedback.ToString() },
                { "minquestions", lesson.MinQuestions.ToString() },
                { "maxpages", lesson.MaxPages.ToString() },
                { "timelimit", lesson.TimeLimit.ToString() },
                { "retake", lesson.Retake ? "1" : "0" },
                { "activitylink", lesson.ActivityLink.ToString() },
                { "mediafile", lesson.MediaFile ?? "" },
                { "mediaheight", lesson.MediaHeight.ToString() },
                { "mediawidth", lesson.MediaWidth.ToString() },
                { "mediaclose", lesson.MediaClose ? "1" : "0" },
                { "slideshow", lesson.Slideshow ? "1" : "0" },
                { "width", lesson.Width.ToString() },
                { "height", lesson.Height.ToString() },
                { "bgcolor", lesson.BgColor ?? "#FFFFFF" },
                { "displayleft", lesson.DisplayLeft ? "1" : "0" },
                { "displayleftif", lesson.DisplayLeftIf ? "1" : "0" },
                { "progressbar", lesson.ProgressBar ? "1" : "0" },
                { "available", lesson.Available.HasValue ? _generalHelper.DateTimeToUnixTimestamp(lesson.Available.Value) : "0" },
                { "deadline", lesson.Deadline.HasValue ? _generalHelper.DateTimeToUnixTimestamp(lesson.Deadline.Value) : "0" },
                { "timemodified", lesson.TimeModified != default ? _generalHelper.DateTimeToUnixTimestamp(lesson.TimeModified) : _generalHelper.DateTimeToUnixTimestamp(DateTime.UtcNow) },
                { "completionendreached", lesson.CompletionEndReached ? "1" : "0" },
                { "completiontimespent", lesson.CompletionTimeSpent.ToString() },
                { "allowofflineattempts", lesson.AllowOfflineAttempts ? "1" : "0" },
            };

            WriteElements(writer, elements);

            WritePages(writer, lesson);

            writer.WriteStartElement("grades");
            writer.WriteEndElement();

            writer.WriteStartElement("timers");
            writer.WriteEndElement();

            writer.WriteStartElement("overrides");
            writer.WriteEndElement();

            writer.WriteEndElement(); // </lesson>
        }

        private void WritePages(XmlWriter writer, LessonModel lesson)
        {
            writer.WriteStartElement("pages");

            var pages = (lesson.Pages ?? new List<LessonModel.LessonPage>())
                .OrderBy(p => p.PageId)
                .ToList();

            if (pages.Count == 0)
            {
                writer.WriteEndElement(); // </pages>
                return;
            }

            var answerId = 1;

            foreach (var (page, index) in pages.Select((p, i) => (p, i)))
            {
                var prevPageId = index == 0 ? 0 : pages[index - 1].PageId;
                var nextPageId = index == pages.Count - 1 ? 0 : pages[index + 1].PageId;

                writer.WriteStartElement("page");
                writer.WriteAttributeString("id", page.PageId.ToString());

                var pageElements = new Dictionary<string, string>
                {
                    { "prevpageid", prevPageId.ToString() },
                    { "nextpageid", nextPageId.ToString() },
                    { "qtype", page.QType.ToString() },
                    { "qoption", page.QOption.ToString() },
                    { "layout", page.Layout.ToString() },
                    { "display", page.Display.ToString() },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(page.TimeModified) },
                    { "timemodified", _generalHelper.DateTimeToUnixTimestamp(page.TimeModified) },
                    { "title", page.Title ?? string.Empty },
                    { "contents", page.Content ?? string.Empty },
                    { "contentsformat", _generalHelper.EnumIntegerToString(page.ContentFormat) },
                };

                WriteElements(writer, pageElements);

                writer.WriteStartElement("answers");

                writer.WriteStartElement("answer");
                writer.WriteAttributeString("id", answerId.ToString());

                var jumpTo = nextPageId == 0 ? "0" : nextPageId.ToString();

                var answerElements = new Dictionary<string, string>
                {
                    { "jumpto", jumpTo },
                    { "grade", LessonDefaults.DEFAULT_ANSWER_GRADE.ToString() },
                    { "score", LessonDefaults.DEFAULT_ANSWER_SCORE.ToString() },
                    { "flags", LessonDefaults.DEFAULT_ANSWER_FLAGS.ToString() },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(page.TimeModified) },
                    { "timemodified", "0" },
                    { "answer_text", page.AnswerText ?? LessonDefaults.DEFAULT_ANSWER_TEXT },
                    { "response", LessonDefaults.DEFAULT_ANSWER_RESPONSE },
                    { "answerformat", LessonDefaults.DEFAULT_ANSWER_FORMAT.ToString() },
                    { "responseformat", LessonDefaults.DEFAULT_RESPONSE_FORMAT.ToString() },
                };

                WriteElements(writer, answerElements);

                writer.WriteStartElement("attempts");
                writer.WriteEndElement(); // </attempts>

                writer.WriteEndElement(); // </answer>
                answerId++;

                writer.WriteEndElement(); // </answers>

                writer.WriteStartElement("branches");
                writer.WriteEndElement(); // </branches>

                writer.WriteEndElement(); // </page>
            }

            writer.WriteEndElement(); // </pages>
        }

        private static string SerializeConditions(LessonModel.LessonConditions? conditions)
        {
            var timespent = conditions?.TimeSpentSeconds ?? LessonDefaults.DEFAULT_CONDITIONS_TIMESPENT_SECONDS;
            var completed = conditions?.Completed ?? LessonDefaults.DEFAULT_CONDITIONS_COMPLETED;
            var gradebetterthan = conditions?.GradeBetterThan ?? LessonDefaults.DEFAULT_CONDITIONS_GRADEBETTERTHAN;

            return $"O:8:\"stdClass\":3:{{s:9:\"timespent\";i:{timespent};s:9:\"completed\";i:{completed};s:15:\"gradebetterthan\";i:{gradebetterthan};}}";
        }
    }
}
