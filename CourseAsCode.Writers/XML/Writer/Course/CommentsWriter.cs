using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CommentsWriter : XmlWriterBase
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();
	private const string DEFAULT_COMMENTS_RELATIVE_PATH = "course/comments.xml";
	
	public void WriteComments(Model.Course course, string outputPath)
	{
		try
		{
			string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMMENTS_RELATIVE_PATH));
                
			using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("comments");

				// Add whitespace to prevent self-closing tag
				writer.WriteString("\n");

				writer.WriteEndElement(); // Close <comments>
				writer.WriteEndDocument();
			}
                
			_logger.Debug($"Created {DEFAULT_COMMENTS_RELATIVE_PATH}");
		}
		catch (Exception e)
		{
			_logger.Error(e, $"Error writing {DEFAULT_COMMENTS_RELATIVE_PATH}");
			throw;
		}
	}
}