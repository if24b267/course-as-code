using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter.Strategies.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters.ActivityGradesWriter
{
    public class ActivityGradesWriter
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";

        private readonly IGradeItemStrategyResolver _gradeItemStrategyResolver;
        private readonly IXmlHelperFactory _xmlHelperFactory;
        private readonly ILogger<ActivityGradesWriter> _logger;


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityGradesWriter"/> class
        /// </summary>
        /// <param name="gradeItemStrategyResolver">Strategy resolver for determining the strategy to use for writing the grades.xml file</param>
        /// <param name="xmlHelperFactory">Factory for creating XmlHelpers for writing XML files</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityGradesWriter"/></param>
        public ActivityGradesWriter(IGradeItemStrategyResolver gradeItemStrategyResolver, IXmlHelperFactory xmlHelperFactory, ILogger<ActivityGradesWriter> logger)
        {
            _gradeItemStrategyResolver = gradeItemStrategyResolver ?? throw new ArgumentNullException(nameof(gradeItemStrategyResolver));
            _xmlHelperFactory = xmlHelperFactory ?? throw new ArgumentNullException(nameof(xmlHelperFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                using (var xmlHelper = _xmlHelperFactory.Create(filePath))
                {
                    xmlHelper.WriteElement("activity_gradebook");

                    xmlHelper.WriteElement("grade_items");
                    WriteGradeItems(xmlHelper, activity);
                    xmlHelper.CloseElement(); // Close <grade_items>

                    xmlHelper.WriteElement("grade_letters");
                    xmlHelper.WriteWhitespace("\n");
                    xmlHelper.CloseElement(); // Close <grade_letters>

                    xmlHelper.CloseElement(); // Close <activity_gradebook>
                }

                _logger.LogDebug("Created {Path}/{FileName}", Path.GetFileName(outputPath), DEFAULT_GRADES_RELATIVE_PATH);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error writing {Path}/{FileName}", Path.GetFileName(outputPath), DEFAULT_GRADES_RELATIVE_PATH);
                throw;
            }
        }


        /// <summary>
        /// Writes the `grade_items` section of the grades.xml file required for an activity
        /// </summary>
        /// <param name="xmlHelper">XML helper initialized on the correct document.</param>
        /// <param name="activity">The activity for which the section is being written.</param>
        private void WriteGradeItems(IXmlHelper xmlHelper, Activity activity)
        {
            IGradeItemStrategy strategy = _gradeItemStrategyResolver.GetStrategy(activity);
            strategy.WriteGradeItems(xmlHelper, activity);
        }
    }
}
