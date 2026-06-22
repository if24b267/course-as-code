using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Choice
{
    public class ModuleWriterChoice : ModuleWriterBase<CourseAsCode.Model.Activities.Choice.Choice>
    {
        public ModuleWriterChoice(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterChoice> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(CourseAsCode.Model.Activities.Choice.Choice activity) =>
            activity.SystemDefaults.ModuleId.ToString();

        protected override string GetModuleName(CourseAsCode.Model.Activities.Choice.Choice activity) =>
            activity.SystemDefaults.ModuleName;

        protected override string GetIdNumber(CourseAsCode.Model.Activities.Choice.Choice activity) =>
            "$@NULL@$";

        protected override void WriteAdditionalContent(XmlWriter writer, CourseAsCode.Model.Activities.Choice.Choice activity)
        {
            WriteEmptyPlagiarismPlugin(writer);
        }
    }
}
