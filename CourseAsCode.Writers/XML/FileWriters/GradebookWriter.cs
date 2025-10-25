using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters
{
    public class GradebookWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADEBOOK_RELATIVE_PATH = "gradebook.xml";

        private readonly IGeneralHelper _generalHelper;
        private readonly ILogger<GradebookWriter> _logger;

        private int _gradeItemIdCounter = 1;
        private int _gradeSettingIdCounter = 1;


        /// <summary>
        /// Creates a new instance of the GradebookWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="generalHelper">Instance of the GeneralHelper</param>
        /// <param name="logger">Logger instance specific to GradebookWriter</param>
        public GradebookWriter(IXmlWriterFactory xmlWriterFactory, IGeneralHelper generalHelper, ILogger<GradebookWriter> logger)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                    // Write all grade categories of the course
                    writer.WriteStartElement("grade_categories");
                    foreach (GradeCategory gradeCategory in course.GradeCategories)
                    {
                        WriteGradeCategory(writer, gradeCategory);
                    }
                    writer.WriteEndElement(); // Close <grade_categories>

                    // Write all grade items of the course
                    writer.WriteStartElement("grade_items");
                    foreach (GradeCategory gradeCategory in course.GradeCategories)
                    {
                        WriteGradeItem(writer, gradeCategory);
                    }
                    writer.WriteEndElement(); // Close <grade_items>

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
        /// writes the `grade_category` section of the gradebook.xml file for a single grade category
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="gradeCategory">The GradeCategory for which the grade category should be written.</param>
        private void WriteGradeCategory(XmlWriter writer, GradeCategory gradeCategory)
        {
            WriteStartElementWithAttributes(writer, "grade_category", new Dictionary<string, string>
            {
                { "id", gradeCategory.GradeCategoryId.ToString() },
            });

            var elements = new Dictionary<string, string>
            {
                { "parent", gradeCategory.Parent != null ? gradeCategory.Parent.GradeCategoryId.ToString() : "$@NULL@$" },
                { "depth", GetGradeCategoryDepth(GetGradeCategoryPath(gradeCategory)).ToString() }, // TODO: Doing the path calculation twice is not ideal
                { "path", GetGradeCategoryPath(gradeCategory) },
                { "fullname", gradeCategory.GradeName },
                { "aggregation", _generalHelper.EnumIntegerToString(gradeCategory.AggregationType) },
                { "keephigh", "0" },
                { "droplow", gradeCategory.DropLowestGrades != null ? gradeCategory.DropLowestGrades.ToString() : "0" },
                { "aggregateonlygraded", "1" },
                { "aggregateoutcomes", "0" },
                { "timecreated", _generalHelper.DateTimeToUnixTimestamp(gradeCategory.Created) },
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(gradeCategory.Modified) },
                { "hidden", "0" },
            };

            if (gradeCategory.AggregationType != GradeAggregation.Natural)
            {
                _logger.LogWarning($"You are using an aggregation type other than 'Natural' for grade category '{gradeCategory.GradeName}'. Make sure this aggregation type is enabled in your Moodle instance.");
            }

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <grade_category>
        }

        /// <summary>
        /// writes the `grade_items` section of the gradebook.xml file for a single grade category.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="gradeCategory">The GradeCategory for which the grade item should be written.</param>
        private void WriteGradeItem(XmlWriter writer, GradeCategory gradeCategory)
        {
            WriteStartElementWithAttributes(writer, "grade_item", new Dictionary<string, string>
            {
                { "id", _gradeItemIdCounter++.ToString() },
            });

            var elements = new Dictionary<string, string>
            {
                { "categoryid", "$@NULL@$" },
                { "itemname", gradeCategory.GradeName ?? "$@NULL@$" },
                { "itemtype", GetGradeCategoryDepth(GetGradeCategoryPath(gradeCategory)) == 1 ? "course" : "category" },
                { "itemmodule", "$@NULL@$" },
                { "iteminstance", gradeCategory.GradeCategoryId.ToString() },
                { "itemnumber", "$@NULL@$" },
                { "iteminfo", gradeCategory.GradeInfo ?? "$@NULL@$" },
                { "idnumber", "$@NULL@$" },
                { "calculation", "$@NULL@$" },
                { "gradetype", _generalHelper.EnumIntegerToString(gradeCategory.GradeType) },
                { "grademax", gradeCategory.MaxGrade.ToString("F5", CultureInfo.InvariantCulture) },
                { "grademin", 0.ToString("F5", CultureInfo.InvariantCulture) },
                { "scaleid", "$@NULL@$" },
                { "outcomeid", "$@NULL@$" },
                { "gradepass", (gradeCategory.GradeToPass ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "multfactor", (gradeCategory.GradeMultiplicator ?? 1).ToString("F5", CultureInfo.InvariantCulture) },
                { "plusfactor", (gradeCategory.GradeOffset ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                { "aggregationcoef", "0.00000" },
                { "aggregationcoef2", ((gradeCategory.GradingWeight ?? 0) / 100.0m).ToString("F5", CultureInfo.InvariantCulture) },
                { "weightoverride", gradeCategory.GradingWeight.HasValue ? "1" : "0" },
                { "sortorder", "1" },
                { "display", "0" },
                { "decimals", gradeCategory.GradeDecimalPoints != null ? gradeCategory.GradeDecimalPoints.ToString() : "$@NULL@$" },
                { "hidden", gradeCategory.GradeHiddenUntil != null ? _generalHelper.DateTimeToUnixTimestamp(gradeCategory.GradeHiddenUntil.Value) : gradeCategory.GradeHidden ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locked", gradeCategory.GradingLocked ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                { "locktime", gradeCategory.GradingLockedAfter != null ? _generalHelper.DateTimeToUnixTimestamp(gradeCategory.GradingLockedAfter.Value) : "0" },
                { "needsupdate", "0" },
                { "timecreated", "1731353585" },
                { "timemodified", "1731353585" },
            };

            WriteElements(writer, elements);

            writer.WriteStartElement("grade_grades");
            writer.WriteEndElement(); // Close <grade_grades>

            writer.WriteEndElement(); // Close <grade_item>
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


        /// <summary>
        /// Determine the path to a grade category (as required by gradebook.xml)
        /// </summary>
        /// <param name="gradeCategory">The grade category to determine the path for</param>
        /// <returns>The path to the grade category from its parents, e.g. /1/2/3</returns>
        private string GetGradeCategoryPath(GradeCategory gradeCategory)
        {
            List<int> categoryIds = new();
            categoryIds.Add(gradeCategory.GradeCategoryId);

            // Get the IDs of all parents of the category
            while (gradeCategory.Parent != null)
            {
                gradeCategory = gradeCategory.Parent;
                categoryIds.Add(gradeCategory.GradeCategoryId);
            }

            // Construct the string from the IDs
            /* categoryIds now contains for example 2, 1, 4
             * The last number is the highest parent
             * The first number is the lowest parent (the category we're currently looking at)
             * The path is therefore /4/2/1/
            */
            string path = "";

            for (int i = categoryIds.Count - 1; i >= 0; i--)
            {
                path += $"/{categoryIds[i]}";
            }
            path += "/";

            return path;
        }


        /// <summary>
        /// Determines the depth of a grade category based on its path
        /// </summary>
        /// <param name="gradeCategoryPath">The path to the grade category as determined by GetGradeCategoryPath</param>
        /// <returns>The depth of the grade category</returns>
        private int GetGradeCategoryDepth(string gradeCategoryPath)
        {
            /* /4/2/1/ -> Depth: 3
             * /4/2/ -> Depth: 2
             * /4/ -> Depth: 1
             *
             * -> Number of slashes in the path minus 1
             */

            return gradeCategoryPath.Count(c => c == '/') - 1;
        }
    }
}
