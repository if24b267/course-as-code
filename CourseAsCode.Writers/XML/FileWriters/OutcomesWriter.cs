using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters
{
    public class OutcomesWriter
    {
        private const string DEFAULT_OUTCOMES_RELATIVE_PATH = "outcomes.xml";

        private readonly IXmlServiceFactory _xmlServiceFactory;
        private readonly ILogger<OutcomesWriter> _logger;

        /// <summary>
        /// Creates a new instance of the OutcomesWriter class
        /// </summary>
        /// <param name="xmlServiceFactory">Factory to create XML services</param>
        /// <param name="logger">Logger instance specific to OutcomesWriter</param>
        public OutcomesWriter(IXmlServiceFactory xmlServiceFactory, ILogger<OutcomesWriter> logger)
        {
            this._xmlServiceFactory = xmlServiceFactory ?? throw new ArgumentNullException(nameof(xmlServiceFactory));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Writes the outcomes.xml file required for a course
        /// </summary>
        /// <param name="course">The course model for which outcomes.xml will be written</param>
        /// <param name="outputPath">The base output path where the outcomes.xml file will be created</param>
        public void WriteOutcomes(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_OUTCOMES_RELATIVE_PATH));

                using (var xmlHelper = _xmlServiceFactory.Create(filePath))
                {
                    xmlHelper.WriteElement("outcomes_definition");
                    xmlHelper.CloseElement(); // Close <outcomes_definition>
                }

                _logger.LogDebug("Created {DEFAULT_OUTCOMES_RELATIVE_PATH}", DEFAULT_OUTCOMES_RELATIVE_PATH);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error writing {DEFAULT_OUTCOMES_RELATIVE_PATH}", DEFAULT_OUTCOMES_RELATIVE_PATH);
                throw;
            }
        }
    }
}
