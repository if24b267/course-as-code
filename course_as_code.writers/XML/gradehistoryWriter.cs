using System;
using System.Xml;

namespace course_as_code.writers.XML;

public class GradehistoryWriter
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true,
		NewLineOnAttributes = false,
		OmitXmlDeclaration = true
	};

	public void WriteGradehistory(string filePath = "gradehistory.xml")
	{
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			writer.WriteStartElement("grade_history"); // Root element

			writer.WriteStartElement("grade_grades");
			writer.WriteString(" ");
			writer.WriteEndElement(); // Close </grade_grades>

			writer.WriteEndElement(); // Close </grade_history>
			writer.WriteEndDocument();
		}
	}
}
