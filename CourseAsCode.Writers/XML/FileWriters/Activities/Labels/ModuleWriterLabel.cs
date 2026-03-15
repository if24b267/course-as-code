using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class ModuleWriterLabel : ModuleWriterBase<TextAndMediaArea>
    {
        public ModuleWriterLabel(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterLabel> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(TextAndMediaArea activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(TextAndMediaArea activity) =>
            activity.ModuleName;

        protected override string GetIdNumber(TextAndMediaArea activity) =>
            "$@NULL@$";
    }
}
