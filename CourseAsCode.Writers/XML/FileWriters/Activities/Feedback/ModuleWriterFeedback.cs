using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;
using FeedbackModel = CourseAsCode.Model.Activities.Resources.Feedback;

namespace CourseAsCode.Writers.XML.Writer.Activities.Feedback
{
    public class ModuleWriterFeedback : ModuleWriterBase<FeedbackModel>
    {
        public ModuleWriterFeedback(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterFeedback> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(FeedbackModel activity) =>
            activity.SystemDefaults.ModuleId.ToString();

        protected override string GetModuleName(FeedbackModel activity) =>
            activity.SystemDefaults.ModuleName;

        protected override string GetIdNumber(FeedbackModel activity) =>
            string.Empty;

        protected override string GetLang(FeedbackModel activity) =>
            "$@NULL@$";

        protected override void WriteAdditionalContent(XmlWriter writer, FeedbackModel activity)
        {
            WriteEmptyPlagiarismPlugin(writer);
        }
    }
}
