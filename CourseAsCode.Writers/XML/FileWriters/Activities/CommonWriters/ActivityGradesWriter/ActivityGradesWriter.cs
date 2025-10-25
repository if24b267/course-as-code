using System.Xml;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";

        private readonly IGradeItemStrategyResolver _gradeItemStrategyResolver;

        /// <summary>
        /// Creates a new instance of the <see cref="ActivityGradesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="gradeItemStrategyResolver">Strategy resolver for determining the strategy to use for writing the grades.xml file</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityGradesWriter"/></param>
        public ActivityGradesWriter(IXmlWriterFactory xmlWriterFactory, IGradeItemStrategyResolver gradeItemStrategyResolver, ILogger<ActivityGradesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
            _gradeItemStrategyResolver = gradeItemStrategyResolver ?? throw new ArgumentNullException(nameof(gradeItemStrategyResolver));
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
            IGradeItemStrategy strategy = _gradeItemStrategyResolver.GetStrategy(activity);
            strategy.WriteGradeItems(writer, activity);
        }
    }
}
