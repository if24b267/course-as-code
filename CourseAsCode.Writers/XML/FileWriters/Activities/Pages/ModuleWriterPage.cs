using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;
using PageModel = CourseAsCode.Model.Activities.Resources.Page;

namespace CourseAsCode.Writers.XML.Writer.Activities.Pages
{
    public class ModuleWriterPage : ModuleWriterBase<PageModel>
    {
        public ModuleWriterPage(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterPage> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(PageModel activity) =>
            activity.SystemDefaults.ModuleId.ToString();

        protected override string GetModuleName(PageModel activity) =>
            activity.SystemDefaults.ModuleName;

        protected override string GetIdNumber(PageModel activity) =>
            string.Empty;

        protected override string GetLang(PageModel activity) =>
            "$@NULL@$";

        protected override void WriteAdditionalContent(XmlWriter writer, PageModel activity)
        {
            WriteEmptyPlagiarismPlugin(writer);
        }
    }
}
