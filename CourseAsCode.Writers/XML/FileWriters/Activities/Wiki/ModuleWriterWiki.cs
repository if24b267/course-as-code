using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Wiki
{
    public class ModuleWriterWiki : ModuleWriterBase<Model.Activities.Resources.Wiki>
    {
        public ModuleWriterWiki(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterWiki> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(Model.Activities.Resources.Wiki activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(Model.Activities.Resources.Wiki activity) =>
            "wiki";

        protected override string GetIdNumber(Model.Activities.Resources.Wiki activity) =>
            activity.CustomId?.ToString() ?? "$@NULL@$";
    }
}
