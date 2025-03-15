using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CalendarWriter : XmlWriterBase
    {
        private const string DEFAULT_CALENDAR_RELATIVE_PATH = "course/calendar.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public CalendarWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public CalendarWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
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

                    // Add whitespace to prevent self-closing tag
                    writer.WriteString("\n");

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