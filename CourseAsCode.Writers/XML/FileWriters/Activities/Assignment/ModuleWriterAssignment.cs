using System.Xml;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Assignment
{
    public class ModuleWriterAssignment : ModuleWriterBase<Model.Activities.Resources.Assignment>
    {
        public ModuleWriterAssignment(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterAssignment> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, moodleVersionHelper, generalHelper, logger, tagsWriter)
        {
        }

        protected override string GetModuleId(Model.Activities.Resources.Assignment activity) =>
            activity.ActivityId.ToString();

        protected override string GetModuleName(Model.Activities.Resources.Assignment activity) =>
            "assign";

        protected override string GetIdNumber(Model.Activities.Resources.Assignment activity) =>
            "$@NULL@$";

        protected override string GetIndent(Model.Activities.Resources.Assignment activity) =>
            "1";

        protected override string GetGroupingId(Model.Activities.Resources.Assignment activity) =>
            activity.TeamSubmissionGroupingId.ToString();

        protected override string GetCompletion(Model.Activities.Resources.Assignment activity) =>
            activity.CompletionSubmit ? "1" : "0";

        protected override void WriteAdditionalContent(XmlWriter writer, Model.Activities.Resources.Assignment activity)
        {
            WritePlagiarismTurnitinSimModule(writer, activity);
        }

        private void WritePlagiarismTurnitinSimModule(XmlWriter writer, Model.Activities.Resources.Assignment assignmentObject)
        {
            writer.WriteStartElement("plugin_plagiarism_turnitinsim_module");

            writer.WriteStartElement("turnitinsim_mods");
            writer.WriteStartElement("turnitinsim_mod");
            writer.WriteAttributeString("id", assignmentObject.ActivityId.ToString());

            var turnitinElements = new Dictionary<string, string>
            {
                { "turnitinenabled", "0" },
                { "reportgeneration", "0" },
                { "queuedrafts", "0" },
                { "addtoindex", "0" },
                { "excludequotes", "0" },
                { "excludebiblio", "0" },
                { "accessstudents", "0" },
            };
            WriteElements(writer, turnitinElements);

            writer.WriteEndElement(); // Close <turnitinsim_mod>
            writer.WriteEndElement(); // Close <turnitinsim_mods>

            writer.WriteStartElement("turnitinsim_subs");
            writer.WriteStartElement("turnitinsim_sub");
            writer.WriteAttributeString("id", "");
            writer.WriteEndElement(); // Close <turnitinsim_sub>
            writer.WriteEndElement(); // Close <turnitinsim_subs>

            writer.WriteStartElement("turnitinsim_usrs");
            writer.WriteEndElement(); // Close <turnitinsim_usrs>

            writer.WriteEndElement(); // Close <plugin_plagiarism_turnitinsim_module>
        }
    }
}
