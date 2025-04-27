using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GradebookWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADEBOOK_RELATIVE_PATH = "gradebook.xml";

        private int _gradeCategoryIdCounter = 1;
        private int _gradeItemIdCounter = 1;
        private int _gradeSettingIdCounter = 1;

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public GradebookWriter()
            : base(new DefaultXmlWriterFactory(), NLog.LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public GradebookWriter(IXmlWriterFactory xmlWriterFactory, NLog.ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteGradebook(Model.Course course, string outputPath)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADEBOOK_RELATIVE_PATH));

            try
            {
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("gradebook");

                    WriteAttributes(writer, course);
                    WriteGradeCategories(writer, course);
                    WriteGradeItems(writer, course);
                    WriteGradeLetters(writer, course);
                    WriteGradeSettings(writer, course);

                    writer.WriteEndElement(); // Close <gradebook>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_GRADEBOOK_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_GRADEBOOK_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// writes the `attributes` section of the gradebook.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteAttributes(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("attributes");
            writer.WriteEndElement(); // Close <attributes>
        }

        /// <summary>
        /// writes the `grade_categories` section of the gradebook.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteGradeCategories(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("grade_categories");

            WriteStartElementWithAttributes(writer, "grade_category", new Dictionary<string, string>
            {
                { "id", _gradeCategoryIdCounter++.ToString() },
            });

            // TODO: Determine what these values mean once we start implementing the gradebook
            var elements = new Dictionary<string, string>
            {
                { "parent", "$@NULL@$" },
                { "depth", "1" },
                { "path", $"/{_gradeCategoryIdCounter - 1}/" },
                { "fullname", "?" },
                { "aggregation", "13" },
                { "keephigh", "0" },
                { "droplow", "0" },
                { "aggregateonlygraded", "1" },
                { "aggregateoutcomes", "0" },
                { "timecreated", "1731353585" },
                { "timemodified", "1731353585" },
                { "hidden", "0" },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <grade_category>
            writer.WriteEndElement(); // Close <grade_categories>
        }

        /// <summary>
        /// writes the `grade_items` section of the gradebook.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteGradeItems(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("grade_items");

            WriteStartElementWithAttributes(writer, "grade_item", new Dictionary<string, string>
            {
                { "id", _gradeItemIdCounter++.ToString() },
            });

            // TODO: Determine what these values mean once we start implementing the gradebook
            var elements = new Dictionary<string, string>
            {
                { "categoryid", "$@NULL@$" },
                { "itemname", "$@NULL@$" },
                { "itemtype", "course" },
                { "itemmodule", "$@NULL@$" },
                { "iteminstance", "3" },
                { "itemnumber", "$@NULL@$" },
                { "iteminfo", "$@NULL@$" },
                { "idnumber", "$@NULL@$" },
                { "calculation", "$@NULL@$" },
                { "gradetype", "1" },
                { "grademax", "0.00000" },
                { "grademin", "0.00000" },
                { "scaleid", "$@NULL@$" },
                { "outcomeid", "$@NULL@$" },
                { "gradepass", "0.00000" },
                { "multfactor", "1.00000" },
                { "plusfactor", "0.00000" },
                { "aggregationcoef", "0.00000" },
                { "aggregationcoef2", "0.00000" },
                { "weightoverride", "0" },
                { "sortorder", "1" },
                { "display", "0" },
                { "decimals", "$@NULL@$" },
                { "hidden", "0" },
                { "locked", "0" },
                { "locktime", "0" },
                { "needsupdate", "0" },
                { "timecreated", "1731353585" },
                { "timemodified", "1731353585" },
            };

            WriteElements(writer, elements);

            writer.WriteStartElement("grade_grades");
            writer.WriteEndElement(); // Close <grade_grades>

            writer.WriteEndElement(); // Close <grade_item>
            writer.WriteEndElement(); // Close <grade_items>
        }

        /// <summary>
        /// writes the `grade_letters` section of the gradebook.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteGradeLetters(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("grade_letters");
            writer.WriteEndElement(); // Close grade_letters
        }

        /// <summary>
        /// writes the `grade_settings` section of the gradebook.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">initialized Moodle Course.</param>
        private void WriteGradeSettings(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("grade_settings");

            WriteStartElementWithAttributes(writer, "grade_setting", new Dictionary<string, string>
            {
                { "id", _gradeSettingIdCounter++.ToString() },
            });

            // TODO: Determine what these values mean once we start implementing the gradebook
            var elements = new Dictionary<string, string>
            {
                { "name", "minmaxtouse" },
                { "value", "1" },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <grade_setting>
            writer.WriteEndElement(); // Close <grade_settings>
        }
    }
}