using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CalendarWriter : XmlWriterBase
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();
	private const string DEFAULT_CALENDAR_RELATIVE_PATH = "course/calendar.xml";
	
	public void WriteCalendar(Model.Course course, string outputPath)
	{
		try
		{
			string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CALENDAR_RELATIVE_PATH));
			
			using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("events");

				// Add whitespace to prevent self-closing tag
				writer.WriteString("\n");

				writer.WriteEndElement(); // Close <events>
				writer.WriteEndDocument();
			}
			
			_logger.Debug($"Created {DEFAULT_CALENDAR_RELATIVE_PATH}");
		}
		catch (Exception e)
		{
			_logger.Error(e, $"Error writing {DEFAULT_CALENDAR_RELATIVE_PATH}");
			throw;
		}
	}
}