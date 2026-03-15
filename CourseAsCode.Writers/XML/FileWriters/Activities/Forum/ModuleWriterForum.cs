using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.Forum
{
    public class ModuleWriterForum : ModuleWriterBase<Model.Activities.Resources.Forum>
    {
        public ModuleWriterForum(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterForum> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(Model.Activities.Resources.Forum activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(Model.Activities.Resources.Forum activity) =>
            "forum";

        protected override string GetIdNumber(Model.Activities.Resources.Forum activity) =>
            "$@NULL@$";
    }
}
