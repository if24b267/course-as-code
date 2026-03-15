using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Quiz
{
    public class ModuleWriterQuiz : ModuleWriterBase<Model.Activities.Resources.Quiz>
    {
        public ModuleWriterQuiz(
            IXmlWriterFactory xmlWriterFactory,
            ILogger<ModuleWriterQuiz> logger,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(Model.Activities.Resources.Quiz activity) =>
            activity.ModuleId.ToString();

        protected override string GetModuleName(Model.Activities.Resources.Quiz activity) =>
            activity.ModuleName;

        protected override string GetIdNumber(Model.Activities.Resources.Quiz activity) =>
            string.Empty;
    }
}
