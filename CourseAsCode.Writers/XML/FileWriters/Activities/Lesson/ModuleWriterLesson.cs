using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;
using LessonModel = CourseAsCode.Model.Activities.Resources.Lesson;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.Lesson
{
    public class ModuleWriterLesson : ModuleWriterBase<LessonModel>
    {
        public ModuleWriterLesson(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterLesson> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(LessonModel activity) =>
            activity.ActivityId.ToString();

        protected override string GetModuleName(LessonModel activity) =>
            "lesson";

        protected override string GetIdNumber(LessonModel activity) =>
            "";
    }
}
