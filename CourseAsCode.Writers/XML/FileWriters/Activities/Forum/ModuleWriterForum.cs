using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.Forum
{
    // Erbt von der Basisklasse und implementiert das ActivityModuleWriter Interface
    public class ModuleWriterForum : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;

        // Konstruktor (wie in den anderen Writer-Klassen)
        public ModuleWriterForum(IXmlWriterFactory xmlWriterFactory, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper, ILogger<ModuleWriterForum> logger, ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }

        // 1. Hauptmethode, die das Schreiben der Datei startet
        public void WriteModule(string outputPath, Course course, Section section, Activity activityObject)
        {
            try
            {
                // Cast auf das spezifische Modell (Forum)
                if (activityObject is not Model.Activities.Resources.Forum forumObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Forum");
                }

                var filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("module");

                    // Attribute setzen
                    // Anmerkung: Die forum.xml verwendet 'moduleid' (57), aber die ModuleWriter verwenden 'ActivityId' oder 'ModuleId'
                    // Hier wird 'ModuleId' verwendet, wie in ModuleWriterFile.cs
                    writer.WriteAttributeString("id", forumObject.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    // Hauptlogik zum Schreiben der Elemente
                    WriteGeneralInformationToModules(writer, outputPath, course, section, forumObject);

                    writer.WriteEndElement(); // Close <module>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                throw;
            }
        }

        // 2. Hilfsmethode zum Schreiben der XML-Elemente
        private void WriteGeneralInformationToModules(XmlWriter writer, string outputPath, Course course, Section section, Model.Activities.Resources.Forum forumObject)
        {
            var elements = new Dictionary<string, string>
            {
                // Modulname ist "forum"
                { "modulename", "forum" },
                { "sectionid", section.SystemDefaults.SectionId.ToString() },
                { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                { "idnumber", "$@NULL@$" },
                { "added", _generalHelper.DateTimeToUnixTimestamp(forumObject.TimeCreated) },
                { "score", "0" },
                { "indent", "0" },

                // Sichtbarkeit
                { "visible", forumObject.VisibleToStudents ? "1" : "0" },
                { "visibleoncoursepage", forumObject.VisibleToStudents ? "1" : "0" },
                { "visibleold", forumObject.VisibleToStudents ? "1" : "0" },

                // Gruppeneinstellungen
                { "groupmode", _generalHelper.EnumIntegerToString(forumObject.GroupMode) },
                // Im Forum gibt es kein direktes GroupingId-Feld wie bei Assignment, daher 0 oder Null
                { "groupingid", "0" },

                // Abschluss (Completion)
                // Die Abschluss-Logik für Foren ist komplexer (CompletionDiscussions, etc.) und wird hier vereinfacht
                { "completion", "0" },
                { "completiongradeitemnumber", "$@NULL@$" },
                { "completionpassgrade", "0" },
                { "completionview", "0" },
                { "completionexpected", "0" },

                { "availability", "$@NULL@$" },

                // Beschreibung
                { "showdescription", forumObject.DisplayDescription ? "1" : "0" },
                { "downloadcontent", "1" },

                // Sprache
                { "lang", _generalHelper.GetLanguageFromEnum(forumObject.Language) },
            };
            WriteElements(writer, elements);

            // Tags schreiben
            _tagsWriter.WriteTags(writer, forumObject.Tags, outputPath);
        }
    }
}
