using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities;
using Microsoft.Extensions.Logging;
using SchedulerModel = CourseAsCode.Model.Activities.Resources.Scheduler;

namespace CourseAsCode.Writers.XML.Writer.Activities.Scheduler
{
    public class ModuleWriterScheduler : ModuleWriterBase<SchedulerModel>
    {
        public ModuleWriterScheduler(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterScheduler> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(SchedulerModel activity) =>
            activity.SystemDefaults.ModuleId.ToString();

        protected override string GetModuleName(SchedulerModel activity) =>
            activity.SystemDefaults.ModuleName;

        protected override string GetIdNumber(SchedulerModel activity) =>
            string.Empty;

        protected override string GetLang(SchedulerModel activity) =>
            "$@NULL@$";

        protected override void WriteAdditionalContent(XmlWriter writer, SchedulerModel activity)
        {
            WriteEmptyPlagiarismPlugin(writer);
        }
    }
}
