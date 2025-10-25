using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";

        private readonly IGeneralHelper _generalHelper;


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityGradesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="generalHelper">Instance of the GeneralHelper</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityGradesWriter"/></param>
        public ActivityGradesWriter(IXmlWriterFactory xmlWriterFactory, IGeneralHelper generalHelper, ILogger<ActivityGradesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }


        /// <summary>
        /// Writes the grades.xml file required for an activity.
        /// </summary>
        /// <param name="activity">The activity for which the grades.xml is being written.</param>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteGrade(Activity activity, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("activity_gradebook");

                    writer.WriteStartElement("grade_items");
                    WriteGradeItems(writer, activity);
                    writer.WriteEndElement(); // Close <grade_items>

                    writer.WriteStartElement("grade_letters");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <grade_letters>

                    writer.WriteEndElement(); // Close <activity_gradebook>

                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
                throw;
            }
        }


        /// <summary>
        /// Writes the `grade_items` section of the grades.xml file required for an activity
        /// </summary>
        /// <param name="writer">The XmlWriter initialized on the correct document.</param>
        /// <param name="activity">The activity for which the section is being written.</param>
        private void WriteGradeItems(XmlWriter writer, Activity activity)
        {
            // TODO: Use Strategy pattern or similar to comply with Open/Closed Principle
            if (activity is Quiz quiz)
            {
                WriteStartElementWithAttributes(writer, "grade_item", new Dictionary<string, string>
                {
                    { "id", activity.ActivityId.ToString() },
                });

                var gradeItemElements = new Dictionary<string, string>
                {
                    { "categoryid", quiz.GradeCategory.GradeCategoryId.ToString() },
                    { "itemname", quiz.GradeName ?? quiz.Name },
                    { "itemtype", "mod" },
                    { "itemmodule", quiz.ModuleName },
                    { "iteminstance", "1" },
                    { "itemnumber", "0" },
                    { "iteminfo", quiz.GradeInfo ?? "$@NULL@$" },
                    { "idnumber", quiz.CustomId != null ? quiz.CustomId.ToString() : "$@NULL@$" },
                    { "calculation", "$@NULL@$" },
                    { "gradetype", _generalHelper.EnumIntegerToString(quiz.GradeType) },
                    { "grademax", quiz.MaxGrade.ToString("F5", CultureInfo.InvariantCulture) },
                    { "grademin", 0.ToString("F5", CultureInfo.InvariantCulture) },
                    { "scaleid", "$@NULL@$" },
                    { "outcomeid", "$@NULL@$" },
                    { "gradepass", (quiz.GradeToPass ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                    { "multfactor", (quiz.GradeMultiplicator ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                    { "plusfactor", (quiz.GradeOffset ?? 0).ToString("F5", CultureInfo.InvariantCulture) },
                    { "aggregationcoef", 0.ToString("F5", CultureInfo.InvariantCulture) },
                    { "aggregationcoef2", ((quiz.GradingWeight ?? 0) / 100.0m).ToString("F5", CultureInfo.InvariantCulture) },
                    { "weightoverride", quiz.GradingWeight.HasValue ? "1" : "0" },
                    { "sortorder", "6" },
                    { "display", "0" },
                    { "decimals", "$@NULL@$" },
                    { "hidden", quiz.GradeHiddenUntil != null ? _generalHelper.DateTimeToUnixTimestamp(quiz.GradeHiddenUntil.Value) : quiz.GradeHidden ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                    { "locked", quiz.GradingLocked ? _generalHelper.DateTimeToUnixTimestamp(DateTime.Now) : "0" },
                    { "locktime", quiz.GradingLockedAfter != null ? _generalHelper.DateTimeToUnixTimestamp(quiz.GradingLockedAfter.Value) : "0" },
                    { "needsupdate", "0" },
                    { "timecreated", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeCreated) },
                    { "timemodified", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeModified) },
                };

                WriteElements(writer, gradeItemElements);

                writer.WriteEndElement(); // Close <grade_item>
            }
        }
    }
}
