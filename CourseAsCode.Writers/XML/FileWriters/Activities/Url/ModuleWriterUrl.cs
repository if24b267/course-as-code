using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url
{
    public class ModuleWriterUrl : ModuleWriterBase<URL>
    {
        public ModuleWriterUrl(
            IXmlWriterFactory xmlWriterFactory,
            ILogger<ModuleWriterUrl> logger,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(URL activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(URL activity) =>
            activity.ModuleName;

        protected override string GetIdNumber(URL activity) =>
            string.Empty;
    }
}
