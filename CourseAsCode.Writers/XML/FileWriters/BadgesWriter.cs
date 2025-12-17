using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters
{
    public class BadgesWriter
    {
        private const string DEFAULT_BADGES_RELATIVE_PATH = "badges.xml";

        private readonly IXmlServiceFactory _xmlServiceFactory;
        private readonly ILogger<BadgesWriter> _logger;

        /// <summary>
        /// Creates a new instance of the BadgesWriter class
        /// </summary>
        /// <param name="xmlServiceFactory">Factory for creating XML Services</param>
        /// <param name="logger">Logger instance specific to BadgesWriter</param>
        public BadgesWriter(IXmlServiceFactory xmlServiceFactory, ILogger<BadgesWriter> logger)
        {
            this._xmlServiceFactory = xmlServiceFactory ?? throw new ArgumentNullException(nameof(xmlServiceFactory));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Writes the badges.xml file to the specified output path
        /// </summary>
        /// <param name="course">The course model for which badges.xml will be written</param>
        /// <param name="outputPath">The directory where the badges.xml file will be created.</param>
        public void WriteBadges(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_BADGES_RELATIVE_PATH));


                using (var xmlHelper = _xmlServiceFactory.Create(filePath))
                {
                    xmlHelper.WriteElement("badges");
                    xmlHelper.CloseElement(); // Close <badges>
                }

                _logger.LogDebug("Created {DEFAULT_BADGES_RELATIVE_PATH}", DEFAULT_BADGES_RELATIVE_PATH);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error writing {DEFAULT_BADGES_RELATIVE_PATH}", DEFAULT_BADGES_RELATIVE_PATH);
                throw;
            }
        }
    }
}
