using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CalendarWriter : XmlWriterBase
    {
        private const string DEFAULT_CALENDAR_RELATIVE_PATH = "course/calendar.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="CalendarWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="CalendarWriter"/></param>
        public CalendarWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CalendarWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        public void WriteCalendar(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CALENDAR_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("events");

                    writer.WriteEndElement(); // Close <events>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_CALENDAR_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_CALENDAR_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
