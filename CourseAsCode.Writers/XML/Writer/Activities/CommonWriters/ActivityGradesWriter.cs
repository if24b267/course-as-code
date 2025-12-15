using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";

        public ActivityGradesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityGradesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }

        // Backward-compatible overload for existing tests and callers
        public void WriteGrade(string outputPath)
        {
            WriteGrade(outputPath, new List<Grade>());
        }

        /// <summary>
        /// Writes the grades.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        /// <param name="gradeItems">List of grade items to serialize.</param>
        public void WriteGrade(string outputPath, List<Grade> gradeItems)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity_gradebook");

                    writer.WriteStartElement("grade_items");
                    if (gradeItems != null)
                    {
                        foreach (var item in gradeItems)
                        {
                            WriteSingleGradeItem(writer, item);
                        }
                    }
                    writer.WriteEndElement(); // </grade_items>

                    writer.WriteStartElement("grade_letters");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // </grade_letters>

                    writer.WriteEndElement(); // </activity_gradebook>
                    writer.WriteEndDocument();
                }

                // Log full relative path as expected by tests: "<outputPath>/grades.xml"
                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                // Log full relative path as expected by tests: "<outputPath>/grades.xml"
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteSingleGradeItem(XmlWriter writer, Grade item)
        {
            WriteStartElementWithAttributes(writer, "grade_item", new Dictionary<string, string>
            {
                { "id", item.Id.ToString() }
            });

            var elements = new Dictionary<string, string>
            {
                { "categoryid", item.CategoryId.ToString() },
                { "itemname", item.ItemName ?? string.Empty },
                { "itemtype", item.ItemType ?? string.Empty },
                { "itemmodule", item.ItemModule ?? string.Empty },
                { "iteminstance", item.ItemInstance.ToString() },
                { "itemnumber", item.ItemNumber.ToString() },
                { "iteminfo", item.ItemInfo ?? "$@NULL@$" },
                { "idnumber", item.IdNumber ?? "" },
                { "calculation", item.Calculation ?? "$@NULL@$" },
                { "gradetype", item.GradeType.ToString() },
                { "grademax", item.GradeMax ?? "100.00000" },
                { "grademin", item.GradeMin ?? "0.00000" },
                { "scaleid", item.ScaleId ?? "$@NULL@$" },
                { "outcomeid", item.OutcomeId ?? "$@NULL@$" },
                { "gradepass", item.GradePass ?? "0.00000" },
                { "multfactor", item.MultFactor ?? "1.00000" },
                { "plusfactor", item.PlusFactor ?? "0.00000" },
                { "aggregationcoef", item.AggregationCoef ?? "0.00000" },
                { "aggregationcoef2", item.AggregationCoef2 ?? "0.14286" },
                { "weightoverride", item.WeightOverride.ToString() },
                { "sortorder", item.SortOrder.ToString() },
                { "display", item.Display.ToString() },
                { "decimals", item.Decimals ?? "$@NULL@$" },
                { "hidden", item.Hidden.ToString() },
                { "locked", item.Locked.ToString() },
                { "locktime", item.LockTime.ToString() },
                { "needsupdate", item.NeedsUpdate.ToString() },
                { "timecreated", item.TimeCreated ?? "0" },
                { "timemodified", item.TimeModified ?? "0" }
            };

            WriteElements(writer, elements);

            writer.WriteStartElement("grade_grades");
            if (item.GradeGrades != null)
            {
                foreach (var grade in item.GradeGrades)
                {
                    WriteSingleGradeGrade(writer, grade);
                }
            }
            writer.WriteEndElement(); // </grade_grades>

            writer.WriteEndElement(); // </grade_item>
        }

        private void WriteSingleGradeGrade(XmlWriter writer, Grade.GradeGrade grade)
        {
            WriteStartElementWithAttributes(writer, "grade_grade", new Dictionary<string, string>
            {
                { "id", grade.Id.ToString() }
            });

            var elements = new Dictionary<string, string>
            {
                { "userid", grade.UserId.ToString() },
                { "rawgrade", grade.RawGrade ?? "$@NULL@$" },
                { "rawgrademax", grade.RawGradeMax ?? "100.00000" },
                { "rawgrademin", grade.RawGradeMin ?? "0.00000" },
                { "rawscaleid", grade.RawScaleId ?? "$@NULL@$" },
                { "usermodified", grade.UserModified ?? "$@NULL@$" },
                { "finalgrade", grade.FinalGrade ?? "$@NULL@$" },
                { "hidden", grade.Hidden.ToString() },
                { "locked", grade.Locked.ToString() },
                { "locktime", grade.LockTime.ToString() },
                { "exported", grade.Exported.ToString() },
                { "overridden", grade.Overridden.ToString() },
                { "excluded", grade.Excluded.ToString() },
                { "feedback", grade.Feedback ?? "$@NULL@$" },
                { "feedbackformat", grade.FeedbackFormat.ToString() },
                { "information", grade.Information ?? "$@NULL@$" },
                { "informationformat", grade.InformationFormat.ToString() },
                { "timecreated", grade.TimeCreated ?? "$@NULL@$" },
                { "timemodified", grade.TimeModified ?? "$@NULL@$" },
                { "aggregationstatus", grade.AggregationStatus ?? "used" },
                { "aggregationweight", grade.AggregationWeight ?? "0.14286" }
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // </grade_grade>
        }
    }
}
