using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CompetenciesWriter : XmlWriterBase
{
	public void WriteCompetencies(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/competencies.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("course_competencies");

			writer.WriteStartElement("competencies");
			writer.WriteString("\n  ");
			writer.WriteEndElement(); // Close </competencies>

			writer.WriteStartElement("user_competencies");
			writer.WriteString("\n  ");
			writer.WriteEndElement(); // Close </user_competencies>

			writer.WriteEndElement(); // Close </course_competencies>
			writer.WriteEndDocument();
		}
	}
}