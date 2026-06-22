using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;
using FolderModel = CourseAsCode.Model.Activities.Resources.Folder;

namespace CourseAsCode.Writers.XML.Writer.Activities.Folder
{
    public class ModuleWriterFolder : ModuleWriterBase<FolderModel>
    {
        public ModuleWriterFolder(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterFolder> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(FolderModel activity) =>
            activity.SystemDefaults.ModuleId.ToString();

        protected override string GetModuleName(FolderModel activity) =>
            activity.SystemDefaults.ModuleName;

        protected override string GetIdNumber(FolderModel activity) =>
            string.Empty;

        protected override string GetLang(FolderModel activity) =>
            "$@NULL@$";

        protected override void WriteAdditionalContent(XmlWriter writer, FolderModel activity)
        {
            WriteEmptyPlagiarismPlugin(writer);
        }
    }
}
