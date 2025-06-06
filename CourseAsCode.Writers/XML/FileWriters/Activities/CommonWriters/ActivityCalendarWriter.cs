using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCalendarWriter : XmlWriterBase
    {
        private const string DEFAULT_CALENDAR_RELATIVE_PATH = "calendar.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityCalendarWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityCalendarWriter"/></param>
        public ActivityCalendarWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityCalendarWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the calendar.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteCalendar(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CALENDAR_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("events");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close <events>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_CALENDAR_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_CALENDAR_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
