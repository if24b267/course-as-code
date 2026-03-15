using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.File
{
    public class ModuleWriterFile : ModuleWriterBase<MoodleFile>
    {
        public ModuleWriterFile(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterFile> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(MoodleFile activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(MoodleFile activity) =>
            activity.ModuleName;

        protected override string GetIdNumber(MoodleFile activity) =>
            "$@NULL@$";
    }
}
